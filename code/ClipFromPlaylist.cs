using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ClipFromPlaylist : MonoBehaviour
{
    AudioSource trueAudio;
    AudioSource fakeAudio;
    [SerializeField] globalTintHandler tint;

    // Start is called before the first frame update
    async Task Start()
    {
        trueAudio = GetComponent<AudioSource>();
        fakeAudio = transform.parent.GetComponent<AudioSource>();
        if(GameState.Playlist.Count > 0)
        {
            await ChangeClip();
        }
        fakeAudio.volume = 1f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public async Task ChangeClip()
    {

        string prevName = trueAudio.clip.name;
        List<string> clips = GameState.Playlist;
        int rnd;
        string clipName;
        do
        {
            rnd = Random.Range(0, clips.Count);
            clipName = clips[rnd];
        } while (clipName == prevName);
        string clipToPlay = clipName + ".wav";
        var filePath = Path.Combine(Application.persistentDataPath, "Playlist", clipToPlay);
        try
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.WAV))
            {
                www.SendWebRequest();
                while (!www.isDone)
                {
                    await Task.Delay(5);
                }
                var audioClip = DownloadHandlerAudioClip.GetContent(www);
                fakeAudio.clip = audioClip;
                trueAudio.clip = audioClip;
                trueAudio.clip.name = clipName;

            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
        tint.Init();
        GetComponent<AudioVisualization>().Init();
        trueAudio.Play();
        fakeAudio.Play();
    }
}
