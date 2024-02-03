using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LinkPosToEmitter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<VisualEffect>().SetVector3("Emitter Pos", new Vector3(transform.position.x, transform.position.y, transform.position.z));
    }
}
