using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePitch : MonoBehaviour
{
    [SerializeField] float decaySpeed;
    [SerializeField] bool pitchDown;
    [SerializeField] bool pitchUp;
    [SerializeField] AudioSource fakeAudio;
    AudioSource trueAudio;

    public bool PitchDown { get => pitchDown; set => pitchDown = value; }
    public bool PitchUp { get => pitchUp; set => pitchUp = value; }

    // Start is called before the first frame update
    void Start()
    {
        trueAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pitchUp)
        {
            trueAudio.pitch += decaySpeed * Time.deltaTime * GameState.DeltaTimeCorrection;
            fakeAudio.pitch += decaySpeed * Time.deltaTime * GameState.DeltaTimeCorrection;
            if (trueAudio.pitch > 1f)
            {
                trueAudio.pitch = 1f;
                fakeAudio.pitch = 1f;
                pitchUp = false;
            }
        }
        else if (pitchDown)
        {
            trueAudio.pitch -= decaySpeed * Time.deltaTime * GameState.DeltaTimeCorrection;
            fakeAudio.pitch -= decaySpeed * Time.deltaTime * GameState.DeltaTimeCorrection;
            if (trueAudio.pitch < 0f)
            {
                trueAudio.pitch = 0f;
                fakeAudio.pitch = 0f;
                pitchDown = false;
            }
        }
    }
}
