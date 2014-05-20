using System;
using System.Threading;
using FunnyNetduino.Netduino.Infrastructure;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace FunnyNetduino.Netduino.Commands
{
    public class BlinkLedCommand : ICommand
    {
        public void Execute(String commandLine)
        {
            try
            {
                String[] commandLineDetails = commandLine.Split(';');
                Int16 amount = Convert.ToInt16(commandLineDetails[1]);

                OutputPort port = Ports.Instance.GetLetOnBoard();

                for (int i = 0; i < amount; i++)
                {
                    port.Write(true);
                    Thread.Sleep(300);
                    port.Write(false);
                    Thread.Sleep(300);
                }
            }
            catch { }
        }
    }
}
