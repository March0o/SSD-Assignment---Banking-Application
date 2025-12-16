using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SSD_Assignment___Banking_Application
{
    public class Logger
    {
        private EventLog eventLog;
        public Logger() { 

            if (!EventLog.SourceExists("SSD Banking Application"))
            {
                EventLog.CreateEventSource("SSD Banking Application", "Application");
                Console.WriteLine("CreatedEventSource");
                Console.WriteLine("Exiting, execute the application a second time to use the source.");
                return;
            }

            // Create an EventLog instance and assign its source.
            eventLog = new EventLog();
            eventLog.Source = "SSD Banking Application";
            eventLog.WriteEntry("Setup Complete");
        }

        public void Log(string teller, string accNo, string type, string identifier, string reason, string metadata)
        {
            DateTime now = DateTime.Now;
            if (reason == "") { reason = "N/A"; }
            eventLog.WriteEntry($"Teller: {teller}\tAccount Number: {accNo}\nType: {type}\tTime: {now}\nReason: {reason}\tMetadata: {metadata}");
        }
    }
}
