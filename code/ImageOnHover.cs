using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageOnHover : MonoBehaviour
{
    [SerializeField] Color hoverColor;
    Color initColor;
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        initColor = image.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseEnter()
    {
        image.color = hoverColor;
    }

    void OnMouseExit()
    {
        image.color = initColor;
    }

    void OnMouseDown()
    {
        //todo: imp
    }
}
