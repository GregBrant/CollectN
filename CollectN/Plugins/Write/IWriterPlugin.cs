﻿using System.Collections;
using System.Collections.Generic;
using CollectN.Core;

namespace CollectN.Plugins.Write
{
    public interface IWriterPlugin
    {
        void Write(string application, IEnumerable<StatResult> data);
    }
}