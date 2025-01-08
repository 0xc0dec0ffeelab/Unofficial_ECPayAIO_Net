using System;

namespace ECPay.Payment.Integration.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TextAttribute : Attribute
    {
        public string? Name { get; set; }
    }
}
