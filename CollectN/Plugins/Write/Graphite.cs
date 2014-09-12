using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectN.Core;
using CollectN.Debug;
using Graphite;

namespace CollectN.Plugins.Write
{
    class Graphite : IWriterPlugin
    {
        private GraphiteTcpClient _client;

        private ApplicationConfiguration _config;
        
        public Graphite(ApplicationConfiguration config)
        {
            using (Profiler.Step("Graphite Init"))
            {
                _config = config;
                _client = new GraphiteTcpClient("192.168.10.34", 2003);
            }
        }

        public void Write(string application, IEnumerable<StatResult> data)
        {
            using (Profiler.Step("Graphite Signal"))
            {
                foreach (var item in data)
                {
                    // Report a metric
                    var name = string.Join(".", application, _config.Hostname, item.Key).ToLower();
                    Console.WriteLine(name, item.Value);
                    _client.Send(name, item.Value);
                }


                //// Report a metric specifying timestamp
                //_client.Send("baz", i++, DateTime.Now.AddSeconds(42));
            }
        }
    }
}
