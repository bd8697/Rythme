using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextOnHover : MonoBehaviour
{
    [SerializeField] Color hoverColor;
    Color initColor;
    Material textMat;

    // Start is called before the first frame update
    void Start()
    {
        textMat = GetComponent<TMP_Text>().materialForRendering;
        initColor = textMat.GetColor("_FaceColor");
        hoverColor.a = initColor.a;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseEnter()
    {
        textMat.SetColor("_FaceColor", hoverColor);
        textMat.SetColor("_GlowColor", hoverColor);
    }

    void OnMouseExit()
    {
        textMat.SetColor("_FaceColor", initColor);
        textMat.SetColor("_GlowColor", initColor);
    }

    void OnMouseDown()
    {
        //todo: imp
    }
}
