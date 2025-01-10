using System.Linq;

namespace ECPay.Payment.Integration
{
    public class PeriodCreditCardTradeInfo
    {
        public string? MerchantID { get; set; }

        public string? MerchantTradeNo { get; set; }

        public string? TradeNo { get; set; }

        public int RtnCode { get; set; }

        public string? PeriodType { get; set; }

        public int Frequency { get; set; }

        public int ExecTimes { get; set; }

        public int PeriodAmount { get; set; }

        public int amount { get; set; }

        public long gwsr { get; set; }

        public string? process_date { get; set; }

        public string? auth_code { get; set; }

        public string? card4no { get; set; }

        public string? card6no { get; set; }

        public int TotalSuccessTimes { get; set; }

        public int TotalSuccessAmount { get; set; }

        public PeriodCreditCardTradeInfoLog[] ExecLog { get; set; } = Enumerable.Empty<PeriodCreditCardTradeInfoLog>().ToArray();

        public string? ExecStatus { get; set; }
    }
}
