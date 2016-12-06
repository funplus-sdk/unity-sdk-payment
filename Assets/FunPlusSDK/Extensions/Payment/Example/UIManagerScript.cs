using UnityEngine;
using System.Collections;
using FunPlus.Payment;

public class UIManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FunPlusPaymentClient.SendData (
			PaymentEnvironment.Sandbox,
			PaymentChannel.Google,
			"123",
			"456",
			"product_1",
			"sig",
			"signed",
			"though_cargo"
		);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
