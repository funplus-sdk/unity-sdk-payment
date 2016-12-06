using UnityEngine;
using System.Collections;
using System;

namespace FunPlus.Payment
{
	public enum PaymentEnvironment
	{
		Sandbox,
		Production
	}

	public enum PaymentChannel
	{
		Google,
		Apple
	}

	public class FunPlusPaymentClient : MonoBehaviour
	{
		private static FunPlusPaymentClient Instance { get; set; }

		void Awake()
		{
			Instance = this;
		}

		private void SendDataToPaymentServer(PaymentEnvironment environment,
											 PaymentChannel channel,
											 string appId,
											 string userId,
											 string productId,
											 string signature,
											 string signedData,
											 string throughCargo)
		{
			string url = GetPaymentServerUrl (environment, channel);

			WWWForm wf = new WWWForm ();
			wf.AddField ("appid", appId);
			wf.AddField ("uid", userId);

			StartCoroutine (Post (url, wf, null, null));
		}

		private IEnumerator Post(string url, WWWForm wf, Action<Hashtable> onSuccess, Action<Hashtable> onFailure)
		{
			WWW www = new WWW (url, wf);
			Debug.Log (url);
			yield return www;

			if (www == null || www.error != null || !www.isDone)
			{
				Debug.LogWarning ("[FunPlusSDK] Failed to request to payment server.");
			}
			else if (string.IsNullOrEmpty (www.text))
			{
				Debug.LogWarning ("[FunPlusSDK] Invalid response from payment server.");
			}
			else
			{
				Debug.Log(www.text);
			}
		}

		private static string GetPaymentServerUrl(PaymentEnvironment environment, PaymentChannel channel)
		{
			string host = environment.Equals (PaymentEnvironment.Sandbox) ?
				"https://payment-sandbox.funplusgame.com" :
				"https://payment.funplusgame.com";

			string channelName = channel.Equals (PaymentChannel.Google) ?
				"googleplayiap" :
				channel.Equals (PaymentChannel.Apple) ?
				"appleiap" :
				"unknown";

			return string.Format ("{0}/callback/{1}/", host, channelName);
		}

		public static void SendData(PaymentEnvironment environment,
									PaymentChannel channel,
								    string appId,
							 	    string userId,
						   		    string productId,
									string signature,
									string signedData,
									string throughCargo)
		{
			Instance.SendDataToPaymentServer (
				environment,
				channel,
				appId,
				userId,
				productId,
				signature,
				signedData,
				throughCargo
			);
		}
	}

}