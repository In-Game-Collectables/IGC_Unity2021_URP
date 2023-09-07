using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturerLighting : MonoBehaviour
{
    public bool UseSceneLights = true;

    [Tooltip("Lights to turn off if using custom lighting")]
    public GameObject[] LightsToTurnOff;
    public GameObject[] SpawnedLights;

    private bool[] IsLightsOn;
    private SphericalManager Spherical;


    void Start()
    {
        Spherical = GetComponent<SphericalManager>();
        IsLightsOn = new bool[LightsToTurnOff.Length];

        if (!UseSceneLights)
        {
            for (int i = 0; i < LightsToTurnOff.Length; i++)
            {
                if (LightsToTurnOff[i].TryGetComponent<Light>(out Light light))
                {
                    IsLightsOn[i] = light.enabled;
                }
                else
                {
                    IsLightsOn[i] = false;
                }
            }
        }
    }

    public void SetUpLights()
    {
        if (UseSceneLights)
                return;
        for (int i = 0; i < LightsToTurnOff.Length; i++)
        {
            if (IsLightsOn[i] == true && LightsToTurnOff[i].TryGetComponent<Light>(out Light light))
            {
                light.enabled = false;
            }
        }
    }

    public void ResetLights()
    {
        if (UseSceneLights)
            return;
        for (int i = 0; i < LightsToTurnOff.Length; i++)
        {
            if (IsLightsOn[i])
            {
                LightsToTurnOff[i].GetComponent<Light>().enabled = true;
            }
        }

        for (int i = 0; i < SpawnedLights.Length; i++)
        {
            Destroy(SpawnedLights[i]);
        }
    }

    public void SpawnLights(float radius, Vector3 center, Vector3 targetDirection, float lightIntensity = 3.5f)
    {
        if (UseSceneLights)
            return;
        SpawnedLights = new GameObject[4];
        float lights = 3;

        Vector3 location = Spherical.GetTopLocation(radius, center);
        GameObject light = new GameObject();
        SpawnedLights[3] = light;
        Light lightComp = light.AddComponent<Light>();
        lightComp.intensity = lightIntensity;
        light.transform.position = location;

        for (int i = 0; i < 3; i++)
        {
            location = Spherical.GetCenterLocation(i / lights, radius, center, targetDirection);
            light = new GameObject();
            SpawnedLights[i] = light;
            lightComp = light.AddComponent<Light>();
            lightComp.intensity = lightIntensity;
            light.transform.position = location;
        }
    }

}
