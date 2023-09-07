using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsManager : MonoBehaviour
{
    public float LightsIntensity = 1f;
    public float LightsRange = 10f;
    void Start()
    {
        float lights = transform.childCount;

        for (int i = 0;i < lights; i++)
        {
            GameObject lightObj = transform.GetChild(i).gameObject;
            Light light = lightObj.GetComponent<Light>();
            light.enabled = false;
            light.intensity = LightsIntensity;
            light.range = LightsRange;
            light.enabled = true;
        }
    }
}
