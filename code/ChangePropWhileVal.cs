using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ChangePropWhileVal : MonoBehaviour
{
    [SerializeField] string propName;
    [SerializeField] Vector3 color1;
    [SerializeField] Vector3 color2;

    private VisualEffect visEff;
    // Start is called before the first frame update
    void Start()
    {
        visEff = GetComponent<VisualEffect>();
        visEff.SetVector3(propName, color1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeProp()
    {
        if(visEff.GetVector3(propName) == color1)
        {
            visEff.SetVector3(propName, color2);
        } 
        else
        {
            visEff.SetVector3(propName, color1);
        }

    }
}
