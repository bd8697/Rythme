using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameState : MonoBehaviour
{
    float score;
    int lastSceneIdx;
    int enemyCount;
    int difficulty;
    static List<string> playlist;
    static PlaylistHandler handler;
    static float deltaTimeCorrection = 60f;

    public float Score { get => score; set { score = value; } }
    public int LastSceneIdx { get => lastSceneIdx; set { lastSceneIdx = value; } }
    public int EnemyCount { get => enemyCount; set { enemyCount = value; } }
    public int Difficulty { get => difficulty; set { difficulty = value; } }
    public static List<string> Playlist { get => playlist; set => playlist = value; }
    public static float DeltaTimeCorrection { get => deltaTimeCorrection; }

    //public AudioSource[] Playlist { get => playlist; set => playlist = value; }



    // Start is called before the first frame update
    void Start()
    {
        handler = FindObjectOfType<PlaylistHandler>();
        enemyCount = 1;
        difficulty = myPlayerPrefs.GetMasterDifficulty();
        score = 0;
        lastSceneIdx = 0;
        playlist = new List<string>();
        GetPlaylist();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void AddToPlaylist(string name)
    {
        Playlist.Add(name);
        handler.PopulatePlaylist(name);
    }

    public static void RemoveFromPlaylist(string name)
    {
        Playlist.Remove(name);
    }

    private void GetPlaylist()
    {
        var filePath = Path.Combine(Application.persistentDataPath, "Playlist");
        DirectoryInfo dir = new DirectoryInfo(filePath);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            AddToPlaylist(Path.GetFileNameWithoutExtension(f.ToString()));
        }
        handler.transform.parent.transform.parent.gameObject.SetActive(false);
    }
}
