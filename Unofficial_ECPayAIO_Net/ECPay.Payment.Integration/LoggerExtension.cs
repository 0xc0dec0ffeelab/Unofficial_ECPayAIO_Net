using System.Diagnostics;

namespace ECPay.Payment.Integration
{
    internal static class Logger
    {
        internal static void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
