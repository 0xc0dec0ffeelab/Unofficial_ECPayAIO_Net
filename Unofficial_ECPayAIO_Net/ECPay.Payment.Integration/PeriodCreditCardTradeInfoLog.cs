namespace ECPay.Payment.Integration
{
    public class PeriodCreditCardTradeInfoLog
    {
        public int RtnCode { get; set; }

        public int amount { get; set; }

        public long gwsr { get; set; }

        public string? process_date { get; set; }

        public string? auth_code { get; set; }
    }
}
