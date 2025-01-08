using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECPay.Payment.Integration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class RequiredByPaymentMethodAttribute : RequiredAttribute
    {
        public PaymentMethod PaymentMethod { get; set; }

        public RequiredByPaymentMethodAttribute(PaymentMethod paymentMethod)
        {
            PaymentMethod = paymentMethod;
        }

        public override bool IsValid(object? value)
        {
            object[]? array = value as object[];
            _ = array?[0];
            object? value2 = array?[1];
            object? component = array?[2];
            _ = array?[3];
            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(component);
            object? value3 = propertyDescriptorCollection.Find("_PaymentMethod", ignoreCase: true).GetValue(component);
            if (PaymentMethod.Equals(value3))
            {
                return IsValid(value2);
            }
            return true;
        }
    }

}
