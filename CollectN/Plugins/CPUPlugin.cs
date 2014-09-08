using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectN.Debug;

namespace CollectN.Plugins
{
    class CpuPlugin : IInputPlugin
    {
        private readonly PerformanceCounter[] _counters;

        public CpuPlugin()
        {
            using (Profiler.Step("CPU Init"))
            {
                var category = new PerformanceCounterCategory("Processor");

                _counters = (from name in category.GetInstanceNames()
                             select category.GetCounters(name))
                            .SelectMany(x => x)
                            .ToArray();
            }
        }

        public IEnumerable<StatResult> Signal()
        {
            using (Profiler.Step("CPU Signal"))
            {
                var resultCollection = new List<StatResult>();

                foreach (var performanceCounter in _counters)
                {
                    var value = performanceCounter.NextValue();
                    var name = ("cpu" + performanceCounter.InstanceName +
                                "." + performanceCounter.CounterName.Replace("/", "-")).Replace(" ", "-").ToLower();
                    resultCollection.Add(new StatResult { Key = name, Value = (int)value });
                }

                return resultCollection;
            }
        }
    }
}
