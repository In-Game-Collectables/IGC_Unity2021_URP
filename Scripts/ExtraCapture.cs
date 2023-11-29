using IGC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ExtraCapture : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + gameObject.transform.forward * 2);
        Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + gameObject.transform.up * 0.5f);
        Gizmos.matrix = Matrix4x4.TRS(this.transform.position, this.transform.rotation, Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, 45, 0 , 0.5f, 1);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
