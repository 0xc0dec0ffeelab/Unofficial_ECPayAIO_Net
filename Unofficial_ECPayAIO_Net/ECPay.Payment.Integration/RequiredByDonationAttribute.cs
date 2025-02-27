﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECPay.Payment.Integration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class RequiredByDonationAttribute : RequiredAttribute
    {
        public override bool IsValid(object? value)
        {
            object[]? array = value as object[];
            _ = array?[0];
            object? value2 = array?[1];
            object? component = array?[2];
            _ = array?[3];
            if (component == null) return false;
            PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(component);
            var value3 = propertyDescriptorCollection.Find("Donation", ignoreCase: true)?.GetValue(component);
            if (DonatedInvoice.Yes.Equals(value3))
            {
                return base.IsValid(value2);
            }
            return true;
        }
    }
}
