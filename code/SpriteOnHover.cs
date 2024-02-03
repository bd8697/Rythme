using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpriteOnHover : MonoBehaviour
{
    [SerializeField] Color hoverColor;
    Color initColor;
    SpriteRenderer renderer;


    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        initColor = renderer.color;
        hoverColor.a = initColor.a;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        initColor = renderer.color;
        renderer.color = hoverColor;
    }

    void OnMouseExit()
    {
        renderer.color = initColor;
    }

    void OnMouseDown()
    {
       //todo: imp
    }
}
