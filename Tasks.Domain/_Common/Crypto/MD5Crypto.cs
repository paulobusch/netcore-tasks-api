using System;
using System.Security.Cryptography;
using System.Text;

namespace Tasks.Domain._Common.Crypto
{
    // font: https://stackoverflow.com/questions/3683274/encode-password-to-md5-using-keys
    public static class MD5Crypto
    {
        public static string Encode(string original)
        {
            byte[] encodedBytes;
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var originalBytes = Encoding.Default.GetBytes(original);
                encodedBytes = md5.ComputeHash(originalBytes);
            }

            return Convert.ToBase64String(encodedBytes);
        }
    }
}
