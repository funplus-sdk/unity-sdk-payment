# FunPlus Payment SDK for Unity

## Requirements

* Unity 5.3+

## Integration

First, Import the SDK package `funplus-unity-payment-sdk-<version>.unitypackage` to your project.

Then, add the prefab located at `Assets/FunPlusSDK/Extensions/Payment/Prefab/FunPlusPaymentClient.prefab` to the first scene of your game.

And you are ready to use it.

## Usage

After user succeeds an in-app purchase, app needs to submit the purchase information to FunPlus Payment System. Here's how to do it.

```csharp
using System.Collections.Generic;
using FunPlus.Payment;

void OnRequestSuccess()
{
  	Debug.Log ("Data submitting success");
}

void OnRequestFailure(string reason)
{
  	Debug.Log ("Data submitting failure, reason: " + reason);
}

var data = new Dictionary<string, string> ();
data.Add ("appid", "107");
data.Add ("uid", "testuser");
data.Add ("appservid", "serverid");
data.Add ("product_id", "com.funplus.gold1000");
data.Add ("signature", "purchase signature");
data.Add ("signed_data", "purchase signed data");
data.Add ("through_cargo", "through_cargo");
data.Add ("amount", "10.0");
data.Add ("currency_code", "usd");

FunPlusPaymentClient.SendData (
  	PaymentEnvironment.Production,
  	PaymentChannel.PlayStore,
  	data,
  	OnRequestSuccess,
  	OnRequestFailure
);
```

For different channels, the `data` dictionary should contain different fields. Here's a detailed description about it.

**App Store**

| field         | description                              |
| ------------- | ---------------------------------------- |
| appid         | The FunPlus Payment app ID.              |
| uid           | User ID.                                 |
| receipt_data  | Receipt data received from App Store.    |
| through_cargo | A development payload, you can put here anything you want to identify current purchase. |

**Play Store**

| field         | description                              |
| ------------- | ---------------------------------------- |
| appid         | The FunPlus Payment app ID.              |
| uid           | User ID.                                 |
| product_id    | The product ID.                          |
| signature     | Purchase signature received from Play Store. |
| signed_data   | Purchase signed data received from Play Store. |
| through_cargo | A development payload, you can put here anything you want to identify current purchase. |

## FAQ