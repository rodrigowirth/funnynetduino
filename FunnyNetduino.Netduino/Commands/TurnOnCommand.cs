using System;
using FunnyNetduino.Netduino.Infrastructure;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace FunnyNetduino.Netduino.Commands
{
    public class TurnOnCommand : ICommand
    {
        public void Execute(String commandLine)
        {
            String[] commandLineDetails = commandLine.Split(';');
            OutputPort port = Ports.Instance.GetOutputPort(Int32.Parse(commandLineDetails[1]));

            port.Write(true);

        }
    }
}
