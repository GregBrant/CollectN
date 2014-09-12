using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using CollectN.Core;

namespace CollectN.Configurators
{
    class DaemonConfigurator : IConfigurationWhatsit
    {
        private const string HostnameKey = "Hostname";
        private const string FQDNookupKey = "FQDNLookup";

        public void Munge(ApplicationConfiguration config, ConfigurationFile configFile)
        {
            MungeHostName(config, configFile);
        }

        /// <summary>
        /// Handles the setting of the Hostname and FQDNLookup values.
        /// </summary>
        /// <param name="config">The Application Configuration.</param>
        /// <param name="configFile">The conf file content.</param>
        private void MungeHostName(ApplicationConfiguration config, ConfigurationFile configFile)
        {
            if (configFile.ContainsKey(HostnameKey))
            {
                config.Hostname = configFile[HostnameKey];
            }
            else if (configFile.ContainsKey(FQDNookupKey) &&
                     configFile[FQDNookupKey].IsTrueString())
            {
                config.Hostname = GetFQDN();
            }
            else
            {
                config.Hostname = Environment.MachineName;
            }
        }

        public string GetFQDN()
        {
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();

            if (!hostName.Contains(domainName))            
            {
                hostName = hostName + "." + domainName;   
            }

            return hostName;
        }
    }
}
