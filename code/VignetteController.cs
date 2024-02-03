using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VignetteController : MonoBehaviour
{
    [SerializeField] Volume PPVolume;
    Vignette vignette;

    private void Start()
    {
        PPVolume.profile.TryGet(out vignette);
    }
    public IEnumerator Vignette(float duration, float magnitude)
    {
        float timePassed = 0.0f;
        float intensity = 0.1f;
        bool increasing = true;

        while (intensity > 0f)
        {
            if(increasing)
            {
                intensity += magnitude * Time.deltaTime * 10;
            } else
            {
                intensity -= magnitude * Time.deltaTime * 10;
            }

            if(increasing && timePassed > duration / 2)
            {
                increasing = false;
            }

            vignette.intensity.value = intensity;
            timePassed += Time.deltaTime;

            yield return null; // wait until next frame
        }

        intensity = 0f;
    }
}
