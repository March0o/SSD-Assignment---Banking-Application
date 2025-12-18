using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using Banking_Application;

namespace SSD_Assignment___Banking_Application
{
    public class Logger
    {
        private EventLog eventLog;
        public Logger()
        {

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

        public void Log(string teller, string accNo, string name, Transaction_Type type, string reason)
        {
            DateTime now = DateTime.Now;
            string ip = GetLocalIPAddress();

            // https://learn.microsoft.com/en-us/dotnet/fundamentals/reflection/viewing-type-information
            // Get Metadata about calling assembly
            Assembly callingAssembly = Assembly.GetCallingAssembly();
            string assemblyName = callingAssembly.GetName().Name.ToString();
            double version = callingAssembly.GetName().Version.Major + (callingAssembly.GetName().Version.Minor / 10.0);
            int hashCode = callingAssembly.GetHashCode();
            string metadata = "Assembly: " + assemblyName + " | Version: " + version + " | HashCode: " + hashCode;

            if (reason == "") { reason = "N/A"; }
            eventLog.WriteEntry(
                $"Teller: {teller}\n" +
                $"Account: {accNo} | {name}\n" +
                $"Type: {(int)type}\n" +
                $"Time: {now}\n" +
                $"IP: {ip}\n" +
                $"Reason: {reason}\n" +
                $"Metadata: {metadata}");
        }
        // Source - https://stackoverflow.com/questions/6803073/get-local-ip-address
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}

