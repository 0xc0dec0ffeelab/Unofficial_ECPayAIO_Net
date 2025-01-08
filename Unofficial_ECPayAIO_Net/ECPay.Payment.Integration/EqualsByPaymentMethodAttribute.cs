using System;
using System.ComponentModel.DataAnnotations;

namespace ECPay.Payment.Integration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class EqualsByPaymentMethodAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            object[]? array = value as object[];
            _ = array?[0];
            object? obj2 = array?[1];
            _ = array?[2];
            _ = array?[3];
            string? text = obj2?.ToString();
            if (!string.IsNullOrEmpty(text))
            {
                string[] array2 = text.Split('#');
                string[] array3 = array2;
                foreach (string value2 in array3)
                {
                    try
                    {
                        if ((PaymentMethod)Enum.Parse(typeof(PaymentMethod), value2) == PaymentMethod.ALL)
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
