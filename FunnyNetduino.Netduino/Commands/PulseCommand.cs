using System;
using System.Threading;
using FunnyNetduino.Netduino.Infrastructure;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace FunnyNetduino.Netduino.Commands
{
    public class PulseCommand : ICommand
    {
        public void Execute(String commandLine)
        {
            String[] commandLineDetails = commandLine.Split(';');
            OutputPort port = Ports.Instance.GetOutputPort(Int32.Parse(commandLineDetails[1]));

            port.Write(true);
            Thread.Sleep(200);
            port.Write(false);
        }
    }
}
