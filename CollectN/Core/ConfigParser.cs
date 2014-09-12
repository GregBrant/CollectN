using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Targets;

namespace CollectN.Core
{
    public class ConfigParser
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        public ConfigurationFile Parse(string path)
        {
            return Parse(File.OpenRead(path));
        }

        public ConfigurationFile Parse(Stream stream)
        {
            var config = new ConfigurationFile();

            using (var reader = new StreamReader(stream))
            {
                int lineNumber = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    if (IsConfigLine(line))
                    {
                        ParseLine(config, line, lineNumber);
                    }
                }
            }

            return config;
        }

        private bool IsConfigLine(string line)
        {
            var trimmed = line.Trim();
            if (trimmed.Length == 0 || trimmed[0] == '#')
            {
                return false;
            }

            return true;
        }

        private void ParseLine(ConfigurationFile config, string line, int lineNumber)
        {
            // TODO: Parse quoted string values
            var space = line.IndexOf(' ');
            var key = line.Substring(0, space);
            var value = line.Substring(space + 1);

            if (config.ContainsKey(key))
            {
                _log.Debug("Overwriting value for '{0}' at like {1}", key, lineNumber);
            }

            config[key] = value;
        }
    }
}
