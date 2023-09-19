using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGC
{
    public class CapturerLighting : MonoBehaviour
    {
        private bool UseSceneLights = true;

        [Tooltip("Lights to turn off if using custom lighting")]
        private GameObject[] LightsToTurnOff;
        private GameObject[] SpawnedLights;

        private bool[] IsLightsOn;
        private SphericalManager Spherical;


        void Start()
        {
            //Spherical = GetComponent<SphericalManager>();
            //IsLightsOn = new bool[LightsToTurnOff.Length];

            //if (!UseSceneLights)
            //{
            //    for (int i = 0; i < LightsToTurnOff.Length; i++)
            //    {
            //        if (LightsToTurnOff[i].TryGetComponent<Light>(out Light light))
            //        {
            //            IsLightsOn[i] = light.enabled;
            //        }
            //        else
            //        {
            //            IsLightsOn[i] = false;
            //        }
            //    }
            //}
        }

        public void SetUpLights()
        {
            if (UseSceneLights)
                return;
            for (int i = 0; i < LightsToTurnOff.Length; i++)
            {
                if (IsLightsOn[i] == true && LightsToTurnOff[i].TryGetComponent<Light>(out Light light))
                {
                    light.enabled = true;
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
            SpawnedLights = new GameObject[6];
            float lights = 4;

            Vector3 location = Spherical.GetTopLocation(radius, center);
            GameObject light = new GameObject();
            SpawnedLights[3] = light;
            Light lightComp = light.AddComponent<Light>();
            lightComp.intensity = lightIntensity;
            light.transform.position = location;

            Vector3 location2 = Spherical.GetBottomLocation(radius, center);
            GameObject light2 = new GameObject();
            SpawnedLights[3] = light2;
            Light lightComp2 = light2.AddComponent<Light>();
            lightComp2.intensity = lightIntensity;
            light2.transform.position = location2;

            for (int i = 0; i < lights; i++)
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
}