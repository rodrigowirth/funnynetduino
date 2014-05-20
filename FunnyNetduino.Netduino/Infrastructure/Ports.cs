using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace FunnyNetduino.Netduino.Infrastructure
{
    public class Ports
    {
        private static Ports _instance;
        private OutputPort[] _outputPorts;
        private OutputPort _ledOnBoard;

        public static Ports Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Ports();
                return _instance;
            }
        }

        private Ports()
        {
            this.SetupOutputPorts();
            this.SetupLedOnBoard();
        }

        public OutputPort GetOutputPort(Int32 index)
        {
            return _outputPorts[index];
        }

        public OutputPort GetLetOnBoard()
        {
            return _ledOnBoard;
        }

        private void SetupOutputPorts()
        {
            _outputPorts = new OutputPort[14];
            _outputPorts[0] = new OutputPort(Pins.GPIO_PIN_D0, false);
            _outputPorts[1] = new OutputPort(Pins.GPIO_PIN_D1, false);
            _outputPorts[2] = new OutputPort(Pins.GPIO_PIN_D2, false);
            _outputPorts[3] = new OutputPort(Pins.GPIO_PIN_D3, false);
            _outputPorts[4] = new OutputPort(Pins.GPIO_PIN_D4, false);
            _outputPorts[5] = new OutputPort(Pins.GPIO_PIN_D5, false);
            _outputPorts[6] = new OutputPort(Pins.GPIO_PIN_D6, false);
            _outputPorts[7] = new OutputPort(Pins.GPIO_PIN_D7, false);
            _outputPorts[8] = new OutputPort(Pins.GPIO_PIN_D8, false);
            _outputPorts[9] = new OutputPort(Pins.GPIO_PIN_D9, false);
            _outputPorts[10] = new OutputPort(Pins.GPIO_PIN_D10, false);
            _outputPorts[11] = new OutputPort(Pins.GPIO_PIN_D11, false);
            _outputPorts[12] = new OutputPort(Pins.GPIO_PIN_D12, false);
            _outputPorts[13] = new OutputPort(Pins.GPIO_PIN_D13, false);
        }

        private void SetupLedOnBoard()
        {
            _ledOnBoard = new OutputPort(Pins.ONBOARD_LED, false);
        }
    }
}
