using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanHang.Utils
{
    public static class EncryptUtils
    {
        public static byte[] encryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }
        public static string MD5(this String data)
        {
            return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
        }

        public static bool PwdCompare(String plainPass, String encryptPass)
        {
            if (String.IsNullOrEmpty(plainPass) || String.IsNullOrEmpty(encryptPass))
                return false;
            plainPass = plainPass.Trim();
            encryptPass = encryptPass.Trim();
            return encryptPass.Equals(plainPass.MD5());
        }
    }
}