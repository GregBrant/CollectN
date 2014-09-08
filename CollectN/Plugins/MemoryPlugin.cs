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
        private readonly PerformanceCounter[] _counters;

        public MemoryPlugin()
        {
            using (Profiler.Step("Memory Init"))
            {
                var category = new PerformanceCounterCategory("Memory");

                _counters = category.GetCounters();
            }
        }

        public IEnumerable<StatResult> Signal()
        {
            using (Profiler.Step("Memory Signal"))
            {
                var resultCollection = new List<StatResult>();

                foreach (var performanceCounter in _counters)
                {
                    var value = performanceCounter.NextValue();
                    var name = ("memory" + performanceCounter.InstanceName +
                                "." + performanceCounter.CounterName.Replace("/", "-")).Replace(" ", "-").ToLower();
                    resultCollection.Add(new StatResult { Key = name, Value = (int)value });
                }

                return resultCollection;
            }
        }
    }
}
