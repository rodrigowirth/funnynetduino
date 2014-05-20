using System.Configuration;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MqttLib;

namespace FunnyNetduino.Web.Hubs
{
    public class CommandHub : Hub
    {
        private const String MQTT_TOPIC = "funnynetduino";

        private static IMqtt _mqttClient;

        private static IMqtt MqttClient
        {
            get
            {
                if (_mqttClient == null)
                {
                    String mqttServer = ConfigurationManager.AppSettings["MqttServer"];
                    _mqttClient = MqttClientFactory.CreateClient(mqttServer, "FunnyNetduino.Web.CommandHub");
                }

                if (!_mqttClient.IsConnected)
                    _mqttClient.Connect();

                return _mqttClient;
            }
        }

        public void SendCommand(String commandLine)
        {
            MqttClient.Publish(MQTT_TOPIC, new MqttPayload(commandLine), QoS.BestEfforts, false);
        }
    }
}