﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectN.Core
{
    public class ConfigurationFile
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        private readonly List<string> _plugins = new List<string>();

        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }

        public string this[string key]
        {
            get { return _data[key] as string; }
            set { _data[key] = value; }
        }

        public int Count
        {
            get { return _data.Count; }
        }

        public ICollection<string> Plugins
        {
            get { return _plugins; }
        }
    }
}
