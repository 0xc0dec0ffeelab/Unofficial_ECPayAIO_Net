using System.Security.Cryptography;
using System.Text;

namespace ECPay.Payment.Integration
{
    internal static class SHA256Encoder
    {
        private static readonly HashAlgorithm Crypto;

        static SHA256Encoder()
        {
            Crypto = new SHA256CryptoServiceProvider();
        }

        public static string Encrypt(string originalString)
        {
            byte[] bytes = Encoding.Default.GetBytes(originalString);
            byte[] array = Crypto.ComputeHash(bytes);
            string text = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                text += array[i].ToString("X2");
            }
            return text.ToUpper();
        }
    }
}
