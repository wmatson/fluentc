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

        public static bool IsCondition(this string value)
        {
            bool syntax;
            return bool.TryParse(value, out syntax);
        }

        public static dynamic ToNumber(this string value)
        {
            if (value.IsNumber())
                return decimal.Parse(value);
            return value;
        }
    }
}
