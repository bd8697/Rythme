using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleAlphaFade : MonoBehaviour
{
    private ParticleSystem particles;
    private bool fadeIn = false;
    private float alpha;
    private Color color;
    private bool fading = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            if (fadeIn)
            {
                alpha += alpha * Time.deltaTime * 5;
                if (alpha > 0.5f)
                {
                    alpha = 0.5f;
                    fading = false;
                }
            }
            else if (!fadeIn)
            {
                alpha -= alpha * Time.deltaTime * 5;
                if (alpha < 0f)
                {
                    alpha = 0f;
                    fading = false;
                }
            }

            color.a = alpha;
            particles.startColor = color;
        }
    }

    public void FadeObject(bool fI)
    {
        fading = true;
        fadeIn = fI;
        particles = gameObject.GetComponent<ParticleSystem>();
        if (fI)
        {
            alpha = 0.01f;
        }
        else
        {
            alpha = 1f;
        }
        color = particles.startColor;
    }
}
