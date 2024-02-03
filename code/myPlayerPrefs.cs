using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myPlayerPrefs : MonoBehaviour // myPlayerPrefs
{
    const string MASTER_VOLUME_KEY = "volume";
    const string MASTER_SCORE_KEY = "score";
    const string MASTER_DEATHCOUNT_KEY = "deaths";
    const string MASTER_DIFFICULTY_KEY = "difficulty";

    public static void SetMasterVolume(float volume)
    {
        UnityEngine.PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume); // Unity's Player Prefs
    }

    public static void SetMasterScore(float score)
    {
        UnityEngine.PlayerPrefs.SetFloat(MASTER_SCORE_KEY, score);
    }

    public static void SetMasterDeathCount()
    {
        UnityEngine.PlayerPrefs.SetFloat(MASTER_DEATHCOUNT_KEY, GetMasterDeathCount() + 1);
    }

    public static void ResetMasterDeathCount()
    {
        UnityEngine.PlayerPrefs.SetFloat(MASTER_DEATHCOUNT_KEY, 0);
    }

    public static float GetMasterScore()
    {
        return UnityEngine.PlayerPrefs.GetFloat(MASTER_SCORE_KEY);
    }

    public static float GetMasterVolume()
    {
        return UnityEngine.PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
    }

    public static float GetMasterDeathCount()
    {
        return UnityEngine.PlayerPrefs.GetFloat(MASTER_DEATHCOUNT_KEY);
    }

    public static int GetMasterDifficulty()
    {
        return UnityEngine.PlayerPrefs.GetInt(MASTER_DIFFICULTY_KEY);
    }

    public static void SetMasterDifficulty(int diff)
    {
        UnityEngine.PlayerPrefs.SetInt(MASTER_DIFFICULTY_KEY, diff);
    }

}
