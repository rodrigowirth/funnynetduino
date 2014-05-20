using System;
using Microsoft.SPOT;

namespace FunnyNetduino.Netduino.Commands
{
    public interface ICommand
    {
        void Execute(String commandLine);
    }
}
