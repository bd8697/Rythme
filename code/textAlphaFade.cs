using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class textAlphaFade : MonoBehaviour
{

    TMP_Text text;
    bool fadeIn = false;
    float alpha;
    Color color;
    bool fading = false;
    float fadeSpeed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
        color = text.GetComponent<TMP_Text>().color;

        text.materialForRendering.SetColor("_FaceColor", new Color(127, 255, 255, 255) / 255f);
        text.materialForRendering.SetColor("_GlowColor", new Color(191, 255, 255, 255) / 255f);
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
                }
            }
            else if (!fadeIn)
            {
                alpha -= alpha * Time.deltaTime * fadeSpeed;
                if (alpha < 0.01f)
                {
                    alpha = 0f;
                    fading = false;
                }
            }
            color.a = alpha;
            text.color = color;
        }
    }

    public void FadeObject(bool fI, float fS)
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

    }
}

