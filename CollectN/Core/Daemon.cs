using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CollectN.Debug;
using CollectN.Plugins;

namespace CollectN.Core
{
    internal class Daemon
    {
        private Timer _timer;

        private IInputPlugin[] plugins;

        public void Start()
        {
            // TODO: Load Plugins

            plugins = new[]
            {
                new CpuPlugin()
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
                foreach (var inputPlugin in plugins)
                {
                    inputPlugin.Signal();
                }
            }
        }
    }
}