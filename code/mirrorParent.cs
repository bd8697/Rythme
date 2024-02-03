using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirrorParent : MonoBehaviour
{

    GameObject dread;
    Vector2 reversePos;


    void Start()
    {
        dread = gameObject.transform.parent.gameObject;

    }

    public void Mirror()
    {
        reversePos = new Vector2(-dread.transform.position.x, -dread.transform.position.y);
        gameObject.transform.position = reversePos;
    }
}
