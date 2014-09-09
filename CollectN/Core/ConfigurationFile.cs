using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectN.Core
{
    public class ConfigurationFile
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>(); 

        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }

        public string this[string key]
        {
            get { return _data[key] as string; }
            set { _data[key] = value; }
        }
    }
}
