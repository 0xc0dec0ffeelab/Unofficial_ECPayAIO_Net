using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECPay.Payment.Integration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class ValidateMerchantMemberIDAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            object[]? array = value as object[];
            _ = array?[0];
            object? obj2 = array?[1];
            object? component = array?[2];
            _ = array?[3];
            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(component);
            var value2 = propertyDescriptorCollection.Find("BindingCard", ignoreCase: true).GetValue(component);
            string? value3 = null;
            string? text = null;
            if (null != obj2)
            {
                value3 = obj2.ToString();
            }
            if (null != value2)
            {
                text = value2.ToString();
            }
            if (text == "1" && string.IsNullOrEmpty(value3))
            {
                return false;
            }
            return true;
        }
    }

}
