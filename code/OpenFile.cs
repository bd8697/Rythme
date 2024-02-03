using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class OpenFile : MonoBehaviour
{
    string path = "";
    string extension;
    AudioClip clip;
    [SerializeField] Color succesColor;
    [SerializeField] Color failColor;
    [SerializeField] ShowMessageOnHover showMessage;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        FileSelect();
    }

    public void FileSelect()
    {
        path = "";
        var extensions = new[] {
            new ExtensionFilter("Sound Files", "mp3", "wav")
        };
        var paths = StandaloneFileBrowser.OpenFilePanel("Adding to Playlist", "", extensions, false);
        if(paths.Length > 0)
            path = paths[0];

        extension = path.Substring(path.LastIndexOf('.') + 1);

        if (extension == "mp3" || extension == "wav")
        {
            showMessage.PinMessage("Processing...", succesColor);
            LoadSong();
        }
        else
        {
            showMessage.StartShowMessage("Works with .mp3 or .wav", 1.5f, failColor);
        }
    }

    private void LoadSong()
    {
        StartCoroutine(LoadSongCoroutine());
    }

     private IEnumerator LoadSongCoroutine()
    {
        yield return new WaitForEndOfFrame();

        string url = string.Format(path);

        if(extension == "wav")
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError(www.error);
                    yield break;
                }
                yield return null;
                clip = DownloadHandlerAudioClip.GetContent(www);
            }
        }
        else if (extension == "mp3")
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
                yield break;
            }
            yield return null;
            clip = NAudioPlayer.FromMp3Data(www.downloadHandler.data);
        }
        string name = Path.GetFileNameWithoutExtension(path);
        SavWav.Save(name, clip); //converts the mp3 to wav (cause Unity is garbage and isn't allowed to work as an mp3 player), then saves the wav in a Playlist folder
        GameState.AddToPlaylist(name);

        showMessage.StartShowMessage("Added to playlist!", 1.5f, succesColor);
    }


}
