using System.Text;

namespace ECPay.Payment.Integration.Helpers
{
    public class CharSetHelper
    {
        public static Encoding GetCharSet(CharSetState CharSet)
        {
            return CharSet switch
            {
                CharSetState.Big5 => Encoding.GetEncoding("Big5"),
                CharSetState.UTF8 => Encoding.UTF8,
                _ => Encoding.Default,
            };
        }
    }
}
