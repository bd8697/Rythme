using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinearFade : MonoBehaviour
{

    Image img;
    Color color;
    [SerializeField] float fadeSpeed;
    [SerializeField] bool fadeIn;
    float alpha;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        color = img.GetComponent<Image>().color;
        if (fadeIn)
        {
            alpha = 1f;
        }
        else
        {
            alpha = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(fadeIn)
        {
            alpha -= fadeSpeed;
            if(alpha < 0f)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            alpha += fadeSpeed;
            if (alpha > 1f)
            {
                gameObject.SetActive(false);
            }
        }

        color.a = alpha;
        img.color = color;
    }
}
