using UnityEngine;
using System.Collections;
using FunPlus.Payment;
using System.Collections.Generic;

public class UIManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Dictionary<string, string> data = new Dictionary<string, string> ();
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
			PaymentEnvironment.Sandbox,
			PaymentChannel.Google,
			data,
			OnRequestFailure,
			OnRequestFailure
		);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnRequestSuccess()
	{
		Debug.Log ("Request success");
	}

	void OnRequestFailure(string reason)
	{
		Debug.Log ("Request failure, reason: " + reason);
	}
}
