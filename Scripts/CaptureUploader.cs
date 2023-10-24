using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace IGC
{
    [System.Serializable]
    public class Response
    {
        public string id;
        public string created_at;
        public string order_path;
    }
    public class CaptureUploader : MonoBehaviour
    {
        private string URL = "https://platform.igc.studio/api/create";

        public void UploadCaptures(string outputPath, string API_Key, Action<string, Texture2D> successCallback, Action checkoutCallback, Action<string> failureCallback, string API_Scale="", string API_Notes="")
        {
            var form = new List<IMultipartFormSection>();

            var transformsPath = Path.Combine(outputPath, "transforms.json");
            var bytes = File.ReadAllBytes(transformsPath);
            form.Add(new MultipartFormDataSection("api_key", API_Key));
            form.Add(new MultipartFormDataSection("source", "Unity"));
            form.Add(new MultipartFormFileSection("transforms", bytes, "transforms.json", "text/json"));

            if (Directory.Exists(outputPath))
            {
                var dir = new DirectoryInfo(Path.Combine(outputPath, "images"));
                foreach (var file in dir.GetFiles("*.png"))
                {
                    var imagePath = Path.Combine(outputPath, "images", file.Name);
                    var imageBytes = File.ReadAllBytes(imagePath);
                    form.Add(new MultipartFormFileSection("image_set[]", imageBytes, file.Name, "image/png"));
                }
            }

            if (API_Scale != "")
            {
                form.Add(new MultipartFormDataSection("api_param_scale", API_Scale));
            }
            if (API_Notes != "")
            {
                form.Add(new MultipartFormDataSection("notes", API_Notes));
            }

            var request = UnityWebRequest.Post(URL, form);

            Debug.Log("Starting Upload");
            StartCoroutine(OnUploadResponse(request, outputPath, successCallback, checkoutCallback, failureCallback));
        }


        private IEnumerator OnUploadResponse(UnityWebRequest request, string outputPath, Action<string, Texture2D> successCallback, Action checkoutCallback, Action<string> failureCallback)
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.responseCode >= 400)
            {
                Debug.LogError($"Error during Upload: {request.error}\nResponse Code:{request.responseCode}");
                failureCallback($"Error during Upload: {request.error}\nResponse Code:{request.responseCode}");
            }
            else
            {
                DownloadHandler output = request.downloadHandler;
                Debug.Log($"Success: {output.text}\n\nResponse Code:{request.responseCode}");

                var response = JsonUtility.FromJson<Response>(output.text);

                Debug.Log($"Response Id: {response.id} Order Path: {response.order_path}");

                //Application.OpenURL(response.order_path); // pass the url back to the caller via event/task
                checkoutCallback();
                StartCheckout(response.order_path, outputPath, successCallback, failureCallback);
            }
        }

        private void StartCheckout(string QRURL, string outputPath, Action<string, Texture2D> successCallback, Action<string> failureCallback)
        {
            var checkout = gameObject.GetComponent<CaptureCheckout>();
            checkout.GetCheckoutQR(QRURL, successCallback, failureCallback, savePath: outputPath);
        }
    }
}