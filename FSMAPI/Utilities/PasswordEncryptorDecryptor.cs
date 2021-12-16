using System;
using System.Text;

namespace FSMAPI.Utilities
{

    public static class PasswordEncryptorDecryptor
    {
        public static string Encrypt(this string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(valueBytes);
        }

        public static string Decrypt(this string value)
        {
            var valueBytes = System.Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(valueBytes);
        }
    }
}
