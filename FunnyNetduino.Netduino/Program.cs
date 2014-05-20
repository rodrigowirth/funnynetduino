using System;
using System.Threading;
using FunnyNetduino.Netduino.Commands;
using FunnyNetduino.Netduino.Settings;
using MQTT;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Net.NetworkInformation;

namespace FunnyNetduino.Netduino
{
    public class Program
    {
        private static SettingsModel _settings;
        private static IMqtt _mqtt;
        private static Timer _connectionTimer;
        private static Timer _restartTimer;
        private static Timer _stayAliveTimer;
        private static ICommand _blinkLedCommand;

        public static void Main()
        {
            try
            {
                _blinkLedCommand = CommandFactory.Create(CommandKind.BlinkLed);
                _blinkLedCommand.Execute(CreateLedBlinkCommand(3));

                SetupSettings();
                SetupConnection();
                StartConnectionTimer();
                SetupMqtt();
                StopConnectionTimer();
                StartRestartTimer();
                StartStayAliveTimer();

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Restart("General exception", ex);
            }
        }

        private static void SetupSettings()
        {
            SettingsReader reader = new SettingsReader();
            _settings = reader.GetSettings();

            reader = null;
        }

        private static void SetupConnection()
        {
            try
            {
                for (int i = 0; i <= 120; i++)
                {
                    String currentIp = NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress;

                    if (currentIp != "0.0.0.0")
                    {
                        _blinkLedCommand.Execute(CreateLedBlinkCommand(2));
                        return;
                    }

                    Thread.Sleep(500);
                }

                throw new Exception("Didn't manage to get a valid IP");
            }
            catch (Exception ex)
            {
                Restart(String.Concat("Can't setup the connection", ex.ToString()));
            }
        }

        private static void SetupMqtt()
        {
            try
            {
                _mqtt = MqttClientFactory.CreateClient(_settings.Server, _settings.Key);

                _mqtt.PublishArrived -= new PublishArrivedDelegate(MqttPublishArrived);
                _mqtt.ConnectionLost -= new ConnectionDelegate(MqttConnectionLost);

                _mqtt.Connect();

                Subscription subscription = new Subscription(_settings.Key, QoS.BestEfforts);
                _mqtt.Subscribe(subscription);

                _mqtt.PublishArrived += new PublishArrivedDelegate(MqttPublishArrived);
                _mqtt.ConnectionLost += new ConnectionDelegate(MqttConnectionLost);

                _blinkLedCommand.Execute(CreateLedBlinkCommand(2));
            }
            catch (Exception ex)
            {
                Restart("Setup MQTT has failed", ex);
            }
        }

        private static void StartConnectionTimer()
        {
            //30 second to connect to the MQTT server
            Int32 time = 30000;
            _connectionTimer = new Timer(Restart, "Timeout to connect", time, time);
        }

        private static void StopConnectionTimer()
        {
            if (_connectionTimer != null)
            {
                _connectionTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _connectionTimer.Dispose();
                _connectionTimer = null;
            }
        }

        private static void StartRestartTimer()
        {
            //The device is restarted every 4 hours
            Int32 time = 14400000;
            _connectionTimer = new Timer(Restart, "Recurrent restart", time, time);
        }

        private static void StopRestartTimer()
        {
            if (_restartTimer != null)
            {
                _restartTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        private static void StartStayAliveTimer()
        {
            //1 min for each keep alived publish
            Int32 time = 60000;
            _stayAliveTimer = new Timer(StayAlive, "Stay alive", time, time);
        }

        private static void StopStayAliveTimer()
        {
            if (_stayAliveTimer != null)
            {
                _stayAliveTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _stayAliveTimer.Dispose();
                _stayAliveTimer = null;
            }
        }

        private static void Restart(String reason)
        {
            Restart(reason, null);
        }

        private static void Restart(Object state)
        {
            Restart(state.ToString());
        }

        private static void Restart(String reason, Exception ex)
        {
            try
            {
                StopRestartTimer();
                StopStayAliveTimer();
                _blinkLedCommand.Execute(CreateLedBlinkCommand(4));
            }
            catch { }

            PowerState.RebootDevice(false);
        }

        private static void MqttConnectionLost(object sender, EventArgs e)
        {
            Restart("Connection lost");
        }

        private static bool MqttPublishArrived(object sender, PublishArrivedArgs e)
        {
            try
            {
                String commandLine = e.Payload.ToString();
                String[] commandsLineDetails = commandLine.Split(';');

                CommandKind commandKind = (CommandKind)Int32.Parse(commandsLineDetails[0]);

                ICommand command = CommandFactory.Create(commandKind);
                command.Execute(commandLine);

                command = null;
                commandsLineDetails = null;
                commandLine = null;
            }
            catch (Exception ex)
            {
                Restart("Fail to process a command", ex);
            }

            return true;
        }

        private static void StayAlive(Object state)
        {
            var topic = String.Concat("SA", _settings.Key);
            var parcel = new MqttParcel(topic, new MqttPayload(topic), QoS.BestEfforts, false);

            _mqtt.Publish(parcel);
        }

        private static String CreateLedBlinkCommand(Int16 amount)
        {
            return String.Concat("1;", amount);
        }
    }
}
