using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECPay.Payment.Integration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class ValidatePlatformArgumentsAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            object[]? array = value as object[];
            _ = array?[0];
            object? obj2 = array?[1];
            object? component = array?[2];
            _ = array?[3];
            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(component);
            object value2 = propertyDescriptorCollection.Find("PlatformID", ignoreCase: true).GetValue(component);
            string? value3 = null;
            string? value4 = null;
            if (null != obj2)
            {
                value3 = obj2.ToString();
            }
            if (null != value2)
            {
                value4 = value2.ToString();
            }
            if (!string.IsNullOrEmpty(value3) && string.IsNullOrEmpty(value4))
            {
                return false;
            }
            return true;
        }
    }
}
