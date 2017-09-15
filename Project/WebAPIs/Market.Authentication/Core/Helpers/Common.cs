using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Market.Authentication.Core.Helpers
{
    public static class Common
    {
        public static string GetEnvDesc()
        {
            return string.Format("{0} | {1} | {2} | {3} ({4}) | {5} | {6} | {7} | {8}", Environment.UserDomainName, Environment.UserName, Environment.MachineName, Environment.OSVersion, Environment.Is64BitOperatingSystem ? "x64" : "x86", GetOsName(), Environment.Version, GetProcessorSerial(), GetHddSerial()).Trim().ToUpper();
        }

        public static string GetOsName()
        {
            var result = string.Empty;
            var searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");

            foreach (ManagementObject os in searcher.Get())
            {
                result = os["Caption"].ToString();
                break;
            }

            return result;
        }

        public static string GetProcessorSerial()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            ManagementObjectCollection managementObjects = searcher.Get();

            foreach (ManagementObject obj in managementObjects)
            {
                if (obj["SerialNumber"] != null)
                    return obj["SerialNumber"].ToString();
            }

            return String.Empty;
        }

        public static string GetHddSerial()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            ManagementObjectCollection managementObjects = searcher.Get();

            foreach (ManagementObject obj in managementObjects)
            {
                if (obj["SerialNumber"] != null)
                    return obj["SerialNumber"].ToString();
            }

            return string.Empty;
        }
    }
}