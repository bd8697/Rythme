using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LinkPropToVal : MonoBehaviour
{
    [SerializeField] string propName;
    ValueFade valueParent;
    VisualEffect visEff;

    // Start is called before the first frame update
    void Start()
    {
        visEff = GetComponent<VisualEffect>();
        valueParent = GetComponent<ValueFade>();
    }

    // Update is called once per frame
    void Update()
    {
        visEff.SetFloat(propName, valueParent.Val);
    }
}
