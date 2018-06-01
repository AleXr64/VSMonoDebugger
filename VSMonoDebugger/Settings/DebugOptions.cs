﻿using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace VSMonoDebugger.Settings
{
    public class DebugOptions
    {
        public bool UseSSH { get; set; }
        public UserSettings UserSettings { get; set; }        
        public string OutputDirectory { get; set; }
        public string TargetExeFileName { get; set; }
        public string StartArguments { get; set; }
        public string StartupAssemblyPath { get; set; }
        public string PreDebugScript { get; set; }
        public string DebugScript { get; set; }

        public DebugOptions()
        { }
        
        public string SerializeToJson()
        {
            string json = JsonConvert.SerializeObject(this);
            return json;
        }

        public static DebugOptions DeserializeFromJson(string json)
        {
            var result = JsonConvert.DeserializeObject<DebugOptions>(json);
            return result;
        }

        public IPAddress GetHostIP()
        {
            var hostIp = IPAddress.Loopback;
            if (UseSSH)
            {
                if (!IPAddress.TryParse(UserSettings.SSHHostIP, out hostIp))
                {
                    // try dns
                    hostIp = Dns.GetHostAddresses(UserSettings.SSHHostIP).First();
                }
            }
            else
            {
                hostIp = IPAddress.Parse(UserSettings.LastIp);
            }
            return hostIp;
        }
        
        public int GetMonoDebugPort()
        {
            return UseSSH ? UserSettings.SSHMonoDebugPort : UserSettings.DEFAULT_DEBUGGER_AGENT_PORT;            
        }
    }
}
