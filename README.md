# Unofficial_ECPayAIO_Net
Unofficial ECPayAIO_Net support  .NET 8

## Installation

1. apply for [personal access token @GitHub](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry#installing-a-package)
2. add https://nuget.pkg.github.com/0xc0dec0ffeelab/index.json as nuget source and input the password (=personal access token)
3. `dotnet add package Unofficial_ECPayAIO_Net`   

## Quick Start

Program.cs
```cs
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddHttpClient();
```
appsettings.Development.json
```js

{
  "DetailedErrors": true,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ECPay": {
    "ReturnURL": "",  // your ReturnURL
    "OrderResultURL": "", // your OrderResultURL
    "ClientBackURL": "" // your ClientBackURL
  }
}
```

OrderApiController.cs \
`checkoutForm` is your html form id.
```cs

using ECPay.Payment.Integration;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/order")]
public class OrderApiController : Controller
{

  private readonly HttpClient _client;
  private readonly AllInOne _allInOne;
  private readonly IConfiguration _configuration;

  public OrderApiController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
  {
      _client = httpClientFactory.CreateClient();
      _allInOne = new AllInOne(_client, checkoutFormId: "checkoutForm");
      _allInOne.HashKey = "pwFHCqoQZGmho4w6";  //ECPay Hash Key
      _allInOne.HashIV = "EkRm7iFT261dpevs";   //ECPay Hash IV
      _allInOne.MerchantID = "3002607";        //ECPay MerchantID
      _configuration = configuration;
  }

  /// <summary>
  /// generate order html
  /// </summary>
  /// <returns></returns>
  [HttpPost("create")]
  public async Task<IActionResult> Create(OrderViewModel order)
  {
        string? ReturnURL = _configuration["ECPay:ReturnURL"];
        string? OrderResultURL = _configuration["ECPay:OrderResultURL"];
        string? ClientBackURL = _configuration["ECPay:ClientBackURL"];
  
        _allInOne.ServiceMethod = ECPay.Payment.Integration.HttpMethod.HttpPOST;
        _allInOne.ServiceURL = @"https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";
  
        string MerchantTradeNo = $"fd{DateTime.Now.ToString("yyyyMMddHHmmss")}";
  
       _allInOne.Send.ReturnURL = ReturnURL;
       _allInOne.Send.ClientBackURL = ClientBackURL;
       _allInOne.Send.OrderResultURL = OrderResultURL;
       _allInOne.Send.MerchantTradeNo = MerchantTradeNo;
       _allInOne.Send.MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
       _allInOne.Send.TotalAmount = Decimal.Parse("6000");
       _allInOne.Send.TradeDesc = "fd transdesc";
       _allInOne.Send.ChoosePayment = PaymentMethod.ALL;
       _allInOne.Send.Remark = "";
       _allInOne.Send.ChooseSubPayment = PaymentMethodItem.None;
       _allInOne.Send.NeedExtraPaidInfo = ExtraPaymentInfo.Yes;
       _allInOne.Send.DeviceSource = DeviceType.PC;
       _allInOne.Send.IgnorePayment = "";
       _allInOne.Send.PlatformID = "";
       _allInOne.Send.CustomField1 = "";
       _allInOne.Send.CustomField2 = "";
       _allInOne.Send.CustomField3 = "";
       _allInOne.Send.CustomField4 = "";
       _allInOne.Send.EncryptType = 1;
      
       _allInOne.Send.Items.Add(new Item()
       {
           Name = "fd2",
           Price = Decimal.Parse("6000"),
           Currency = "新台幣",
           Quantity = Int32.Parse("1"),
           URL = "",
       });

       var (htmlForm, errors) = _allInOne.CheckOut();

       return Content(htmlForm, "text/html");
      
  }

  /// <summary>
  /// Payment Result Notification
  /// </summary>
  /// <returns></returns>
  [HttpPost("callback")]
  public IActionResult PaymentCallback()
  {
      try
      {
          var (feedback, errors) = _allInOne.CheckOutFeedback(HttpContext.Request);
  
          foreach (var kv in feedback)
          {
              Console.WriteLine($"PaymentCallback key: {kv.Key}, value: {kv.Value}");
          }
  
          return Ok("1|OK");
      }
      catch
      {
          return new ObjectResult("0|Error")
          {
              StatusCode = 500
          };
      }
   }

}


```


index.html (url = "OrderResultURL")
```html

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>order test</title>
</head>
<body>
    <button id="sendPostRequest">Submit Order</button>
    <div id="checkoutContent"></div>
    <script src="./orders.js" defer></script>
</body>
</html>

```

order.js
```js

document.getElementById('sendPostRequest').addEventListener('click', function (event) {
    var data = {
        orderId: '12345',
    };

    fetch([Your_Create_Order_API_Url], {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
    .then(response => response.text())
    .then(html => {
        document.getElementById('checkoutContent').innerHTML = html;
        document.getElementById("checkoutForm").submit(); // checkoutForm is your html form id
    })
    .catch(error => {
        document.getElementById('checkoutContent').innerHTML = '<p>An error occurred while processing your request.</p>';
    });
});
```

## API

### 1. `(string htmlPostForm, IEnumerable<string> errors) CheckOut()`
Order Creation \
(訂單產生)

Input \
None.

Output
| Field            | Type                  | Description                              |
|------------------|-----------------------|------------------------------------------|
| `htmlPostForm`   | `string`             | The HTML form generated for checkout.    |
| `errors`         | `IEnumerable<string>`| List of error messages encountered.      |

---

### 2. `(IDictionary<string, string> feedback, IEnumerable<string> errors) CheckOutFeedback(HttpRequest CurrentRequest)`
General Payment Result Notification / Authorization Success Notification for Credit Card Recurring Payments \
(一般付款結果通知/Credit 定期定額的授權成功通知)

Input
| Field            | Type                 | Description                              |
|------------------|----------------------|------------------------------------------|
| `CurrentRequest` | `HttpRequest`        | The current HTTP request object.         |

Output
| Field            | Type                          | Description                              |
|------------------|-------------------------------|------------------------------------------|
| `feedback`       | `IDictionary<string, string>` | A dictionary containing feedback information. |
| `errors`         | `IEnumerable<string>`         | List of error messages encountered.      |

---

### 3. `async Task<(IDictionary<string, string> feedback, IEnumerable<string> errors)> QueryTradeInfoAsync()`
Order Query \
(訂單查詢)

Input \
None.

Output
| Field            | Type                          | Description                              |
|------------------|-------------------------------|------------------------------------------|
| `feedback`       | `IDictionary<string, string>` | A dictionary containing trade feedback information. |
| `errors`         | `IEnumerable<string>`         | List of error messages encountered.      |

---

### 4. `async Task<(PeriodCreditCardTradeInfo? feedback, IEnumerable<string> errors)> QueryPeriodCreditCardTradeInfoAsync()`
Credit Card Recurring Payment Order Query \
(信用卡定期定額訂單查詢)

Input \
None.

Output
| Field            | Type                              | Description                              |
|------------------|-----------------------------------|------------------------------------------|
| `feedback`       | `PeriodCreditCardTradeInfo?`     | Optional object containing trade information for a period. |
| `errors`         | `IEnumerable<string>`            | List of error messages encountered.      |

---

### 5. `async Task<(IDictionary<string, string> feedback, IEnumerable<string> errors)> DoActionAsync()`
Credit Card Settlement / Refund / Cancellation / Abandonment \
(信用卡關帳／退刷／取消／放棄)

Input \
None.

Output
| Field            | Type                          | Description                              |
|------------------|-------------------------------|------------------------------------------|
| `feedback`       | `IDictionary<string, string>` | A dictionary containing feedback information. |
| `errors`         | `IEnumerable<string>`         | List of error messages encountered.      |

---
### 6. `async Task<IEnumerable<string>> TradeNoAioAsync(string filepath)`
Merchant Reconciliation Media File Download \
(廠商下載對帳媒體檔)

Input
| Field       | Type       | Description               |
|-------------|------------|---------------------------|
| `filepath`  | `string`   | Path to the file to process. |

Output
| Field            | Type                  | Description                              |
|------------------|-----------------------|------------------------------------------|
| `errors`         | `IEnumerable<string>`| List of error messages encountered.      |



## Note
since ["Microsoft.AspNetCore.Http.Abstractions" 2.2.2 is deprecated](https://www.nuget.org/packages/Microsoft.AspNetCore.Http.Abstractions), I remain it as 2.1.1.






## Credits
[ECPayAIO_Net@ECPay](https://github.com/ECPay/ECPayAIO_Net) \
[GPLv2](https://github.com/0xc0dec0ffeelab/Unofficial_ECPayAIO_Net/blob/main/LICENSE)
