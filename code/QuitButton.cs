using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    [SerializeField] Color myColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Application.Quit();
    }

    public void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = myColor;
    }
}
