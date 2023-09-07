using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CaptureUploader : MonoBehaviour
{
    private string URL = "https://platform.igc.studio/api/create";
    private string Key = "TEST_KEY";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void UploadCaptures(string OutputPath)
    {
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();

        byte[] bytes = File.ReadAllBytes(OutputPath + "transforms.json");
        form.Add(new MultipartFormDataSection("api_key", Key));
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
        StartCoroutine(OnResponse(Request, OutputPath));
    }

    public void DeleteCaptures(string Path)
    {
       if (Directory.Exists(Path + "/images/"))
            Directory.Delete(Path + "/images/", true);

       if (File.Exists(Path + "/transforms.json"))
            File.Delete(Path + "/transforms.json");
    }


    private IEnumerator OnResponse(UnityWebRequest Request, string OutputPath)
    {
        yield return Request.SendWebRequest();

        if (Request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("ERROR: " + Request.error);
            Debug.Log("RESPONSE CODE: " + Request.responseCode);
            Finish();
        }
        else
        {
            DownloadHandler output = Request.downloadHandler;
            Debug.Log("SUCCESS " + output.text);
            Debug.Log("RESPONSE CODE: " + Request.responseCode);

            if (Request.responseCode >= 400)
            {
                Debug.LogError("Something went wrong with the upload");
            }

            Response response = JsonUtility.FromJson<Response>(output.text);

            Debug.Log(response.id);
            Debug.Log(response.order_path);

            Application.OpenURL(response.order_path);
            StartCheckout(response.order_path, OutputPath);
        }
    }

    private void StartCheckout(string QRURL, string OutputPath)
    {
        CaptureCheckout Checkout = gameObject.GetComponent<CaptureCheckout>();
        Checkout.GetCheckoutQR(QRURL, SavePath: OutputPath);
    }

    private void Finish()
    {
        Destroy(gameObject);
    }
}

[System.Serializable]
public class Response
{
    public string id;
    public string created_at;
    public string order_path;
}