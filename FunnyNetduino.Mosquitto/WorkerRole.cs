using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

namespace FunnyNetduino.Mosquitto
{
    public class WorkerRole : RoleEntryPoint
    {
        Process _program = new Process();

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("$projectname$ entry point called", "Information");

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            try
            {
                DiagnosticMonitorConfiguration diagnosticConfig = DiagnosticMonitor.GetDefaultInitialConfiguration();
                diagnosticConfig.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);
                diagnosticConfig.Logs.ScheduledTransferLogLevelFilter = LogLevel.Verbose;
                DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", diagnosticConfig);

                // Set the maximum number of concurrent connections 
                ServicePointManager.DefaultConnectionLimit = 12;

                String rsbroot = Path.Combine(Environment.GetEnvironmentVariable("RoleRoot") + @"\\", @"approot\\Mosquitto");
                Int32 port = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["WorkerIn"].IPEndpoint.Port;

                ProcessStartInfo pInfo = new ProcessStartInfo(Path.Combine(rsbroot, @"mosquitto.exe"))
                {
                    UseShellExecute = false,
                    WorkingDirectory = rsbroot,
                    ErrorDialog = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                _program.StartInfo = pInfo;
                _program.OutputDataReceived += new DataReceivedEventHandler(Program_OutputDataReceived);
                _program.ErrorDataReceived += new DataReceivedEventHandler(Program_ErrorDataReceived);
                _program.Start();
                _program.BeginOutputReadLine();
                _program.BeginErrorReadLine();

                Trace.WriteLine("Completed OnStart", "Information");
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed OnStart: {0}", ex.ToString());
            }
            return false;
        }

        void Program_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                string output = e.Data;
                Trace.WriteLine("[…] Standard output –> " + output, "Information");
            }
        }

        void Program_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                string output = e.Data;
                Trace.WriteLine("[…] Standard output –> " + output, "Information");
            }
        }
    }
}
