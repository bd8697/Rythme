using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueFade : MonoBehaviour
{
    [SerializeField] float val;
    [SerializeField] float multi;
    [SerializeField] float fadeSpeed;
    public float Val { get => val; set { val = value; } }
    float startVal;

    bool fadeIn = false;
    bool fading = false;

    float myDeltaTime;
    const float fps = 60;

    public delegate void DoAtEnd();
    public DoAtEnd endDel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            myDeltaTime = Time.deltaTime * fps;
            if (fadeIn)
            {
                Val += myDeltaTime * fadeSpeed; // % increase each frame
                if (Val > startVal * multi)
                {
                    Val = startVal * multi;
                    fading = false;
                    FadeValue(false, endDel);
                }
            }
            else if (!fadeIn)
            {
                Val -= myDeltaTime * fadeSpeed;
                if (Val < startVal / multi)
                {
                    Val = startVal / multi;
                    fading = false;
                    endDel?.Invoke();
                }
            }
        }
    }

    public void FadeValue(bool fI, DoAtEnd doAtEnd)
    {
        if (!fading) // ignore if already triggered
        {
            fading = true;
            fadeIn = fI;

            startVal = Val;
            endDel = doAtEnd;
        }
    }
}
