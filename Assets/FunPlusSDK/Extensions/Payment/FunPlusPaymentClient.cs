using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
											 Dictionary<string, String> data,
											 Action onSuccess,
											 Action<string> onFailure)
		{
			string url = GetPaymentServerUrl (environment, channel);

			WWWForm wf = new WWWForm ();
			foreach (KeyValuePair<string, string> entry in data)
			{
				wf.AddField (entry.Key, entry.Value);
			}

			StartCoroutine (Post (url, wf, onSuccess, onFailure));
		}

		private IEnumerator Post(string url, WWWForm wf, Action onSuccess, Action<string> onFailure)
		{
			WWW www = new WWW (url, wf);
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
				Debug.Log (www.text);
				var dict = Json.Deserialize (www.text) as Dictionary<string, object>;

				if (!dict.ContainsKey ("status")) {
					onFailure ("Invalid response: the `status` field is missing");
				} else if ((long) dict ["status"] != 1) {
					string reason = dict.ContainsKey ("reason") ? dict ["reason"] as string : "Unknown error";
					onFailure (reason);
				}
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
									Dictionary<string, String> data,
									Action onSuccess,
									Action<string> onFailure)
		{
			Instance.SendDataToPaymentServer (
				environment,
				channel,
				data,
				onSuccess,
				onFailure
			);
		}
	}

}