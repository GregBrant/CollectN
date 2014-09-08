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
        private readonly PerformanceCounter _idleTime;
        private readonly PerformanceCounter _processorTime;
        private readonly PerformanceCounter _userTime;
        private readonly PerformanceCounter _systemTime;
        private readonly PerformanceCounter _interruptTime;

        public CpuPlugin()
        {
            using (Profiler.Step("CPU Init"))
            {
                _idleTime = new PerformanceCounter();
                _idleTime.CategoryName = "Processor";
                _idleTime.CounterName = "% Idle Time";
                _idleTime.InstanceName = "_Total";
                // Prime the counter 
                _idleTime.NextValue();

                
                _processorTime = new PerformanceCounter();
                _processorTime.CategoryName = "Processor";
                _processorTime.CounterName = "% Processor Time";
                _processorTime.InstanceName = "_Total";
                // Prime the counter 
                _processorTime.NextValue();

                _userTime = new PerformanceCounter();
                _userTime.CategoryName = "Processor";
                _userTime.CounterName = "% User Time";
                _userTime.InstanceName = "_Total";
                // Prime the counter 
                _userTime.NextValue();

                _systemTime = new PerformanceCounter();
                _systemTime.CategoryName = "Processor";
                _systemTime.CounterName = "% Privileged Time";
                _systemTime.InstanceName = "_Total";
                // Prime the counter 
                _systemTime.NextValue();

                

                _interruptTime = new PerformanceCounter();
                _interruptTime.CategoryName = "Processor";
                _interruptTime.CounterName = "% Interrupt Time";
                _interruptTime.InstanceName = "_Total";
                // Prime the counter 
                _interruptTime.NextValue();
            }
        }

        public void Signal()
        {
            using (Profiler.Step("CPU Signal"))
            {
                var idle = _idleTime.NextValue();
                Console.WriteLine("CPU:\t% Idle Time\t{0}%", idle);

                var processor = _processorTime.NextValue();
                Console.WriteLine("CPU:\t% Processor Time\t{0}%", processor);
                
                var user = _userTime.NextValue();
                Console.WriteLine("CPU:\t% User Time\t{0}%", user);

                var priv = _userTime.NextValue();
                Console.WriteLine("CPU:\t% Privileged Time\t{0}%", priv);

                var interrupt = _userTime.NextValue();
                Console.WriteLine("CPU:\t% Interrupt Time\t{0}%", interrupt);
            }
        }
    }
}
