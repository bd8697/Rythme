using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFrustrumCulling : MonoBehaviour
{

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.cullingMatrix = Matrix4x4.Ortho(-999, 999, -999, 999, 0.01f, 999) * Matrix4x4.Translate(Vector3.forward * -999 / 2f) * cam.worldToCameraMatrix;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnPreCull()
    //{
    //    cam.cullingMatrix = Matrix4x4.Ortho(-99999, 99999, -99999, 99999, 0.001f, 99999) * Matrix4x4.Translate(Vector3.forward * -99999 / 2f) * cam.worldToCameraMatrix;
    //}

    private void OnDisable()
    {
        cam.ResetCullingMatrix();
    }
}
