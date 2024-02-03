using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myButton : MonoBehaviour
{

    public bool Active { get; set; }
    Color color;

    // Start is called before the first frame update
    void Start()
    {
        if(myPlayerPrefs.GetMasterDifficulty() > 0)
        {
            Active = true;
        }
        color = GetComponent<SpriteRenderer>().color;
    }

    public void Init()
    {
        color = new Color(1f, 0.5f, 0.5f, 1f);
        if (Active)
        {
            color.a = 1f;
            GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            color.a = 0.01f;
            GetComponent<SpriteRenderer>().color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if(Active)
        {
            Active = false;
            color.a = 0.01f;
            GetComponent<SpriteRenderer>().color = color;
            myPlayerPrefs.SetMasterDifficulty(myPlayerPrefs.GetMasterDifficulty() - 1);
        } 
        else
        {
            Active = true;
            color.a = 1f;
            GetComponent<SpriteRenderer>().color = color;
            myPlayerPrefs.SetMasterDifficulty(myPlayerPrefs.GetMasterDifficulty() + 1);
        }
    }
}
