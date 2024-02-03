using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaylistHandler : MonoBehaviour
{
    [SerializeField] GameObject playlistElement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulatePlaylist(string elem)
    {
        GameObject newElem = Instantiate(playlistElement, transform.position, transform.rotation, transform);
        newElem.transform.GetChild(0).GetComponent<TMP_Text>().text = elem;
    }
}
