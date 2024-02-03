using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LinkVelocityToEmitter : MonoBehaviour
{
    VisualEffect visEff;

    // Start is called before the first frame update
    void Start()
    {
        visEff = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        visEff.SetFloat("Velocity", Mathf.Max(Mathf.Abs(transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity.x), Mathf.Abs(transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity.y)));
     //   Debug.Log(transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity);
    }
}
