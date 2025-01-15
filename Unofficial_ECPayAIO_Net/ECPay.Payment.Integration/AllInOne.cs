using ECPay.Payment.Integration.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECPay.Payment.Integration
{

    public class AllInOne : AllInOneMetadata
    {
        private readonly HttpClient _httpClient;

        private readonly string _checkoutFormId = "checkoutForm";

        public AllInOne(HttpClient httpclient, string? checkoutFormId = default)
        {
            _httpClient = httpclient;

            if(!string.IsNullOrWhiteSpace(checkoutFormId))
            {
                _checkoutFormId = checkoutFormId;
            }      
        }

        public (string htmlPostForm, IEnumerable<string> errors) CheckOut()
        {
            List<string> errors =
            [
                .. ServerValidator.Validate(this),
                .. ServerValidator.Validate(Send),
                .. ServerValidator.Validate(Send, SendExtend),
            ];
            var htmlPostForm = string.Empty;
            if (errors.Count == 0)
            {
                if (Send.ChoosePayment == PaymentMethod.Credit && Send.DeviceSource == DeviceType.Mobile && !SendExtend.PeriodAmount.HasValue)
                {
                    bool flag = (!string.IsNullOrEmpty(Send.IgnorePayment) && Send.IgnorePayment.ToUpper() == "GOOGLEPAY");
                    Send.ChoosePayment = PaymentMethod.ALL;
                    Send.IgnorePayment = string.Empty;
                    foreach (PaymentMethod value in Enum.GetValues(typeof(PaymentMethod)))
                    {
                        if (value != 0 && value != PaymentMethod.Credit && value != PaymentMethod.GooglePay)
                        {
                            Send.IgnorePayment += $"{value}#";
                        }
                    }
                    Send.IgnorePayment += "APPBARCODE#";
                    if (flag)
                    {
                        Send.IgnorePayment += "GooglePay";
                    }
                    if (Send.IgnorePayment.EndsWith("#"))
                    {
                        Send.IgnorePayment = Send.IgnorePayment.TrimEnd('#');
                    }
                }
                Hashtable hashtable = new Hashtable();
                string text = Send.ChoosePayment.ToString();
                hashtable.Add("MerchantID", MerchantID);
                hashtable.Add("MerchantTradeNo", Send.MerchantTradeNo);
                hashtable.Add("MerchantTradeDate", Send.MerchantTradeDate);
                hashtable.Add("PaymentType", Send.PaymentType);
                hashtable.Add("TotalAmount", Send.TotalAmount);
                hashtable.Add("TradeDesc", Send.TradeDesc);
                hashtable.Add("ItemName", Send.ItemName);
                hashtable.Add("ReturnURL", Send.ReturnURL);
                hashtable.Add("ChoosePayment", text);
                hashtable.Add("ClientBackURL", Send.ClientBackURL);
                hashtable.Add("ItemURL", Send.ItemURL);
                hashtable.Add("Remark", Send.Remark);
                if (Send.EncryptType == 1)
                {
                    hashtable.Add("EncryptType", Send.EncryptType);
                }
                if (!string.IsNullOrEmpty(Send.StoreID))
                {
                    hashtable.Add("StoreID", Send.StoreID);
                }
                if (!string.IsNullOrEmpty(Send.CustomField1))
                {
                    hashtable.Add("CustomField1", Send.CustomField1);
                }
                if (!string.IsNullOrEmpty(Send.CustomField2))
                {
                    hashtable.Add("CustomField2", Send.CustomField2);
                }
                if (!string.IsNullOrEmpty(Send.CustomField3))
                {
                    hashtable.Add("CustomField3", Send.CustomField3);
                }
                if (!string.IsNullOrEmpty(Send.CustomField4))
                {
                    hashtable.Add("CustomField4", Send.CustomField4);
                }
                if (Send.ChooseSubPayment != 0)
                {
                    string[] array = Send.ChooseSubPayment.ToString().Split('_');
                    switch (array.Length)
                    {
                        case 1:
                            hashtable.Add("ChooseSubPayment", array[0]);
                            break;
                        case 2:
                            hashtable.Add("ChooseSubPayment", array[1]);
                            break;
                    }
                }
                hashtable.Add("OrderResultURL", Send.OrderResultURL);
                hashtable.Add("NeedExtraPaidInfo", Send.NeedExtraPaidInfo.ToString()[..1]);
                if (Send.DeviceSource == DeviceType.None)
                {
                    hashtable.Add("DeviceSource", "");
                }
                else if (Send.DeviceSource == DeviceType.PC || Send.DeviceSource == DeviceType.Mobile)
                {
                    hashtable.Add("DeviceSource", Send.DeviceSource.ToString()[..1]);
                }
                if (!string.IsNullOrEmpty(Send.IgnorePayment))
                {
                    hashtable.Add("IgnorePayment", Send.IgnorePayment);
                }
                if (!string.IsNullOrEmpty(Send.PlatformID))
                {
                    hashtable.Add("PlatformID", Send.PlatformID);
                }
                if (Send.InvoiceMark == InvoiceState.Yes)
                {
                    hashtable.Add("InvoiceMark", "Y");
                }
                if (Send.PlatformChargeFee.HasValue)
                {
                    hashtable.Add("PlatformChargeFee", Send.PlatformChargeFee);
                }
                if (Send.HoldTradeAMT == HoldTradeType.Yes)
                {
                    hashtable.Add("HoldTradeAMT", (int)Send.HoldTradeAMT);
                }
                if (!string.IsNullOrEmpty(Send.AllPayID))
                {
                    hashtable.Add("AllPayID", Send.AllPayID);
                }
                if (!string.IsNullOrEmpty(Send.AccountID))
                {
                    hashtable.Add("AccountID", Send.AccountID);
                }
                if (text == "ATM")
                {
                    hashtable.Add("ExpireDate", SendExtend.ExpireDate);
                    if (!string.IsNullOrEmpty(SendExtend.PaymentInfoURL))
                    {
                        hashtable.Add("PaymentInfoURL", SendExtend.PaymentInfoURL);
                    }
                    if (!string.IsNullOrEmpty(SendExtend.ClientRedirectURL))
                    {
                        hashtable.Add("ClientRedirectURL", SendExtend.ClientRedirectURL);
                    }
                }
                if (text == "CVS" || text == "BARCODE")
                {
                    if (SendExtend.StoreExpireDate.HasValue)
                    {
                        hashtable.Add("StoreExpireDate", SendExtend.StoreExpireDate.Value);
                    }
                    if (!string.IsNullOrEmpty(SendExtend.Desc_1))
                    {
                        hashtable.Add("Desc_1", SendExtend.Desc_1);
                    }
                    if (!string.IsNullOrEmpty(SendExtend.Desc_2))
                    {
                        hashtable.Add("Desc_2", SendExtend.Desc_2);
                    }
                    if (!string.IsNullOrEmpty(SendExtend.Desc_3))
                    {
                        hashtable.Add("Desc_3", SendExtend.Desc_3);
                    }
                    if (!string.IsNullOrEmpty(SendExtend.Desc_4))
                    {
                        hashtable.Add("Desc_4", SendExtend.Desc_4);
                    }
                    if (!string.IsNullOrEmpty(SendExtend.PaymentInfoURL))
                    {
                        hashtable.Add("PaymentInfoURL", SendExtend.PaymentInfoURL);
                    }
                    if (!string.IsNullOrEmpty(SendExtend.ClientRedirectURL))
                    {
                        hashtable.Add("ClientRedirectURL", SendExtend.ClientRedirectURL);
                    }
                }
                if (text == "Alipay")
                {
                    hashtable.Add("AlipayItemName", SendExtend.AlipayItemName);
                    hashtable.Add("AlipayItemCounts", SendExtend.AlipayItemCounts);
                    hashtable.Add("AlipayItemPrice", SendExtend.AlipayItemPrice);
                    hashtable.Add("Email", SendExtend.Email);
                    hashtable.Add("PhoneNo", SendExtend.PhoneNo);
                    hashtable.Add("UserName", SendExtend.UserName);
                }
                if (text == "Tenpay")
                {
                    hashtable.Add("ExpireTime", SendExtend.ExpireTime.ToString("yyyy/MM/dd HH:mm:ss"));
                }
                if (text == "Credit" || text == "GooglePay" || text == "AndroidPay")
                {
                    if (!string.IsNullOrEmpty(SendExtend.CreditInstallment))
                    {
                        hashtable.Add("CreditInstallment", SendExtend.CreditInstallment);
                    }
                    if (SendExtend.InstallmentAmount.HasValue)
                    {
                        hashtable.Add("InstallmentAmount", SendExtend.InstallmentAmount);
                    }
                    if (SendExtend.Redeem.HasValue && SendExtend.Redeem.Value)
                    {
                        hashtable.Add("Redeem", "Y");
                    }
                    if (SendExtend.UnionPay.HasValue)
                    {
                        hashtable.Add("UnionPay", SendExtend.UnionPay.Value ? 1 : 0);
                    }
                    if (SendExtend.PeriodAmount.HasValue)
                    {
                        hashtable.Add("PeriodAmount", SendExtend.PeriodAmount);
                    }
                    if (SendExtend.PeriodType != 0)
                    {
                        hashtable.Add("PeriodType", SendExtend.PeriodType.ToString()[..1]);
                    }
                    if (SendExtend.Frequency.HasValue)
                    {
                        hashtable.Add("Frequency", SendExtend.Frequency);
                    }
                    if (SendExtend.ExecTimes.HasValue)
                    {
                        hashtable.Add("ExecTimes", SendExtend.ExecTimes);
                    }
                    if (!string.IsNullOrEmpty(SendExtend.PeriodReturnURL))
                    {
                        hashtable.Add("PeriodReturnURL", SendExtend.PeriodReturnURL);
                    }
                    if (SendExtend.BindingCard == BindingCardType.Yes)
                    {
                        hashtable.Add("BindingCard", 1);
                    }
                    if (SendExtend.BindingCard == BindingCardType.Yes && !string.IsNullOrEmpty(SendExtend.MerchantMemberID))
                    {
                        hashtable.Add("MerchantMemberID", SendExtend.MerchantMemberID);
                    }
                }
                if (text == "BNPL")
                {
                    if (!string.IsNullOrEmpty(SendExtend.PaymentInfoURL))
                    {
                        hashtable.Add("PaymentInfoURL", SendExtend.PaymentInfoURL);
                    }
                    if (!string.IsNullOrEmpty(SendExtend.ClientRedirectURL))
                    {
                        hashtable.Add("ClientRedirectURL", SendExtend.ClientRedirectURL);
                    }
                }
                if (!string.IsNullOrEmpty(SendExtend.Language))
                {
                    hashtable.Add("Language", SendExtend.Language);
                }
                if (Send.InvoiceMark == InvoiceState.Yes)
                {
                    hashtable.Add("RelateNumber", SendExtend.RelateNumber);
                    hashtable.Add("CustomerID", SendExtend.CustomerID);
                    hashtable.Add("CustomerIdentifier", SendExtend.CustomerIdentifier);
                    hashtable.Add("CustomerName", WebUtility.UrlEncode(SendExtend.CustomerName));
                    hashtable.Add("CustomerAddr", WebUtility.UrlEncode(SendExtend.CustomerAddr));
                    hashtable.Add("CustomerPhone", SendExtend.CustomerPhone);
                    hashtable.Add("CustomerEmail", WebUtility.UrlEncode(SendExtend.CustomerEmail));
                    hashtable.Add("ClearanceMark", (SendExtend.ClearanceMark == CustomsClearance.None) ? string.Empty : ((int)SendExtend.ClearanceMark).ToString());
                    hashtable.Add("TaxType", (int)SendExtend.TaxType);
                    hashtable.Add("CarruerType", (SendExtend.CarruerType == InvoiceVehicleType.None) ? string.Empty : ((int)SendExtend.CarruerType).ToString());
                    hashtable.Add("CarruerNum", SendExtend.CarruerNum);
                    hashtable.Add("Donation", (int)SendExtend.Donation);
                    hashtable.Add("LoveCode", SendExtend.LoveCode);
                    hashtable.Add("Print", (int)SendExtend.Print);
                    hashtable.Add("InvoiceItemName", WebUtility.UrlEncode(SendExtend.InvoiceItemName));
                    hashtable.Add("InvoiceItemCount", SendExtend.InvoiceItemCount);
                    hashtable.Add("InvoiceItemWord", WebUtility.UrlEncode(SendExtend.InvoiceItemWord));
                    hashtable.Add("InvoiceItemPrice", SendExtend.InvoiceItemPrice);
                    hashtable.Add("InvoiceItemTaxType", SendExtend.InvoiceItemTaxType);
                    hashtable.Add("InvoiceRemark", string.IsNullOrEmpty(SendExtend.InvoiceRemark) ? string.Empty : WebUtility.UrlEncode(SendExtend.InvoiceRemark));
                    hashtable.Add("DelayDay", SendExtend.DelayDay);
                    hashtable.Add("InvType", $"{(int)SendExtend.InvType:00}");
                }

                IDictionary<string, string?> htmlDic = new Dictionary<string, string?>();
                string empty = RenderControlAndParamenter(hashtable, htmlDic);
                string empty2 = BuildCheckMacValue(empty, Send.EncryptType);
                empty += RenderControlAndParamenter("CheckMacValue", empty2, htmlDic);
                Logger.WriteLine(string.Format("INFO   {0}  OUTPUT  AllInOne.CheckOut: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), empty));
                if (ServiceMethod == HttpMethod.HttpPOST)
                {
                    htmlPostForm = GenerateHtmlPostForm(ServiceURL, _checkoutFormId, htmlDic);
                }
                else
                {
                    errors.Add("No service for HttpGET, ServerPOST.");
                }
            }
            return (htmlPostForm, errors);
        }

        private string GenerateHtmlPostForm(string? action, string? id, IDictionary<string, string?> parameters, string inputType = "hidden", bool autoSubmit = false)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            htmlBuilder.AppendLine($"<form id=\"{id}\" action=\"{action}\" method=\"post\">");
            foreach (var parameter in parameters)
            {
                htmlBuilder.AppendLine($"<input type=\"{inputType}\" name=\"{parameter.Key}\" id=\"{parameter.Key}\" value=\"{parameter.Value}\" />");
            }
            
            if (autoSubmit)
            {
                htmlBuilder.AppendLine($"<script type=\"text/javascript\">document.getElementById(\"{id}\").submit();</script>");
            }

            htmlBuilder.AppendLine("</form>");
            
            return htmlBuilder.ToString();
        }

        public (IDictionary<string, string> feedback, IEnumerable<string> errors) CheckOutFeedback(HttpRequest CurrentRequest)
        {
            string text = string.Empty;
            string text2 = string.Empty;
            List<string> errors = new List<string>();
            IDictionary<string, string> feedback = new Dictionary<string, string>();
            string[] allKeys = CurrentRequest.Form.Keys.ToArray();
            Array.Sort(allKeys);
            foreach (string text3 in allKeys)
            {
                string text4 = CurrentRequest.Form[text3];
                if (text3 != "CheckMacValue")
                {
                    text += $"&{text3}={text4}";
                    if (text3 == "PaymentType")
                    {
                        text4 = text4.Replace("_CVS", string.Empty);
                        text4 = text4.Replace("_BARCODE", string.Empty);
                        text4 = text4.Replace("_Alipay", string.Empty);
                        text4 = text4.Replace("_Tenpay", string.Empty);
                        text4 = text4.Replace("_CreditCard", string.Empty);
                    }
                    if (text3 == "PeriodType")
                    {
                        text4 = text4.Replace("Y", "Year");
                        text4 = text4.Replace("M", "Month");
                        text4 = text4.Replace("D", "Day");
                    }
                    feedback.Add(text3, text4);
                }
                else
                {
                    text2 = text4;
                }
            }
            Logger.WriteLine(string.Format("INFO   {0}  INPUT   AllInOne.CheckOutFeedback: {1}&CheckMacValue={2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text, text2));
            errors.AddRange(CompareCheckMacValue(text, text2));
            return (feedback, errors);
        }

        public async Task<(IDictionary<string, string> feedback, IEnumerable<string> errors)> QueryTradeInfoAsync()
        {
            string text = string.Empty;
            string empty = string.Empty;
            List<string> errors = new List<string>();
            IDictionary<string, string> feedback = new Dictionary<string, string>();
            errors.AddRange(ServerValidator.Validate(this));
            errors.AddRange(ServerValidator.Validate(Query));
            if (errors.Count == 0)
            {
                empty += BuildParamenter("MerchantID", MerchantID);
                empty += BuildParamenter("MerchantTradeNo", Query.MerchantTradeNo);
                empty += BuildParamenter("TimeStamp", Query.TimeStamp);
                string empty2 = BuildCheckMacValue(empty);
                empty += BuildParamenter("CheckMacValue", empty2);
                empty = empty[1..];
                Logger.WriteLine(string.Format("INFO   {0}  OUTPUT  AllInOne.QueryTradeInfo: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), empty));
                if (ServiceMethod == HttpMethod.ServerPOST)
                {
                    text = await ServerPostAsync(empty);
                }
                else
                {
                    errors.Add("No service for HttpPOST and HttpGET.");
                }
                Logger.WriteLine(string.Format("INFO   {0}  INPUT   AllInOne.QueryTradeInfo: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text));
                if (!string.IsNullOrEmpty(text))
                {
                    empty = string.Empty;
                    empty2 = string.Empty;
                    string[] array = text.Split('&');
                    foreach (string text2 in array)
                    {
                        if (string.IsNullOrEmpty(text2))
                        {
                            continue;
                        }
                        string[] array2 = text2.Split('=');
                        string text3 = array2[0];
                        string text4 = array2[1];
                        if (text3 != "CheckMacValue")
                        {
                            empty += $"&{text3}={text4}";
                            if (text3 == "PaymentType")
                            {
                                text4 = text4.Replace("_CVS", string.Empty);
                                text4 = text4.Replace("_BARCODE", string.Empty);
                                text4 = text4.Replace("_Alipay", string.Empty);
                                text4 = text4.Replace("_Tenpay", string.Empty);
                                text4 = text4.Replace("_CreditCard", string.Empty);
                            }
                            if (text3 == "PeriodType")
                            {
                                text4 = text4.Replace("Y", "Year");
                                text4 = text4.Replace("M", "Month");
                                text4 = text4.Replace("D", "Day");
                            }
                            feedback.Add(text3, text4);
                        }
                        else
                        {
                            empty2 = text4;
                        }
                    }
                    if (string.IsNullOrEmpty(empty2))
                    {
                        errors.Add(string.Format("ErrorCode: {0}", feedback["TradeStatus"]));
                    }
                    else
                    {
                        errors.AddRange(CompareCheckMacValue(empty, empty2));
                    }
                }
            }
            return (feedback, errors);
        }

        public async Task<(PeriodCreditCardTradeInfo? feedback, IEnumerable<string> errors)> QueryPeriodCreditCardTradeInfoAsync()
        {
            string text = string.Empty;
            string empty = string.Empty;
            List<string> errors = new List<string>();
            PeriodCreditCardTradeInfo? feedback = new PeriodCreditCardTradeInfo();
            errors.AddRange(ServerValidator.Validate(this));
            errors.AddRange(ServerValidator.Validate(Query));
            if (errors.Count == 0)
            {
                empty += BuildParamenter("MerchantID", MerchantID);
                empty += BuildParamenter("MerchantTradeNo", Query.MerchantTradeNo);
                empty += BuildParamenter("TimeStamp", Query.TimeStamp);
                string empty2 = BuildCheckMacValue(empty);
                empty += BuildParamenter("CheckMacValue", empty2);
                empty = empty[1..];
                Logger.WriteLine(string.Format("INFO   {0}  OUTPUT  AllInOne.QueryTradeInfo: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), empty));
                if (ServiceMethod == HttpMethod.ServerPOST)
                {
                    text = await ServerPostAsync(empty);
                    feedback = JsonSerializer.Deserialize<PeriodCreditCardTradeInfo>(text);
                }
                else
                {
                    errors.Add("No service for HttpPOST and HttpGET.");
                }
                Logger.WriteLine(string.Format("INFO   {0}  INPUT   AllInOne.QueryTradeInfo: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text));
            }
            return (feedback, errors);
        }

        public async Task<(IDictionary<string, string> feedback, IEnumerable<string> errors)> DoActionAsync()
        {
            string text = string.Empty;
            string empty = string.Empty;
            List<string> errors = new List<string>();
            IDictionary<string, string> feedback = new Dictionary<string, string>();
            errors.AddRange(ServerValidator.Validate(this));
            errors.AddRange(ServerValidator.Validate(Action));
            if (errors.Count == 0)
            {
                _ = Send.ChoosePayment.ToString();
                empty += BuildParamenter("Action", Action.Action.ToString());
                empty += BuildParamenter("MerchantID", MerchantID);
                empty += BuildParamenter("MerchantTradeNo", Action.MerchantTradeNo);
                empty += BuildParamenter("TotalAmount", Action.TotalAmount);
                empty += BuildParamenter("TradeNo", Action.TradeNo);
                string empty2 = BuildCheckMacValue(empty);
                empty += BuildParamenter("CheckMacValue", empty2);
                empty = empty[1..];
                Logger.WriteLine(string.Format("INFO   {0}  OUTPUT  AllInOne.DoAction: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), empty));
                if (ServiceMethod == HttpMethod.ServerPOST)
                {
                    text = await ServerPostAsync(empty);
                }
                else
                {
                    errors.Add("No service for HttpPOST, HttpGET.");
                }
                Logger.WriteLine(string.Format("INFO   {0}  INPUT   AllInOne.DoAction: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text));
                if (!string.IsNullOrEmpty(text))
                {
                    empty = string.Empty;
                    string[] array = text.Split('&');
                    foreach (string text3 in array)
                    {
                        if (!string.IsNullOrEmpty(text3))
                        {
                            string[] array2 = text3.Split('=');
                            string text4 = array2[0];
                            string text5 = array2[1];
                            if (text4 != "CheckMacValue")
                            {
                                empty += $"&{text4}={text5}";
                                feedback.Add(text4, text5);
                            }
                            else
                            {
                                empty2 = text5;
                            }
                        }
                    }
                    if (feedback.ContainsKey("RtnCode") && !"1".Equals(feedback["RtnCode"]))
                    {
                        errors.Add(string.Format("{0}: {1}", feedback["RtnCode"], feedback["RtnMsg"]));
                    }
                }
            }
            return (feedback, errors);
        }

        public async Task<(IDictionary<string, string> feedback, IEnumerable<string> errors)> AioChargebackAsync()
        {
            string text = string.Empty;
            string empty = string.Empty;
            List<string> errors = new List<string>();
            IDictionary<string, string> feedback = new Dictionary<string, string>();
            errors.AddRange(ServerValidator.Validate(this));
            errors.AddRange(ServerValidator.Validate(ChargeBack));
            if (errors.Count == 0)
            {
                empty += BuildParamenter("ChargeBackTotalAmount", ChargeBack.ChargeBackTotalAmount);
                empty += BuildParamenter("MerchantID", MerchantID);
                empty += BuildParamenter("MerchantTradeNo", ChargeBack.MerchantTradeNo);
                empty += BuildParamenter("Remark", ChargeBack.Remark);
                empty += BuildParamenter("TradeNo", ChargeBack.TradeNo);
                string empty2 = BuildCheckMacValue(empty);
                empty += BuildParamenter("CheckMacValue", empty2);
                empty = empty[1..];
                Logger.WriteLine(string.Format("INFO   {0}  OUTPUT  AllInOne.AioChargeback: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), empty));
                if (ServiceMethod == HttpMethod.ServerPOST)
                {
                    text = await ServerPostAsync(empty);
                }
                else
                {
                    errors.Add("No service for HttpPOST, HttpGET.");
                }
                Logger.WriteLine(string.Format("INFO   {0}  INPUT   AllInOne.AioChargeback: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text));
                string[] array = text.Split('|');
                if (array.Length == 2)
                {
                    string value = array[0];
                    string value2 = array[1];
                    feedback.Add("RtnCode", value);
                    feedback.Add("RtnMsg", value2);
                }
                if (text != "1|OK")
                {
                    if (text.Length > 2)
                    {
                        errors.Add(text[2..].Replace("-", ": "));
                    }
                    else
                    {
                        errors.Add("Feedback message error!");
                    }
                }
            }
            return (feedback, errors);
        }

        public async Task<IEnumerable<string>> TradeNoAioAsync(string filepath)
        {
            Hashtable hashtable = new Hashtable();
            string text = string.Empty;
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(filepath))
            {
                errors.Add("The filepath is required.");
            }
            errors.AddRange(ServerValidator.Validate(this));
            errors.AddRange(ServerValidator.Validate(TradeFile));
            if (errors.Count == 0)
            {
                hashtable.Add("MerchantID", MerchantID);
                hashtable.Add("DateType", (int)TradeFile.DateType);
                hashtable.Add("BeginDate", TradeFile.BeginDate);
                hashtable.Add("EndDate", TradeFile.EndDate);
                if (TradeFile.PaymentType != 0)
                {
                    hashtable.Add("PaymentType", $"{(int)TradeFile.PaymentType:00}");
                }
                if (TradeFile.PlatformStatus != 0)
                {
                    hashtable.Add("PlatformStatus", (int)TradeFile.PlatformStatus);
                }
                if (TradeFile.PaymentStatus != PaymentState.ALL)
                {
                    hashtable.Add("PaymentStatus", (int)TradeFile.PaymentStatus);
                }
                if (TradeFile.AllocateStatus != AllocateState.ALL)
                {
                    hashtable.Add("AllocateStauts", (int)TradeFile.AllocateStatus);
                }
                hashtable.Add("MediaFormated", TradeFile.NewFormatedMedia ? 1 : 0);
                if (TradeFile.CharSet != 0)
                {
                    hashtable.Add("CharSet", (byte)TradeFile.CharSet);
                }

                string empty = RenderControlAndParamenter(hashtable);
                string empty2 = BuildCheckMacValue(empty);
                empty += BuildParamenter("CheckMacValue", empty2);
                empty = empty[1..];
                Logger.WriteLine(string.Format("INFO   {0}  OUTPUT  AllInOne.TradeNoAio: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), empty));
                if (ServiceMethod == HttpMethod.ServerPOST)
                {
                    text = await ServerPostAsync(empty, Encoding.GetEncoding("Big5"));
                }
                else
                {
                    errors.Add("No service for HttpPOST, HttpGET.");
                }
                Logger.WriteLine(string.Format("INFO   {0}  INPUT   AllInOne.TradeNoAio: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), text));
                CharSetState charSet = ((TradeFile.CharSet == CharSetState.Default) ? CharSetState.UTF8 : TradeFile.CharSet);
                using StreamWriter streamWriter = new StreamWriter(filepath, append: false, CharSetHelper.GetCharSet(charSet));
                streamWriter.Write(text);
            }
            return errors;
        }

        private string BuildCheckMacValue(string parameters, int encryptType = 0)
        {
            string empty = $"HashKey={HashKey}{parameters}&HashIV={HashIV}";
            empty = WebUtility.UrlEncode(empty).ToLower();
            if (encryptType == 1)
            {
                return SHA256Encoder.Encrypt(empty);
            }
            return MD5Encoder.Encrypt(empty);
        }

        private IEnumerable<string> CompareCheckMacValue(string parameters, string checkMacValue)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(checkMacValue))
            {
                string text = BuildCheckMacValue(parameters);
                string text2 = BuildCheckMacValue(parameters, 1);
                if (checkMacValue != text && checkMacValue != text2)
                {
                    list.Add("CheckMacValue verify fail.");
                }
            }
            else if (string.IsNullOrEmpty(checkMacValue))
            {
                list.Add("No CheckMacValue parameter.");
            }
            return list;
        }

        private string BuildParamenter(string id, object? value)
        {
            string? arg = string.Empty;
            if (null != value)
            {
                arg = ((!value.GetType().Equals(typeof(DateTime))) ? value.ToString() : ((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss"));
            }
            return $"&{id}={arg}";
        }

        private string RenderControlAndParamenter(string id, object value, IDictionary<string, string?> htmlDoc)
        {
            string? value2 = string.Empty;
            if (null != value)
            {
                value2 = ((!value.GetType().Equals(typeof(DateTime))) ? value.ToString() : ((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss"));
            }
            htmlDoc.Add(id, value2);
            return BuildParamenter(id, value);
        }

        private string RenderControlAndParamenter(Hashtable parameters, IDictionary<string, string?> htmlDoc)
        {
            string text = string.Empty;
            ArrayList? arrayList = new ArrayList(parameters.Keys);
            arrayList.Sort();
            foreach (string item in arrayList)
            {
                string? value = string.Empty;
                object? obj = parameters[item];
                if (null != obj)
                {
                    value = ((!obj.GetType().Equals(typeof(DateTime))) ? obj.ToString() : ((DateTime)obj).ToString("yyyy/MM/dd HH:mm:ss"));
                }
                htmlDoc.Add(item, value);
                text += BuildParamenter(item, obj);
            }
            return text;
        }


        private string RenderControlAndParamenter(Hashtable parameters)
        {
            string text = string.Empty;
            ArrayList? arrayList = new ArrayList(parameters.Keys);
            arrayList.Sort();
            foreach (string item in arrayList)
            {
                string? value = string.Empty;
                object? obj = parameters[item];
                if (null != obj)
                {
                    value = ((!obj.GetType().Equals(typeof(DateTime))) ? obj.ToString() : ((DateTime)obj).ToString("yyyy/MM/dd HH:mm:ss"));
                }
                text += BuildParamenter(item, obj);
            }
            return text;
        }

        private async Task<string> ServerPostAsync(string parameters)
        {
            string result = string.Empty;
            if (_httpClient != null)
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                HttpContent content = new StringContent(parameters, Encoding.UTF8, "application/x-www-form-urlencoded");
                using HttpResponseMessage response = await _httpClient.PostAsync(ServiceURL, content);
                if (response.IsSuccessStatusCode)
                {
                    result = (await response.Content.ReadAsStringAsync()).Trim();
                }
                else
                {
                    throw new Exception($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");
                }
            }
            return result;
        }

        private async Task<string> ServerPostAsync(string parameters, Encoding receviceEncoding)
        {
            string result = string.Empty;
            if (_httpClient != null)
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                HttpContent content = new StringContent(parameters, Encoding.UTF8, "application/x-www-form-urlencoded");
                using HttpResponseMessage response = await _httpClient.PostAsync(ServiceURL, content);
                if (response.IsSuccessStatusCode)
                {
                    byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();
                    result = receviceEncoding.GetString(responseBytes).Trim();
                }
                else
                {
                    throw new Exception($"Server returned status code {response.StatusCode}: {response.ReasonPhrase}");
                }
            }

            return result;
        }
    }

}
