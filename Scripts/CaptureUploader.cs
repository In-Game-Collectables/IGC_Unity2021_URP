using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace IGC
{
    public class CaptureUploader : MonoBehaviour
    {
        private string URL = "https://platform.igc.studio/api/create";
        private bool isUploading = false;

        public void UploadCaptures(string OutputPath, string API_Key, Action<string, Texture2D> successCallback, Action<string> failureCallback)
        {
            if (isUploading)
            {
                failureCallback("Upload already in progress");
                Debug.Log("Upload already in progress");
                return;
            }

            List<IMultipartFormSection> form = new List<IMultipartFormSection>();

            byte[] bytes = File.ReadAllBytes(OutputPath + "transforms.json");
            form.Add(new MultipartFormDataSection("api_key", API_Key));
            form.Add(new MultipartFormDataSection("source", "Unity"));
            form.Add(new MultipartFormFileSection("transforms", bytes, "transforms.json", "text/json"));

            if (Directory.Exists(OutputPath))
            {
                DirectoryInfo dir = new DirectoryInfo(OutputPath + "images/");
                foreach (FileInfo file in dir.GetFiles("*.png"))
                {
                    byte[] imageBytes = File.ReadAllBytes(OutputPath + "/images/" + file.Name);
                    form.Add(new MultipartFormFileSection("image_set[]", imageBytes, file.Name, "image/png"));
                }
            }

            UnityWebRequest Request = UnityWebRequest.Post(URL, form);

            Debug.Log("Starting Upload");
            StartCoroutine(OnUploadResponse(Request, OutputPath, successCallback, failureCallback));
        }


        private IEnumerator OnUploadResponse(UnityWebRequest Request, string OutputPath, Action<string, Texture2D> successCallback, Action<string> failureCallback)
        {
            yield return Request.SendWebRequest();

            if (Request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("ERROR: " + Request.error);
                Debug.Log("RESPONSE CODE: " + Request.responseCode);
                failureCallback("Response Code " + Request.responseCode + ". Upload Error: " + Request.error);
            }
            else
            {
                DownloadHandler output = Request.downloadHandler;
                Debug.Log("SUCCESS " + output.text);
                Debug.Log("RESPONSE CODE: " + Request.responseCode);

                if (Request.responseCode >= 400)
                {
                    failureCallback("Something went wrong with the upload");
                    Debug.LogError("Something went wrong with the upload");
                }

                Response response = JsonUtility.FromJson<Response>(output.text);

                Debug.Log(response.id);
                Debug.Log(response.order_path);

                //Application.OpenURL(response.order_path); // pass the url back to the caller via event/task
                StartCheckout(response.order_path, OutputPath, successCallback, failureCallback);
            }
        }

        private void StartCheckout(string QRURL, string OutputPath, Action<string, Texture2D> successCallback, Action<string> failureCallback)
        {
            CaptureCheckout Checkout = gameObject.GetComponent<CaptureCheckout>();
            Checkout.GetCheckoutQR(QRURL, successCallback, failureCallback, SavePath: OutputPath);
        }
    }

    [System.Serializable]
    public class Response
    {
        public string id;
        public string created_at;
        public string order_path;
    }
}