using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PauseUnpause : MonoBehaviour
{
    [SerializeField] Volume PPVolume;
    [SerializeField] AudioSource[] audios;
    [SerializeField] GameObject quitBtn;
    Vignette vignette;
    KeyCode myKeyCode;


    // Start is called before the first frame update
    void Start()
    {
        PPVolume.profile.TryGet(out vignette);

        myKeyCode = KeyCode.Escape;
       if(myPlayerPrefs.GetMasterDeathCount() == 1)
        {
            StartCoroutine(SetText("space", 10, KeyCode.Space));
        }
       else if (myPlayerPrefs.GetMasterDeathCount() == 3)
        {
            StartCoroutine(SetText("shift", 10, KeyCode.LeftShift));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(myKeyCode))
        {
            if(Time.timeScale == 0)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }     
    }

    private void Pause()
    {
        quitBtn.SetActive(true);
        Time.timeScale = 0;
        vignette.intensity.value = 0.5f;
        foreach(AudioSource audio in audios)
        {
            audio.Pause();
        }
    }

    private void Unpause()
    {
        quitBtn.GetComponent<QuitButton>().ResetColor();
        quitBtn.SetActive(false);
        Time.timeScale = 1;
        GetComponent<TMP_Text>().text = "";
        myKeyCode = KeyCode.Escape;
        vignette.intensity.value = 0f;
        foreach (AudioSource audio in audios)
        {
            audio.Play();
        }
    }

    private IEnumerator SetText(string text, float wait, KeyCode keycode)
    {
        yield return new WaitForSeconds(wait);
        GetComponent<TMP_Text>().text = text;
        myKeyCode = keycode;
        Pause(); 
    }
}
