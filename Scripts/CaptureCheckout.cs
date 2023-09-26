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

        public void GetCheckoutQR(string checkoutURL, Action<string, Texture2D> successCallback, Action<string> failureCallback, string savePath = " ")
        {
            checkoutCoroutine = GetQRFromAPI(checkoutURL + "/qr", successCallback, failureCallback, savePath: savePath);
            StartCoroutine(checkoutCoroutine);
        }

        private IEnumerator GetQRFromAPI(string URL, Action<string, Texture2D> successCallback, Action<string> failureCallback, string savePath = " ")
        {
            var request = UnityWebRequestTexture.GetTexture(URL);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                checkoutCoroutine = null;
                Debug.LogError($"Error getting QR: {request.error}\nResponse Code:{request.responseCode}");
                failureCallback($"Error getting QR: {request.error}\nResponse Code:{request.responseCode}");
            }
            else
            {
                Debug.Log("Success: Connected to API Checkout");
                var qrCode = new Texture2D(1, 1);
                qrCode = DownloadHandlerTexture.GetContent(request);
                if (qrCode == null)
                {
                    Debug.Log("Error: QR Image seems to be empty?");
                    checkoutCoroutine = null;
                    failureCallback("QR Code image seems to be empty");
                }
                else
                {
                    // Optional?
                    if (!string.IsNullOrEmpty(savePath))
                    {
                        savePath = savePath + "Checkout_QR.png";
                        var bytes = qrCode.EncodeToPNG();
                        File.WriteAllBytes(savePath, bytes);
                    }
                    checkoutCoroutine = null;
                    successCallback(URL, qrCode);
                }

            }
        }
    }
}