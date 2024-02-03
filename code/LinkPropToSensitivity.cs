using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LinkPropToSensitivity : MonoBehaviour
{
    [SerializeField] string propName;
    VisualEffect visEff;
    [SerializeField] AudioVisualization audioVis;
    float audioRangeMin;
    float audioRangeMax;
    [SerializeField] float propMin;
    [SerializeField] float propMax;

    // Start is called before the first frame update
    void Start()
    {
        visEff = GetComponent<VisualEffect>();
        audioRangeMax = audioVis.initProgressMulti;
        audioRangeMin = audioVis.endProgressMulti;
    }

    // Update is called once per frame
    void Update()
    {
        visEff.SetFloat(propName, MapSensitivityToProp());
    }

    private float MapSensitivityToProp()
    {
        return ((audioVis.progressMultiplier - audioRangeMin) / (audioRangeMin - audioRangeMax) * (propMin - propMax)) * -1 + propMax;
    }
}
