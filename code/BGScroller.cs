using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.7f;
    Material myMaterial;
    Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        offset = new Vector2(0f, scrollSpeed);
        myMaterial.SetColor("_Color", new Color(127, 255, 255) / 255);
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.mainTextureOffset += offset * Time.deltaTime;
    }
}
