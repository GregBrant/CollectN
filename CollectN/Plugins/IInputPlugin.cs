using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CollectN.Core;

namespace CollectN.Plugins
{
    interface IInputPlugin
    {
        IEnumerable<StatResult> Signal();
    }
}
