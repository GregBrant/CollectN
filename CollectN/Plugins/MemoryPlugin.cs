using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectN.Debug;

namespace CollectN.Plugins
{
    class MemoryPlugin : IInputPlugin
    {
        private readonly PerformanceCounter _availableBytes;

        public MemoryPlugin()
        {
            using (Profiler.Step("Memory Init"))
            {
                _availableBytes = new PerformanceCounter();
                _availableBytes.CategoryName = "memory";
                _availableBytes.CounterName = "Available MBytes";
                // Prime the counter 
                _availableBytes.NextValue();
            }
        }

        public void Signal()
        {
            using (Profiler.Step("Memory Signal"))
            {
                var available = _availableBytes.NextValue();
                Console.WriteLine("Memory:\tAvailable MBytes\t{0}", available);
            }
        }
    }
}
