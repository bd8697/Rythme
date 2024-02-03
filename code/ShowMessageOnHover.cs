using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowMessageOnHover : MonoBehaviour
{
    [SerializeField] public TMP_Text message;
    [SerializeField] Color neutralColor;
    [SerializeField] string messageText;

    static bool showing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        if(!showing)
        {
            message.text = messageText;
            message.materialForRendering.SetColor("_FaceColor", neutralColor);
            message.materialForRendering.SetColor("_GlowColor", neutralColor);
            Color c = message.color;
            c.a = 1f;
            message.color = c;
        }
    }

    void OnMouseExit()
    {
        if(!showing)
        {
            Color c = message.color;
            c.a = 0f;
            message.color = c;
        }
    }

    public void StartShowMessage(string msg, float lifetime, Color color) // so the coroutine starts here and we can delete the triggerer
    {
        StartCoroutine(ShowMessage(msg, lifetime, color));
    }

    private IEnumerator ShowMessage(string msg, float lifetime, Color color)
    {
        showing = true;
        message.text = msg;
        message.materialForRendering.SetColor("_FaceColor", color);
        message.materialForRendering.SetColor("_GlowColor", color);

        message.GetComponent<textAlphaFade>().FadeObject(true, 10f);
        yield return new WaitForSeconds(lifetime);
        showing = false;
        message.GetComponent<textAlphaFade>().FadeObject(false, 10f);
    }

    public void PinMessage(string msg, Color color)
    {
        showing = true;
        message.text = msg;
        message.materialForRendering.SetColor("_FaceColor", color);
        message.materialForRendering.SetColor("_GlowColor", color);
    }
}
