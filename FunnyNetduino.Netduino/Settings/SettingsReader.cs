using System;
using Microsoft.SPOT;

namespace FunnyNetduino.Netduino.Settings
{
    public class SettingsReader
    {
        public SettingsModel GetSettings()
        {
            return new SettingsModel
            {                
                Key = "00001",
                Server = "tcp://devndomusworker.cloudapp.net:1883"
            };
        }
    }   
}
