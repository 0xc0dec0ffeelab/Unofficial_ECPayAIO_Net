# Unofficial_ECPayAIO_Net
Unofficial ECPayAIO_Net support  .NET 8

## Installation

1. apply for [personal access token @GitHub](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry#installing-a-package)
2. add https://nuget.pkg.github.com/0xc0dec0ffeelab/index.json as nuget source and input the password (=personal access token)
   

## Quick Start
```cs


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
