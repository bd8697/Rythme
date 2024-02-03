using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioHandler : MonoBehaviour
{
    [SerializeField] float fadeSpeed;
    AudioSource audio;
    bool fade;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fade)
        {
            audio.volume -= fadeSpeed * Time.deltaTime;

            if (audio.volume < 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Fade()
    {
        fade = true;
    }
}
