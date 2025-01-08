using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECPay.Payment.Integration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class RequiredByPrintAttribute : RequiredAttribute
    {
        public override bool IsValid(object? value)
        {
            object[]? array = value as object[];
            _ = array?[0];
            object? value2 = array?[1];
            object? component = array?[2];
            _ = array?[3];
            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(component);
            var value3 = propertyDescriptorCollection.Find("Print", ignoreCase: true).GetValue(component);
            if (value3.Equals(PrintFlag.Yes))
            {
                return IsValid(value2);
            }
            return true;
        }
    }
}
