using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetRoundScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TMP_Text>().text = FindObjectOfType<GameState>().Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
