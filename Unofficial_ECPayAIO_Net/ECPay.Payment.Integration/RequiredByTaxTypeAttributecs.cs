using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECPay.Payment.Integration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class RequiredByTaxTypeAttribute : RequiredAttribute
    {
        public override bool IsValid(object? value)
        {
            object[]? array = value as object[];
            _ = array?[0];
            object? obj2 = array?[1];
            object? component = array?[2];
            _ = array?[3];
            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(component);
            object value2 = propertyDescriptorCollection.Find("TaxType", ignoreCase: true).GetValue(component);
            if (value2.Equals(TaxationType.ZeroTaxRate) && obj2 != null)
            {
                return !obj2.Equals(CustomsClearance.None);
            }
            return true;
        }
    }
}
