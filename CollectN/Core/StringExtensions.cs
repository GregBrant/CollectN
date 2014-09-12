using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectN.Core
{
    public static class StringExtensions
    {
        public static bool IsTrueString(this string instance)
        {
            bool val;
            bool.TryParse(instance, out val);
            return val;
        }
    }
}
