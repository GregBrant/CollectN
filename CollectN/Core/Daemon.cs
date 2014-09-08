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
            // TODO: Load Plugins

            plugins = new IInputPlugin[]
            {
                new CpuPlugin(),
                new MemoryPlugin(), 
            };

            writers = new IWriterPlugin[]
            {
                new Plugins.Write.Graphite(),
            };

            _timer = new Timer(10 * 1000);
            _timer.AutoReset = true;
            _timer.Elapsed += (sender, args) => SignalAllPlugins();
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _timer.Dispose();
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

                Parallel.ForEach(writers, x => x.Write("collectd", Environment.MachineName, stats));
            }
        }
    }
}