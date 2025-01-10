using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;

namespace ECPay.Payment.Integration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class RequiredByInvoiceMarkAttribute : RequiredAttribute
    {
        private readonly string[] szaPhoneOrEmail = ["CustomerPhone", "CustomerEmail"];

        private readonly string[] szaAllowEmpty = ["CustomerID", "CustomerIdentifier", "CustomerName", "CustomerAddr"];

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
                object? value2 = propertyDescriptorCollection.Find("InvoiceMark", ignoreCase: true)?.GetValue(obj3);
                if (InvoiceState.Yes.Equals(value2))
                {
                    bool flag = base.IsValid(obj2);
                    if (!flag && szaPhoneOrEmail.Contains(obj))
                    {
                        object? value3 = null;
                        if (component == null) return false;
                        propertyDescriptorCollection = TypeDescriptor.GetProperties(component);
                        if ("CustomerPhone".Equals(obj))
                        {
                            value3 = propertyDescriptorCollection.Find("CustomerEmail", ignoreCase: true)?.GetValue(component);
                        }
                        if ("CustomerEmail".Equals(obj))
                        {
                            value3 = propertyDescriptorCollection.Find("CustomerPhone", ignoreCase: true)?.GetValue(component);
                        }
                        return base.IsValid(value3);
                    }
                    if ("TaxType".Equals(obj))
                    {
                        return !TaxationType.None.Equals(obj2);
                    }
                    if ("Donation".Equals(obj))
                    {
                        return !DonatedInvoice.None.Equals(obj2);
                    }
                    if ("Print".Equals(obj))
                    {
                        return !PrintFlag.None.Equals(obj2);
                    }
                    if ("InvType".Equals(obj))
                    {
                        return !TheWordType.None.Equals(obj2);
                    }
                    if (szaAllowEmpty.Contains(obj))
                    {
                        return string.Empty.Equals(obj2);
                    }
                    return flag;
                }
            }
            return true;
        }
    }
}
