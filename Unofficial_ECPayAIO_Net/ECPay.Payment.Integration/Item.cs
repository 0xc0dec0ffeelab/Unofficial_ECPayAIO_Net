namespace ECPay.Payment.Integration
{
    public class Item
    {
        public string? Name { get; set; }

        public decimal Price { get; set; }

        public string? Currency { get; set; }

        public int Quantity { get; set; }

        [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
        public string? Unit { get; set; }

        [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
        public TaxationType TaxType { get; set; }

        public string? URL { get; set; }

        public Item()
        {
            TaxType = TaxationType.None;
        }
    }
}
