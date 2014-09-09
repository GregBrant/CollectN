using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectN.Core;
using CollectN.Debug;

namespace CollectN.Plugins
{
    class InterfacePlugin : IInputPlugin
    {
        private readonly PerformanceCounter[] _counters;

        public InterfacePlugin()
        {
            using (Profiler.Step("Interface Init"))
            {
                var category = new PerformanceCounterCategory("Network Interface");

                // TODO: This is too slow. Need to define a default list of counters
                //       which can be overridden in the config file.
                _counters = (from name in category.GetInstanceNames()
                             select category.GetCounters(name))
                            .SelectMany(x => x)
                            .ToArray();
            }
        }

        public IEnumerable<StatResult> Signal()
        {
            using (Profiler.Step("Interface Signal"))
            {
                var resultCollection = new List<StatResult>();

                foreach (var performanceCounter in _counters)
                {
                    var value = performanceCounter.NextValue();
                    var name = ("interface-" + performanceCounter.InstanceName +
                                "." + performanceCounter.CounterName.Replace("/", "-")).Replace(" ", "-").ToLower();
                    resultCollection.Add(new StatResult { Key = name, Value = (int)value });
                }

                return resultCollection;
            }
        }
    }
}
