using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CollectN.Debug;
using CollectN.Plugins;
using CollectN.Plugins.Write;
using Graphite = CollectN.Plugins.Write.Graphite;
using Timer = System.Timers.Timer;

namespace CollectN.Core
{
    internal class Daemon
    {
        private Timer _timer;

        private IInputPlugin[] plugins;
        private IWriterPlugin[] writers;

        public void Start()
        {
            var configFile = LoadConfig();
            var config = ProcessConfig(configFile);

            // TODO: Load Plugins

            plugins = new IInputPlugin[]
            {
                new CpuPlugin(config),
                new MemoryPlugin(config), 
                // Interface plugin removed until it's performance is improved
                // new InterfacePlugin(config), 
            };

            writers = new IWriterPlugin[]
            {
                new Plugins.Write.Graphite(config),
            };

            _timer = new Timer(10 * 1000);
            _timer.AutoReset = true;
            _timer.Elapsed += (sender, args) => SignalAllPlugins();
            _timer.Start();
        }

        private ApplicationConfiguration ProcessConfig(ConfigurationFile configFile)
        {
            var x = typeof(IConfigurationWhatsit);
            var interestedParties = AppDomain.CurrentDomain.GetAssemblies()
                                             .ToList()
                                             .SelectMany(a => a.GetTypes())
                                             .Where(t => x.IsAssignableFrom(t))
                                             .Where(t => t.IsClass)
                                             .Where(t => !t.IsAbstract);

            var config = new ApplicationConfiguration();
            foreach (var type in interestedParties)
            {
                var instance = (IConfigurationWhatsit)Activator.CreateInstance(type);
                instance.Munge(config, configFile);
            }
            return config;
        }

        public void Stop()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        private ConfigurationFile LoadConfig()
        {
            var parser = new ConfigParser();
            var config = parser.Parse("collectn.conf");
            return config;
        }

        private void SignalAllPlugins()
        {
            Console.WriteLine("Signalling plugins");
            using (Profiler.Step("Signalling plugins"))
            {
                var stats = (from plugin in plugins.AsParallel()
                             select plugin.Signal())
                             .SelectMany(x => x)
                             .ToList();

                Parallel.ForEach(writers, x => x.Write("collectd", stats));
            }
        }
    }
}