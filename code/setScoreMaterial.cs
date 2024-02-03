using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class setScoreMaterial : MonoBehaviour
{

    [SerializeField] Material mat;
    // Start is called before the first frame update
    void Start()
    {
        var text = GetComponent<TMP_Text>();
        text.fontSharedMaterial = mat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
