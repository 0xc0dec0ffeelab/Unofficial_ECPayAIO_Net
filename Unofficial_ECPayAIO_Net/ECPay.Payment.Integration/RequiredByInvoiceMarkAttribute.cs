using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;

namespace ECPay.Payment.Integration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class RequiredByInvoiceMarkAttribute : RequiredAttribute
    {
        private readonly string[] szaPhoneOrEmail = new string[2] { "CustomerPhone", "CustomerEmail" };

        private readonly string[] szaAllowEmpty = new string[4] { "CustomerID", "CustomerIdentifier", "CustomerName", "CustomerAddr" };

        public override bool IsValid(object? value)
        {
            object[]? array = (object[]?)value;
            object? obj = array?[0];
            object? obj2 = array?[1];
            object? component = array?[2];
            object? obj3 = array?[3];
            if (null != obj3)
            {
                PropertyDescriptorCollection? propertyDescriptorCollection = TypeDescriptor.GetProperties(obj3);
                object value2 = propertyDescriptorCollection.Find("InvoiceMark", ignoreCase: true).GetValue(obj3);
                if (value2.Equals(InvoiceState.Yes))
                {
                    bool flag = IsValid(obj2);
                    if (!flag && szaPhoneOrEmail.Contains(obj))
                    {
                        object? value3 = null;
                        propertyDescriptorCollection = TypeDescriptor.GetProperties(component);
                        if (obj?.Equals("CustomerPhone") ?? false)
                        {
                            value3 = propertyDescriptorCollection.Find("CustomerEmail", ignoreCase: true).GetValue(component);
                        }
                        if (obj?.Equals("CustomerEmail") ?? false)
                        {
                            value3 = propertyDescriptorCollection.Find("CustomerPhone", ignoreCase: true).GetValue(component);
                        }
                        return IsValid(value3);
                    }
                    if (obj?.Equals("TaxType") ?? false)
                    {
                        return !obj2!.Equals(TaxationType.None);
                    }
                    if (obj?.Equals("Donation") ?? false)
                    {
                        return !obj2!.Equals(DonatedInvoice.None);
                    }
                    if (obj?.Equals("Print") ?? false)
                    {
                        return !obj2!.Equals(PrintFlag.None);
                    }
                    if (obj?.Equals("InvType") ?? false)
                    {
                        return !obj2!.Equals(TheWordType.None);
                    }
                    if (szaAllowEmpty.Contains(obj))
                    {
                        return obj2.Equals(string.Empty);
                    }
                    return flag;
                }
            }
            return true;
        }
    }
}
