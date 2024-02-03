using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialAlphaFade : MonoBehaviour
{

    List<Material> materials;
    bool fadeIn = false;
    float alpha;
    bool fading = false;
    float fadeSpeed = 0f;
    Color color;

    // Start is called before the first frame update
    void Start()
    {
        materials = new List<Material>();
        color = new Color(1f, 1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            if (fadeIn)
            {
                alpha += alpha * Time.deltaTime * fadeSpeed;
                if (alpha > 1f)
                {
                    alpha = 1f;
                    fading = false;
                    materials.Clear();
                }
            }
            else if (!fadeIn)
            {
                alpha -= alpha * Time.deltaTime * fadeSpeed;
                if (alpha < 0f)
                {
                    alpha = 0f;
                    fading = false;
                    materials.Clear();
                }
            }

            foreach(Material mat in materials)
            {
                if (mat.HasProperty("_GlobalAlpha"))
                {
                    mat.SetFloat("_GlobalAlpha", alpha); // fades fires
                }
                else if(mat.HasProperty("_Color"))
                {
                    color.a = alpha;
                    mat.SetColor("_Color", color); // fades trail
                }
            }
        }
    }

    public void FadeObject(bool fI, Material[] mats, float fS)
    {
        fading = true;
        fadeIn = fI;
        fadeSpeed = fS;

        if (fI)
        {
            alpha = 0.01f;
        }
        else
        {
            alpha = 1f;
        }

        foreach (Material mat in mats)
        {
            materials.Add(mat);

            if (mat.HasProperty("_GlobalAlpha"))
            {
                mat.SetFloat("_GlobalAlpha", alpha); // fades fires
            }
            else if (mat.HasProperty("_Color"))
            {
                color.a = alpha;
                mat.SetColor("_Color", color); // fades trail
            }
        }
    }
}
