using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tint : MonoBehaviour
{
    float noAnimFontSize;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<TMP_Text>())
        {
            noAnimFontSize = GetComponent<TMP_Text>().fontSize;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void tintOnTick(Color globalTint) // not the most optimal solution, but it's clean
    {
        if (GetComponent<MeshRenderer>())
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", globalTint / 255);
        }
        else if (GetComponent<TMP_Text>())
        {
            if(GetComponent<TMP_Text>().fontSize == noAnimFontSize) // if fontSize different than the set one, animation is happening, don't intrerupt it.
            {
                Color faceColor = globalTint / 255;
                faceColor.r += (1 - faceColor.r) / 2;
                faceColor.g += (1 - faceColor.g) / 2;
                faceColor.b += (1 - faceColor.b) / 2;
                GetComponent<TMP_Text>().materialForRendering.SetColor("_FaceColor", faceColor);
                GetComponent<TMP_Text>().materialForRendering.SetColor("_GlowColor", globalTint / 255);
            }
        }
        else if (GetComponent<ParticleSystem>())
        {
            var main = GetComponent<ParticleSystem>().main;
            main.startColor = globalTint / 255;
        }
    }
}
