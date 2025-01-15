using System.Security.Cryptography;
using System.Text;

namespace ECPay.Payment.Integration
{
    internal static class MD5Encoder
    {
        private static readonly HashAlgorithm Crypto;

        static MD5Encoder()
        {
            Crypto = MD5.Create();
        }

        public static string Encrypt(string originalString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(originalString);
            byte[] array = Crypto.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("X").PadLeft(2, '0'));
            }
            return stringBuilder.ToString().ToUpper();
        }
    }

}
