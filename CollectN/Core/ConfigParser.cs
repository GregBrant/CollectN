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

        public ApplicationConfiguration Parse(string path)
        {
            throw new NotImplementedException();
        }

        public ApplicationConfiguration Parse(Stream stream)
        {
            var config = new ApplicationConfiguration();

            using (var reader = new StreamReader(stream))
            {
                int lineNumber = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    ParseLine(config, line, lineNumber);
                }
            }

            return config;
        }

        private void ParseLine(ApplicationConfiguration config, string line, int lineNumber)
        {
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
