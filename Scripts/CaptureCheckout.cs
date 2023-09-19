using System.Collections;
using UnityEngine;
using System.IO;

using UnityEngine.Networking;
using System;
using UnityEditor.PackageManager.Requests;

namespace IGC
{
    public class CaptureCheckout : MonoBehaviour
    {
        public string TestURL = "https://images.dog.ceo/breeds/pinscher-miniature/n02107312_6617.jpg";

        private IEnumerator checkoutCoroutine;

        public void GetCheckoutQR(string CheckoutURL, Action<string, Texture2D> successCallback, Action<string> failureCallback, string SavePath = " ")
        {
            checkoutCoroutine = GetQRFromAPI(CheckoutURL + "/qr", successCallback, failureCallback, SavePath: SavePath);
            StartCoroutine(checkoutCoroutine);
        }

        private IEnumerator GetQRFromAPI(string URL, Action<string, Texture2D> successCallback, Action<string> failureCallback, string SavePath = " ")
        {
            var request = UnityWebRequestTexture.GetTexture(URL);
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("ERROR: " + request.error);
                Debug.Log("RESPONSE CODE: " + request.responseCode);
                checkoutCoroutine = null;
                failureCallback("Response Code " + request.responseCode + ". QR Code Error: " + request.error);
            }
            else
            {
                Debug.Log("SUCCESS: Connected to API");
                var qrCode = new Texture2D(1, 1);
                qrCode = DownloadHandlerTexture.GetContent(request);
                if (qrCode == null)
                {
                    Debug.Log("ERROR: Image seems to be empty?");
                    checkoutCoroutine = null;
                    failureCallback("QR Code image seems to be empty");
                }
                else
                {
                    // Optional?
                    if (SavePath != " ")
                    {
                        SavePath = SavePath + "Checkout_QR.png";
                        byte[] bytes = qrCode.EncodeToPNG();
                        File.WriteAllBytes(SavePath, bytes);
                    }
                    checkoutCoroutine = null;
                    successCallback(URL, qrCode);
                }

            }
        }
    }
}