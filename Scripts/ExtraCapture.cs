using IGC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ExtraCapture : MonoBehaviour
{

    public float Length = 1;
    public float Height = 0.33f;
    public float FrustumLength = 0.25f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + gameObject.transform.forward * Length);
        Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + gameObject.transform.up * Height);
        Gizmos.matrix = Matrix4x4.TRS(this.transform.position, this.transform.rotation, Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, 45, 0 , FrustumLength, 1);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
