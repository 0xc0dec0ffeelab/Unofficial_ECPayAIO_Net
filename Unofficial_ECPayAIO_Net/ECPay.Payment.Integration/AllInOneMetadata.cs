using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace ECPay.Payment.Integration
{
    public class AllInOneMetadata : CommonMetadata
    {
        public class ChargeBackArguments : INotifyPropertyChanged
        {
            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string? MerchantTradeNo { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string? TradeNo { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            public decimal ChargeBackTotalAmount { get; set; }

            [StringLength(100, ErrorMessage = "{0} max langth as {1}.")]
            public string? Remark { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;
        }

        public class SendArguments : BaseSendArguments, INotifyPropertyChanged
        {
            private PaymentMethod _ChoosePayment;

            [Required(ErrorMessage = "{0} is required.")]
            public PaymentMethod ChoosePayment
            {
                get
                {
                    return _ChoosePayment;
                }
                set
                {
                    _ChoosePayment = value;
                    RaisePropertyEvents((SendArguments p) => p.ChoosePayment);
                }
            }

            [Required(ErrorMessage = "{0} is required.")]
            public PaymentMethodItem ChooseSubPayment { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            public ExtraPaymentInfo NeedExtraPaidInfo { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            public DeviceType DeviceSource { get; set; }

            [EqualsByPaymentMethod(ErrorMessage = "The {0} string must be same as the PaymentMethod, and not allow as \"ALL\".")]
            [StringLength(100, ErrorMessage = "{0} max langth as {1}.")]
            public string? IgnorePayment { get; set; }

            [StringLength(10, ErrorMessage = "{0} max langth as {1}.")]
            public string? PlatformID { get; set; }

            [ValidatePlatformArguments(ErrorMessage = "If the PlatformID is empty, then the {0} must be empty.")]
            public int? PlatformChargeFee { get; set; }

            public HoldTradeType HoldTradeAMT { get; set; }

            [StringLength(10, ErrorMessage = "{0} max langth as {1}.")]
            [ValidatePlatformArguments(ErrorMessage = "If the PlatformID is empty, then the {0} must be empty.")]
            public string? AllPayID { get; set; }

            [ValidatePlatformArguments(ErrorMessage = "If the PlatformID is empty, then the {0} must be empty.")]
            [StringLength(50, ErrorMessage = "{0} max langth as {1}.")]
            public string? AccountID { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            public InvoiceState InvoiceMark { get; set; }

            [RegularExpression("^(?:http|https|ftp)://[a-zA-Z0-9\\.\\-]+(?:\\:\\d{1,5})?(?:[A-Za-z0-9\\.\\;\\:\\@\\&\\=\\+\\-\\$\\,\\?/_]|%u[0-9A-Fa-f]{4}|%[0-9A-Fa-f]{2})*$", ErrorMessage = "{0} is not correct URL.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string? OrderResultURL { get; set; }

            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string? StoreID { get; set; }

            [StringLength(50, ErrorMessage = "{0} max langth as {1}.")]
            public string? CustomField1 { get; set; }

            [StringLength(50, ErrorMessage = "{0} max langth as {1}.")]
            public string? CustomField2 { get; set; }

            [StringLength(50, ErrorMessage = "{0} max langth as {1}.")]
            public string? CustomField3 { get; set; }

            [StringLength(50, ErrorMessage = "{0} max langth as {1}.")]
            public string? CustomField4 { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            public int EncryptType { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;

            public SendArguments()
            {
                PaymentType = "aio";
                ChoosePayment = PaymentMethod.ALL;
                ChooseSubPayment = PaymentMethodItem.None;
                NeedExtraPaidInfo = ExtraPaymentInfo.No;
                HoldTradeAMT = HoldTradeType.No;
                InvoiceMark = InvoiceState.No;
                DeviceSource = DeviceType.PC;
                Items.CollectionChanged += Items_CollectionChanged;
            }

            private void Items_CollectionChanged(object sender, ItemCollectionEventArgs e)
            {
                if (!string.IsNullOrEmpty(e.MethodName))
                {
                    RaisePropertyEvents((SendArguments p) => p.Items);
                }
            }

            protected virtual void RaisePropertyEvents<T>(Expression<Func<SendArguments, T>> property)
            {
                if (!(property.Body is MemberExpression memberExpression) || memberExpression.Expression != property.Parameters[0] || memberExpression.Member.MemberType != MemberTypes.Property)
                {
                    throw new InvalidOperationException("Now tell me about the property");
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
            }
        }

        public class SendExtendArguments
        {
            public PaymentMethod _PaymentMethod { get; internal set; }

            [RequiredByPaymentMethod(PaymentMethod.ATM, ErrorMessage = "{0} is required.")]
            public int ExpireDate { get; set; }

            public int? StoreExpireDate { get; set; }

            public string? Desc_1 { get; set; }

            public string? Desc_2 { get; set; }

            public string? Desc_3 { get; set; }

            public string? Desc_4 { get; set; }

            [RegularExpression("^(?:http|https|ftp)://[a-zA-Z0-9\\.\\-]+(?:\\:\\d{1,5})?(?:[A-Za-z0-9\\.\\;\\:\\@\\&\\=\\+\\-\\$\\,\\?/_]|%u[0-9A-Fa-f]{4}|%[0-9A-Fa-f]{2})*$", ErrorMessage = "{0} is not correct URL.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string? ClientRedirectURL { get; set; }

            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            [RequiredByPaymentMethod(PaymentMethod.Alipay, ErrorMessage = "{0} is required.")]
            public string? AlipayItemName { get; set; }

            [StringLength(100, ErrorMessage = "{0} max langth as {1}.")]
            [RequiredByPaymentMethod(PaymentMethod.Alipay, ErrorMessage = "{0} is required.")]
            public string? AlipayItemCounts { get; set; }

            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            [RequiredByPaymentMethod(PaymentMethod.Alipay, ErrorMessage = "{0} is required.")]
            public string? AlipayItemPrice { get; set; }

            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            [RequiredByPaymentMethod(PaymentMethod.Alipay, ErrorMessage = "{0} is required.")]
            public string? Email { get; set; }

            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            [RequiredByPaymentMethod(PaymentMethod.Alipay, ErrorMessage = "{0} is required.")]
            public string? PhoneNo { get; set; }

            [RequiredByPaymentMethod(PaymentMethod.Alipay, ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string? UserName { get; set; }

            [RequiredByPaymentMethod(PaymentMethod.Tenpay, ErrorMessage = "{0} is required.")]
            public DateTime ExpireTime { get; set; }

            [CompareByPaymentMethod(PaymentMethod.Credit, "PeriodAmount/PeriodType/Frequency/ExecTimes", ErrorMessage = "When payment method is Credit and you want to installment, then {0} is required and {4} is not allow required.")]
            public string? CreditInstallment { get; set; }

            [CompareByPaymentMethod(PaymentMethod.Credit, "PeriodAmount/PeriodType/Frequency/ExecTimes", ErrorMessage = "When payment method is Credit and you want to installment, then {0} is required and {4} is not allow required.")]
            public decimal? InstallmentAmount { get; set; }

            [CompareByPaymentMethod(PaymentMethod.Credit, "PeriodAmount/PeriodType/Frequency/ExecTimes", ErrorMessage = "When payment method is Credit and you want to installment, then {0} is required and {4} is not allow required.")]
            public bool? Redeem { get; set; }

            [CompareByPaymentMethod(PaymentMethod.Credit, "PeriodAmount/PeriodType/Frequency/ExecTimes", ErrorMessage = "When payment method is Credit and you want to installment, then {0} is required and {4} is not allow required.")]
            public bool? UnionPay { get; set; }

            public string? Language { get; set; }

            [CompareByPaymentMethod(PaymentMethod.Credit, "CreditInstallment/InstallmentAmount/Redeem/UnionPay", ErrorMessage = "When payment method is Credit and you want to Systematic Investment Plan(SIP), then {0} is required and {4} is not allow required.")]
            public int? PeriodAmount { get; set; }

            [CompareByPaymentMethod(PaymentMethod.Credit, "CreditInstallment/InstallmentAmount/Redeem/UnionPay", ErrorMessage = "When payment method is Credit and you want to Systematic Investment Plan(SIP), then {0} is required and {4} is not allow required.")]
            public PeriodType PeriodType { get; set; }

            [CompareByPaymentMethod(PaymentMethod.Credit, "CreditInstallment/InstallmentAmount/Redeem/UnionPay", ErrorMessage = "When payment method is Credit and you want to Systematic Investment Plan(SIP), then {0} is required and {4} is not allow required.")]
            public int? Frequency { get; set; }

            [CompareByPaymentMethod(PaymentMethod.Credit, "CreditInstallment/InstallmentAmount/Redeem/UnionPay", ErrorMessage = "When payment method is Credit and you want to Systematic Investment Plan(SIP), then {0} is required and {4} is not allow required.")]
            public int? ExecTimes { get; set; }

            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(30, ErrorMessage = "{0} max langth as {1}.")]
            public string? RelateNumber { get; set; }

            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            [RequiredByCarruerType(ErrorMessage = "When CarruerType equal \"Member\", then {0} is required.")]
            public string? CustomerID { get; set; }

            [StringLength(8, ErrorMessage = "{0} max langth as {1}.")]
            public string? CustomerIdentifier { get; set; }

            [RequiredByPrint(ErrorMessage = "When Print equal \"Yes\", then {0} is required.")]
            [StringLength(60, ErrorMessage = "{0} max langth as {1}.")]
            public string? CustomerName { get; set; }

            [RequiredByPrint(ErrorMessage = "When Print equal \"Yes\", then {0} is required.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string? CustomerAddr { get; set; }

            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string? CustomerPhone { get; set; }

            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string? CustomerEmail { get; set; }

            [RequiredByTaxType(ErrorMessage = "{0} is not allow none.")]
            public CustomsClearance ClearanceMark { get; set; }

            [RequiredByInvoiceMark(ErrorMessage = "{0} is not allow none.")]
            public TaxationType TaxType { get; set; }

            public InvoiceVehicleType CarruerType { get; set; }

            [StringLength(64, ErrorMessage = "{0} max langth as {1}.")]
            [RequiredByCarruerType(ErrorMessage = "When CarruerType equal \"NaturalPersonEvidence\" or \"PhoneBarcode\", then {0} is required.")]
            public string? CarruerNum { get; set; }

            [RequiredByInvoiceMark(ErrorMessage = "{0} is not allow none.")]
            public DonatedInvoice Donation { get; set; }

            [RequiredByDonation(ErrorMessage = "{0} is required.")]
            [RegularExpression("^([Xx0-9])[0-9]{2,6}$", ErrorMessage = "{0} format error.")]
            [StringLength(7, ErrorMessage = "{0} max langth as {1}.")]
            public string? LoveCode { get; set; }

            [RequiredByInvoiceMark(ErrorMessage = "{0} is not allow none.")]
            public PrintFlag Print { get; set; }

            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(4000, ErrorMessage = "{0} max langth as {1}.")]
            public string? InvoiceItemName { get; set; }

            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(4000, ErrorMessage = "{0} max langth as {1}.")]
            public string? InvoiceItemCount { get; set; }

            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(4000, ErrorMessage = "{0} max langth as {1}.")]
            public string? InvoiceItemWord { get; set; }

            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(4000, ErrorMessage = "{0} max langth as {1}.")]
            public string? InvoiceItemPrice { get; set; }

            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(4000, ErrorMessage = "{0} max langth as {1}.")]
            public string? InvoiceItemTaxType { get; set; }

            [StringLength(4000, ErrorMessage = "{0} max langth as {1}.")]
            public string? InvoiceRemark { get; set; }

            [Range(0, 15, ErrorMessage = "{0} range langth as between {1} to {2}.")]
            public int DelayDay { get; set; }

            [RequiredByInvoiceMark(ErrorMessage = "{0} is not allow none.")]
            public TheWordType InvType { get; set; }

            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string? PaymentInfoURL { get; set; }

            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string? PeriodReturnURL { get; set; }

            public BindingCardType BindingCard { get; set; }

            [ValidateMerchantMemberID(ErrorMessage = "If the BindingCard is Yes, then the {0} must be required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string? MerchantMemberID { get; set; }

            public SendExtendArguments()
            {
                ExpireDate = 3;
                ExpireTime = DateTime.Now.AddHours(72.0);
                PeriodType = PeriodType.None;
                _PaymentMethod = PaymentMethod.ALL;
                ClearanceMark = CustomsClearance.None;
                TaxType = TaxationType.None;
                CarruerType = InvoiceVehicleType.None;
                Donation = DonatedInvoice.None;
                Print = PrintFlag.None;
                DelayDay = 0;
                InvType = TheWordType.None;
            }
        }

        public class ActionArguments : INotifyPropertyChanged
        {
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            [Required(ErrorMessage = "{0} is required.")]
            public string? MerchantTradeNo { get; set; }

            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            [Required(ErrorMessage = "{0} is required.")]
            public string? TradeNo { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            public ActionType Action { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            public decimal TotalAmount { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;
        }

        public class QueryArguments : BaseQueryArguments
        {
        }

        public class TradeFileArguments : INotifyPropertyChanged
        {
            [Required(ErrorMessage = "{0} is required.")]
            public TradeDateType DateType { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            [RegularExpression("^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$", ErrorMessage = "{0} format is yyyy-MM-dd.")]
            public string? BeginDate { get; set; }

            [RegularExpression("^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$", ErrorMessage = "{0} format is yyyy-MM-dd.")]
            [Required(ErrorMessage = "{0} is required.")]
            public string? EndDate { get; set; }

            [Required(ErrorMessage = "{0} is required.")]
            public PaymentMethod PaymentType { get; set; }

            public PlatformState PlatformStatus { get; set; }

            public PaymentState PaymentStatus { get; set; }

            public AllocateState AllocateStatus { get; set; }

            public bool NewFormatedMedia { get; set; }

            public CharSetState CharSet { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;

            public TradeFileArguments()
            {
                DateType = TradeDateType.Payment;
                PaymentType = PaymentMethod.ALL;
                PlatformStatus = PlatformState.ALL;
                PaymentStatus = PaymentState.ALL;
                AllocateStatus = AllocateState.ALL;
                NewFormatedMedia = true;
                CharSet = CharSetState.Default;
            }
        }

        public SendArguments Send { get; private set; }

        public SendExtendArguments SendExtend { get; private set; }

        public QueryArguments Query { get; private set; }

        public ActionArguments Action { get; private set; }

        public ChargeBackArguments ChargeBack { get; private set; }

        public TradeFileArguments TradeFile { get; private set; }

        public AllInOneMetadata()
        {
            ServiceMethod = HttpMethod.HttpPOST;
            Send = new SendArguments();
            Send.PropertyChanged += Send_PropertyChanged;
            SendExtend = new SendExtendArguments();
            Query = new QueryArguments();
            Action = new ActionArguments();
            ChargeBack = new ChargeBackArguments();
            TradeFile = new TradeFileArguments();
        }

        private void Send_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ChoosePayment")
            {
                SendExtend._PaymentMethod = Send.ChoosePayment;
            }
            if (!(e.PropertyName == "Items") || Send.Items.Count <= 0)
            {
                return;
            }
            string text = string.Empty;
            string text2 = string.Empty;
            string text3 = string.Empty;
            string text4 = string.Empty;
            string text5 = string.Empty;
            string text6 = string.Empty;
            string text7 = string.Empty;
            string text8 = string.Empty;
            string text9 = string.Empty;
            string text10 = string.Empty;
            foreach (Item item in Send.Items)
            {
                text += $"{item.Name} {item.Price} {item.Currency} x {item.Quantity}#";
                if (string.IsNullOrEmpty(text2))
                {
                    text2 = item.URL ?? string.Empty;
                }
                text3 += $"{item.Name}#";
                text4 += $"{item.Quantity}#";
                text5 += $"{item.Price}#";
                text6 += $"{item.Name}|";
                text7 += $"{item.Quantity}|";
                text8 += $"{item.Unit}|";
                text9 += $"{item.Price}|";
                text10 += $"{((item.TaxType == TaxationType.None) ? string.Empty : ((int)item.TaxType).ToString())}|";
            }
            text = text.Substring(0, text.Length - 1);
            text = text.Substring(0, (text.Length > 200) ? 200 : text.Length);
            text3 = text3.Substring(0, text3.Length - 1);
            text3 = text3.Substring(0, (text3.Length > 200) ? 200 : text3.Length);
            text4 = text4.Substring(0, text4.Length - 1);
            text4 = text4.Substring(0, (text4.Length > 100) ? 100 : text4.Length);
            text5 = text5.Substring(0, text5.Length - 1);
            text5 = text5.Substring(0, (text5.Length > 20) ? 20 : text5.Length);
            text6 = text6.Substring(0, text6.Length - 1);
            text7 = text7.Substring(0, text7.Length - 1);
            text8 = ((text8.Length == Send.Items.Count) ? string.Empty : text8.Substring(0, text8.Length - 1));
            text9 = text9.Substring(0, text9.Length - 1);
            text10 = ((text10.Length == Send.Items.Count) ? string.Empty : text10.Substring(0, text10.Length - 1));
            Send._ItemName = text;
            Send._ItemURL = text2;
            SendExtend.AlipayItemName = text3;
            SendExtend.AlipayItemCounts = text4;
            SendExtend.AlipayItemPrice = text5;
            SendExtend.InvoiceItemName = text6;
            SendExtend.InvoiceItemCount = text7;
            SendExtend.InvoiceItemWord = text8;
            SendExtend.InvoiceItemPrice = text9;
            SendExtend.InvoiceItemTaxType = text10;
        }
    }
}
