using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FitInFitter : MonoBehaviour
{


    // Start is called before the first frame update
    IEnumerator Start() // This works by letting this fitter scale the container to fit the text for 1 frame, than disabling it forever to let the parent fitter fit it. #WorstScriptEver ＼(＾O＾)／
    {
        yield return 0;
        GetComponent<ContentSizeFitter>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
