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
		PlayStore,
		AppStore,
		Amazon
	}

	public class FunPlusPaymentClient : MonoBehaviour
	{
		public static string VERSION = "4.0.0-alpha.0";

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
				onFailure ("Failed to request to payment server");
			}
			else if (string.IsNullOrEmpty (www.text))
			{
				onFailure ("Invalid response from payment server");
			}
			else
			{
				var dict = Json.Deserialize (www.text) as Dictionary<string, object>;

				if (!dict.ContainsKey ("status")) 
				{
					onFailure ("Invalid response: the `status` field is missing");
				}
				else if ((long)dict ["status"] != 1)
				{
					string reason = dict.ContainsKey ("reason") ? dict ["reason"] as string : "Unknown error";
					onFailure (reason);
				}
				else
				{
					onSuccess ();
				}
			}
		}

		private static string GetPaymentServerUrl(PaymentEnvironment environment, PaymentChannel channel)
		{
			string host = environment.Equals (PaymentEnvironment.Sandbox) ?
				"https://payment-sandbox.funplusgame.com" :
				"https://payment.funplusgame.com";

			string channelName;

			switch (channel) {
			case PaymentChannel.PlayStore:
				channelName = "googleplayiap";
				break;
			case PaymentChannel.AppStore:
				channelName = "appleiap";
				break;
			case PaymentChannel.Amazon:
				channelName = "amazoniap";
				break;
			default:
				channelName = "unknown";
				break;
			}

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