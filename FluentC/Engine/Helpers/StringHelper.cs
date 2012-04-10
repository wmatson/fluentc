using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCEngine.Helpers
{
    public static class StringHelper
    {
        public static bool IsNumber(this string value)
        {
            decimal syntax;
            return decimal.TryParse(value, out syntax);
        }
    }
}
