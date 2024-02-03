using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteAlphaFade : MonoBehaviour
{

    SpriteRenderer sprRen;
    bool fadeIn = false;
    float alpha;
    Color color;
    bool fading = false;
    float fadeSpeed = 0f;

    public delegate void DoAtPeak();
    public DoAtPeak peakDel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(fading)
        {
            if(fadeIn)
            {
                alpha += alpha * Time.deltaTime * fadeSpeed;
                if(alpha > 1f)
                {
                    alpha = 1f;
                    fading = false;
                    peakDel?.Invoke();
                }
            } 
            else if(!fadeIn)
            {
                alpha -= alpha * Time.deltaTime * fadeSpeed;
                if (alpha < 0.01f)
                {
                    alpha = 0f;
                    fading = false;
                    peakDel?.Invoke();
                }
            }

            color.a = alpha;
            sprRen.color = color;
        }
    }

    public void FadeObject(bool fI, float fS)
    {
        SetUpFade(fI, fS);
    }

    public void FadeObject(bool fI, float fS, DoAtPeak doAtPeak)
    {
        peakDel = doAtPeak;
        SetUpFade(fI, fS);
    }

    private void SetUpFade(bool fI, float fS)
    {
        fading = true;
        fadeIn = fI;
        fadeSpeed = fS;

        sprRen = gameObject.GetComponent<SpriteRenderer>();
        if (fI)
        {
            alpha = 0.01f;
        }
        else
        {
            alpha = 1f;
        }
        color = sprRen.GetComponent<SpriteRenderer>().color;
    }
}
