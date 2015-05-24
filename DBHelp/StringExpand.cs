using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALOS.DBHelp
{
    public static class StringExpand
    {
        public static bool IsNull(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
