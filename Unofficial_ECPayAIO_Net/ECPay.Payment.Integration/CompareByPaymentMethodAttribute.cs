using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECPay.Payment.Integration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class CompareByPaymentMethodAttribute : RequiredAttribute
    {
        public PaymentMethod PaymentMethod { get; set; }

        public string ConfirmPropertyNames { get; set; }

        public CompareByPaymentMethodAttribute(PaymentMethod paymentMethod, string confirmPropertyNames)
        {
            PaymentMethod = paymentMethod;
            ConfirmPropertyNames = confirmPropertyNames ?? throw new ArgumentNullException("confirmPropertyNames is null.");
        }

        public override bool IsValid(object? value)
        {
            object[]? array = value as object[];
            _ = array?[0];
            object? obj2 = array?[1];
            object? component = array?[2];
            _ = array?[3];
            string[] array2 = ConfirmPropertyNames.Split('/');
            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(component);
            var value2 = propertyDescriptorCollection.Find("_PaymentMethod", ignoreCase: true).GetValue(component);
            if (PaymentMethod.Equals(value2) && IsValid(obj2) && !PeriodType.None.Equals(obj2))
            {
                string[] array3 = array2;
                foreach (string name in array3)
                {
                    var value3 = propertyDescriptorCollection.Find(name, ignoreCase: true).GetValue(component);
                    if (!PeriodType.None.Equals(value3) && null != value3)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
