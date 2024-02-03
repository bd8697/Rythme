using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] TMP_Text score;
    [SerializeField] Slider volumeSlider;
    [SerializeField] float minScoreSkull;

    // Start is called before the first frame update
    void Start()
    {
        var difficulty = myPlayerPrefs.GetMasterDifficulty();
        volumeSlider.value = myPlayerPrefs.GetMasterVolume();

        score.text = myPlayerPrefs.GetMasterScore().ToString();

        if(myPlayerPrefs.GetMasterScore() > minScoreSkull)
        {
                transform.GetChild(0).gameObject.SetActive(true);
        }
        var button = transform.GetChild(0).GetComponent<myButton>();
        if (difficulty > 0) // need this for multiple difficulty levels
        {
            button.Active = true;
        }
        button.Init();
    }

    // Update is called once per frame
    void Update()
    {
       // FindObjectOfType<myMusic>().SetVolume(volumeSlider.value);
    }

    public void Save(float volume)
    {
        myPlayerPrefs.SetMasterVolume(volume);
        AudioListener.volume = volume;
    }
}
