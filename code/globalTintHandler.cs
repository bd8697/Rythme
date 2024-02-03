using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalTintHandler : MonoBehaviour
{
    [Header("Tint")]
    [SerializeField] List<Tint> toTint;
    [SerializeField] AudioSource audioSource;
    Color globalTint;
    float secondsTweenTintTicks;
    public Color GlobalTint { get => globalTint; set { globalTint = value; } }

    // Start is called before the first frame update
    void Start()
    {
        toTint.Add(FindObjectOfType<Tint>());
        Init(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        StopAllCoroutines();
        secondsTweenTintTicks = audioSource.clip.length / 256; // 256 ticks in tint animation
        globalTint = new Color(127, 255, 255, 255);
        StartCoroutine(TintOverTime());
    }

    private IEnumerator TintOverTime()
    {
        if (globalTint.b > 127)
        {
            globalTint.b--;
        }
        else if (globalTint.r < 255)
        {
            globalTint.r++;
        }

        foreach(Tint tint in toTint)
        {
            tint.tintOnTick(globalTint);
        }

        yield return new WaitForSeconds(secondsTweenTintTicks);
        StartCoroutine(TintOverTime());
    }
}
