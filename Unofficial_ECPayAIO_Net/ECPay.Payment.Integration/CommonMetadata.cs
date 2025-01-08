using System;
using System.ComponentModel.DataAnnotations;

namespace ECPay.Payment.Integration
{
    public abstract class CommonMetadata
    {
        public abstract class BaseQueryArguments
        {
            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string? MerchantTradeNo { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            public int TimeStamp { get; private set; }

            public BaseQueryArguments()
            {
                TimeStamp = Convert.ToInt32((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000L) / 10000000);
            }
        }

        public abstract class BaseSendArguments
        {
            internal string _ItemName = string.Empty;

            internal string _ItemURL = string.Empty;

            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string? MerchantTradeNo { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            public string? MerchantTradeDate { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string? PaymentType { get; protected set; }

            [Required(ErrorMessage = "{0} is required.")]
            public decimal TotalAmount { get; set; }

            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            [Required(ErrorMessage = "{0} is required.")]
            public string? TradeDesc { get; set; }

            public string? ItemName => _ItemName;

            [Required(ErrorMessage = "{0} is required.")]
            public ItemCollection Items { get; set; } = new ItemCollection();

            [RegularExpression("^(?:http|https|ftp)://[a-zA-Z0-9\\.\\-]+(?:\\:\\d{1,5})?(?:[A-Za-z0-9\\.\\;\\:\\@\\&\\=\\+\\-\\$\\,\\?/_]|%u[0-9A-Fa-f]{4}|%[0-9A-Fa-f]{2})*$", ErrorMessage = "{0} is not correct URL.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string? ItemURL { get; set; }

            [StringLength(100, ErrorMessage = "{0} max langth as {1}.")]
            public string? Remark { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            [RegularExpression("^(?:http|https|ftp)://[a-zA-Z0-9\\.\\-]+(?:\\:\\d{1,5})?(?:[A-Za-z0-9\\.\\;\\:\\@\\&\\=\\+\\-\\$\\,\\?/_]|%u[0-9A-Fa-f]{4}|%[0-9A-Fa-f]{2})*$", ErrorMessage = "{0} is not correct URL.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string? ReturnURL { get; set; }

            [RegularExpression("^(?:http|https|ftp)://[a-zA-Z0-9\\.\\-]+(?:\\:\\d{1,5})?(?:[A-Za-z0-9\\.\\;\\:\\@\\&\\=\\+\\-\\$\\,\\?/_]|%u[0-9A-Fa-f]{4}|%[0-9A-Fa-f]{2})*$", ErrorMessage = "{0} is not correct URL.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string? ClientBackURL { get; set; }

            public BaseSendArguments()
            {
                Items = new ItemCollection();
            }
        }

        [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
        [Required(ErrorMessage = "{0} is required.")]
        [RegularExpression("^(?:http|https|ftp)://[a-zA-Z0-9\\.\\-]+(?:\\:\\d{1,5})?(?:[A-Za-z0-9\\.\\;\\:\\@\\&\\=\\+\\-\\$\\,\\?/_]|%u[0-9A-Fa-f]{4}|%[0-9A-Fa-f]{2})*$", ErrorMessage = "{0} is not correct URL.")]
        public string? ServiceURL { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public HttpMethod ServiceMethod { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string? HashKey { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string? HashIV { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(10, ErrorMessage = "{0} max langth as {1}.")]
        public string? MerchantID { get; set; }

        public CommonMetadata()
        {
        }
    }
}
