using System.Collections;
using UnityEngine;
using System.IO;

using UnityEngine.Networking;
using System;

public class CaptureCheckout : MonoBehaviour
{
    public string TestURL = "https://images.dog.ceo/breeds/pinscher-miniature/n02107312_6617.jpg";

    private void UseQRTexture(Texture2D QRCode)
    {
        /* Put logic in this function to use the QRCode texture for whatevs */
        




        //Finish();
    }


    // -------------------------------------------------------------


    public void GetCheckoutQR(string CheckoutURL, string SavePath = " ")
    {
        StartCoroutine(GetFromAPI(CheckoutURL + "/qr", (callback) =>
        {
            UseQRTexture(callback);
            Debug.Log("Received QR Code texture");
        }, SavePath: SavePath));
    }
    private IEnumerator GetFromAPI(string URL, Action<Texture2D> callback, string SavePath = " ")
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(URL);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("ERROR: " + request.error);
            Debug.Log("RESPONSE CODE: " + request.responseCode);
        }
        else
        {
            Debug.Log("SUCCESS: Connected to API");
            Texture2D QRCode = new Texture2D(1,1);
            QRCode = DownloadHandlerTexture.GetContent(request);
            if (QRCode == null)
            {
                Debug.Log("ERROR: Image seems to be empty?");
            }
            else
            {
                // Optional?
                if (SavePath != " ")
                {
                    SavePath = SavePath + "Checkout_QR.png";
                    byte[] bytes = QRCode.EncodeToPNG();
                    File.WriteAllBytes(SavePath, bytes);
                }
            }
            callback(QRCode);
        }
    }

    private void Finish()
    {
        Destroy(gameObject);
    }
}
