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

        public static String ShortCity(String s)
        {
            return s.Replace("Thành Phố","TP. ");
        }

        public static String GenerateID()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string id = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');
            return id;
        }
    }
}