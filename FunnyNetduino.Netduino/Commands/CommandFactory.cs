using System;
using Microsoft.SPOT;

namespace FunnyNetduino.Netduino.Commands
{
    public static class CommandFactory
    {
        public static ICommand Create(CommandKind kind)
        {
            switch (kind)
            {
                case CommandKind.Pulse:
                    return new PulseCommand();
                case CommandKind.Turn:
                    return new TurnCommand();
                case CommandKind.TurnOn:
                    return new TurnOnCommand();
                case CommandKind.TurnOff:
                    return new TurnOffCommand();
                case CommandKind.BlinkLed:
                    return new BlinkLedCommand();
                default:
                    throw new Exception("Command kind is not valid");
            }
        }
    }

    public enum CommandKind
    {
        Pulse = 0,
        Turn = 1,
        TurnOn = 2,
        TurnOff = 3,
        BlinkLed = 4
    }
}
