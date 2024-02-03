using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

public class DreadAttack3 : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] GameObject tentacleContainer;
    [SerializeField] GameObject dreadSmall;
    [SerializeField] Tentacle tentaclePrefab;
    [SerializeField] float tenWidth = 1f;
    [SerializeField] float tenLength = 10f;
    [SerializeField] float spawnFrom;
    [SerializeField] float cap;

    Dread dread;
    bool lightUp;
    float maxIntensity;

    // Start is called before the first frame update
    void Start()
    {
        dread = gameObject.GetComponent<Dread>();
    }

    // Update is called once per frame
    void Update()
    {
        Cap();
    }
       

    public void Attack3()
    {
        var smallDreadPos = new Vector3(-dread.transform.position.x, spawnFrom, 0f);
        GameObject newDreadSmall = Instantiate(dreadSmall, smallDreadPos, Quaternion.identity) as GameObject;
        newDreadSmall.transform.parent = parent.transform;
        
        Tentacle tentacle = Instantiate(tentaclePrefab, smallDreadPos, Quaternion.identity);
        tentacle.Tip.tag = "tentacleProjectile"; // aka destroyable
        tentacle.Tip.gameObject.layer = LayerMask.NameToLayer("TentacleTipProjectile");
        tentacle.Width = tenWidth;
        tentacle.Length = tenLength;
        tentacle.transform.parent = tentacleContainer.transform;
        tentacle.GetComponent<Tentacle>().ParentRigidbody = newDreadSmall.transform.GetChild(0).GetComponent<Rigidbody2D>();

        VisualEffect newDreadSmallVis = newDreadSmall.GetComponent<VisualEffect>();
        newDreadSmallVis.Play();
        // StartCoroutine(Blink());

        tentacle.Drag = 0f;
        //increase amplitude of wave animation for bigger force. ((eg 2000)
    }

    //private IEnumerator Blink()
    //{
    //    light.gameObject.SetActive(true);
    //    light.intensity = 1f;
    //    lightUp = true;
    //    yield return new WaitForSeconds(blinkSpeed);
    //    lightUp = false;
    //}

    private void Cap()
    {
        if(tentacleContainer.transform.childCount > cap)
        {
            var startIdx = 0;
            var count = tentacleContainer.transform.childCount - cap; // no. of tentacles to destroy
            for(int i = startIdx; i < count; i++)
            {
                tentacleContainer.transform.GetChild(i).GetComponent<Tentacle>().startFade();
            }
        }
    }
}
