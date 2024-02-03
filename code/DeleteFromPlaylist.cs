using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class DeleteFromPlaylist : MonoBehaviour
{
    string fileName;

    // Start is called before the first frame update
    void Start()
    {
        fileName = transform.GetChild(0).GetComponent<TMP_Text>().text + ".wav";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Delete()
    {
        FindObjectOfType<ShowMessageOnHover>().StartShowMessage("Removed!", 1.5f, new Color(255, 255, 127, 255) / 255f);
        var filePath = Path.Combine(Application.persistentDataPath, "Playlist", fileName);
        File.Delete(filePath);
        Destroy(gameObject);
        // UnityEditor.AssetDatabase.Refresh();

        transform.parent.transform.parent.transform.parent.gameObject.SetActive(false); // accesed parent is not root
    }
}
