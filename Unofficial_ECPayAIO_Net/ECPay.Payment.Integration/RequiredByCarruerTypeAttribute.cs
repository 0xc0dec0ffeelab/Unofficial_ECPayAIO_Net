using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECPay.Payment.Integration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class RequiredByCarruerTypeAttribute : RequiredAttribute
    {
        public override bool IsValid(object? value)
        {
            object[]? array = value as object[];
            object? obj = array?[0];
            object? obj2 = array?[1];
            object? component = array?[2];
            object? obj3 = array?[3];
            if (null != obj3)
            {
                PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(obj3);
                var value2 = propertyDescriptorCollection.Find("InvoiceMark", ignoreCase: true).GetValue(obj3);
                if (value2.Equals(InvoiceState.Yes))
                {
                    bool result = obj2 != null;
                    if (obj?.Equals("CustomerID") ?? false)
                    {
                        propertyDescriptorCollection = TypeDescriptor.GetProperties(component);
                        var obj4 = propertyDescriptorCollection.Find("CarruerType", ignoreCase: true).GetValue(component);
                        if (obj4.Equals(InvoiceVehicleType.Member))
                        {
                            return IsValid(obj2);
                        }
                    }
                    else if (obj?.Equals("CarruerNum") ?? false)
                    {
                        propertyDescriptorCollection = TypeDescriptor.GetProperties(component);
                        var obj4 = propertyDescriptorCollection.Find("CarruerType", ignoreCase: true).GetValue(component);
                        if (obj4.Equals(InvoiceVehicleType.NaturalPersonEvidence) || obj4.Equals(InvoiceVehicleType.PhoneBarcode))
                        {
                            return IsValid(obj2);
                        }
                    }
                    return result;
                }
            }
            return true;
        }
    }
}
