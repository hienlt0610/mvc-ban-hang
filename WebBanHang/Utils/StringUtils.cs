using System;
using System.Collections.Generic;

namespace WebBanHang.Utils
{
    public static class StringUtils
    {
        public static int ToIntWithDef(this String s, int def)
        {
            int temp;
            if (!Int32.TryParse(s, out temp))
                return def;
            return temp;
        }
    }
}