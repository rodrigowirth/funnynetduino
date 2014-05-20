using System;
using System.Threading;
using FunnyNetduino.Netduino.Infrastructure;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace FunnyNetduino.Netduino.Commands
{
    public class TurnCommand : ICommand
    {
        public void Execute(String commandLine)
        {
            String[] commandLineDetails = commandLine.Split(';');
            OutputPort port = Ports.Instance.GetOutputPort(Int32.Parse(commandLineDetails[1]));

            Boolean currentStatus = port.Read();
            port.Write(!currentStatus);
            
        }
    }
}
