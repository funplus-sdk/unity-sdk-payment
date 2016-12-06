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
data.Add ("product_id", "product_1");
data.Add ("signature", "sig");
data.Add ("signed_data", "signed");
data.Add ("through_cargo", "through_cargo");
data.Add ("appservid", "serverid");
data.Add ("amount", "10.0");
data.Add ("currency_code", "usd");

FunPlusPaymentClient.SendData (
  	PaymentEnvironment.Production,
  	PaymentChannel.Google,
  	data,
  	OnRequestSuccess,
  	OnRequestFailure
);
```



## FAQ