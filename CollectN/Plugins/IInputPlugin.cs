﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollectN.Plugins
{
    interface IInputPlugin
    {
        IEnumerable<StatResult> Signal();
    }
}
