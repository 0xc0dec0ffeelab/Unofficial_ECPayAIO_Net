using System;

namespace ECPay.Payment.Integration
{
    public class ItemCollectionEventArgs : EventArgs
    {
        public string MethodName { get; set; }

        public ItemCollectionEventArgs(object _, string methodName)
        {
            MethodName = methodName;
        }
    }
}
