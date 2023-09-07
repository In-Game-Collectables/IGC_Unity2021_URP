using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class CharacterCapturer : MonoBehaviour
{
    [Tooltip("GameObject that the camera will capture")]
    public GameObject target;
    public Material MaskerMat;

    private Camera Camera;
    private Camera MaskCamera;
    private Light PointLight;

    [Header("Capture Parameters")]
    public float CaptureRadius = 5;
    public float FOV = 45;
    public int Frames = 100;
    [Range(0,10)]
    private float xSpeed = 5.17f;
    public int Dimension = 1024;

    [Space(10)]

    [Tooltip("Add Y-axis offset to where the camera is looking on the target")]
    public float HeightOffset = 0.85f; // ~half the height of the unity pawn
    private float ScaleMult = 1.5f; // for instant ngp?

    [Tooltip("Layers that will show up for the camera")]
    public string[] ShownLayers;

    private int aabb = 64;
    private string OutputPath;
    [Tooltip("If empty, will render to OUTPUT folder above Asset folder")]
    public string CustomOutputPath;

    private CapturerLighting Lighting;
    private SphericalManager Spherical;
    private GameObject Uploader;

    void Start()
    {
        Spherical = GetComponent<SphericalManager>();
        Lighting = GetComponent<CapturerLighting>();
        if (CustomOutputPath == "")
        {
            OutputPath = Application.dataPath + "/../OUTPUT/Captures/";
        }
        else
        {
            OutputPath = CustomOutputPath;
        }

        SetUpComponents();
        SetUpLayerMasks();
    }

    void LateUpdate()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            Capture();
        }
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            UploadCaptures();
        }
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            CaptureThenUpload();
        }
    }


    //--------------------------------------------------------

    public void CaptureThenUpload()
    {
        Capture();
        UploadCaptures();
    }

    public void Capture()
    {
        Debug.Log("Capture Starting");

        Lighting.SetUpLights();
        PointLight.enabled = true;
        Vector3 center = target.transform.position + new Vector3(0, HeightOffset, 0);
        Lighting.SpawnLights(CaptureRadius, center, target.transform.forward, lightIntensity: CaptureRadius * 1.5f);

        CaptureInformation CI = CreateCaptureInformation();
        for (int i = 0; i < Frames; i++)
        {
            center = target.transform.position + new Vector3(0, HeightOffset, 0);
            transform.position = Spherical.GetSpiralLocation(CaptureRadius, Frames, i, xSpeed, center, target.transform.forward, phiMin:0.01f, phiMax:0.99f);
            // if phi = 0, camera doesn't point to target properly

            transform.LookAt(center);
            string fileName = new string('0', 3 - i.ToString().Length) + i + ".png";
            string filePath = "./images/" + fileName;
            SaveRender(fileName);

            CI = AddToCaptureInformation(i, CI, center, transform: transform, filePath: filePath);
        }
        SaveInformationToJson(CI);
        Lighting.ResetLights();
        PointLight.enabled = false;
        Debug.Log("Capture Finished");
    }

    public void UploadCaptures()
    {
        if (Uploader != null || GameObject.Find("CaptureUploader") != null)
        {
            Debug.Log("Upload is in progress");
        }
        else
        {
            Uploader = new GameObject("CaptureUploader");
            Uploader.AddComponent<CaptureUploader>();
            Uploader.AddComponent<CaptureCheckout>();

            CaptureUploader uploaderComp = Uploader.GetComponent<CaptureUploader>();
            uploaderComp.UploadCaptures(OutputPath);
        }
    }

    //--------------------------------------------------------

    private void SetUpLayerMasks()
    {
        if (ShownLayers.Length == 0)
        {
            Camera.cullingMask = -1; // turns on every layer
            MaskCamera.cullingMask = -1; // turns on every layer
            return;
        }
        foreach (string layerName in ShownLayers)
        {
            int layer = LayerMask.NameToLayer(layerName);
            Camera.cullingMask |= 1 << layer;
            MaskCamera.cullingMask |= 1 << layer;
        }
    }

    private void SetUpComponents()
    {
        Camera = transform.Find("Camera_RGB").GetComponent<Camera>();
        MaskCamera = transform.Find("Camera_Mask").GetComponent<Camera>();
        Camera.targetTexture = new RenderTexture(Dimension, Dimension, 0, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_SRGB);
        MaskCamera.targetTexture = new RenderTexture(Dimension, Dimension, 0, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_SRGB);

        PointLight = transform.Find("Capture_Light").GetComponent<Light>();

        Camera.fieldOfView = FOV;
        MaskCamera.fieldOfView = FOV;
    }

    //--------------------------------------------------------

    private void SaveRender(string filename)
    // gets camera render textures and exports to png
    {
        Texture2D RGBImage = GetCameraImage(Camera);
        MaskerMat.SetTexture("_RGB", RGBImage);
        Texture2D Output;
        if (ShownLayers.Length != 0)
        {
            Texture2D MaskImage = GetCameraImage(MaskCamera);
            MaskerMat.SetTexture("_Mask", MaskImage);
            RenderTexture RT = new RenderTexture(Dimension, Dimension, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
            Graphics.Blit(MaskerMat.mainTexture, RT, MaskerMat, 0); // convert material to render texture

            RenderTexture.active = RT;
            Output = new Texture2D(RT.width, RT.height, TextureFormat.ARGB32, false);
            Output.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
            Destroy(MaskImage);
        }
        else
        {
            Output = RGBImage;
        }

        byte[] bytes = Output.EncodeToPNG();

        Destroy(RGBImage);
        Destroy(Output);

        string filePath = OutputPath + "/images/";
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        File.WriteAllBytes(filePath + filename, bytes);
    }

    private Texture2D GetCameraImage(Camera camera)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;
        camera.Render();

        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = currentRT;

        return image;
    }

    //--------------------------------------------------------

    private CaptureInformation CreateCaptureInformation()
    {
        float camera_angle = Mathf.Deg2Rad * Camera.fieldOfView;
        float focal_length = Dimension / (2 * Mathf.Tan(camera_angle / 2));
        return new CaptureInformation(camera_angle, camera_angle, focal_length, focal_length, Dimension, Dimension, aabb, Frames);
    }

    private CaptureInformation AddToCaptureInformation(int index, CaptureInformation CI, Vector3 center, string filePath="", float sharpness=50, Transform transform=null)
    {
        Transform transf = transform;
        // transform to make compatible with ngp
        transf.position = ((transform.position - center) * -1) * ScaleMult;
        transf.RotateAround(new Vector3(), new Vector3(0, 1, 0), 180);
        transf.RotateAround(new Vector3(), new Vector3(1, 0, 0), 270);
        transf.Rotate(new Vector3(0, 0, 180));

        Matrix4x4 matrix = transf.localToWorldMatrix;
        List<Vector4> tm = new List<Vector4>();

        for (int i = 0; i < 4; i++)
        {
            tm.Add(matrix.GetRow(i));
        }
        FrameInformation FI = new FrameInformation(filePath, sharpness, tm);
        CI.AddFrameInformation(index, FI);

        return CI;
    }

    private void SaveInformationToJson(CaptureInformation CI)
    {
        string output = JsonUtility.ToJson(CI, true);
        System.IO.File.WriteAllText(OutputPath + "/transforms.json", output);
    }
}

//--------------------------------------------------------
