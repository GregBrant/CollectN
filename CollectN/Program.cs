using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectN.Core;
using Topshelf;

namespace CollectN
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
        {
            x.Service<Daemon>(s =>
            {
                s.ConstructUsing(name => new Daemon());
                s.WhenStarted(tc => tc.Start());
                s.WhenStopped(tc => tc.Stop());
            });
            x.RunAsLocalSystem();

            x.SetDescription("CollectN - A collectd clone for Windows.");
            x.SetDisplayName("CollectN");
            x.SetServiceName("CollectN");
        });
        }
    }
}
