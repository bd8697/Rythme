using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEffect : MonoBehaviour
{
    private PowerUp eff;

    // Start is called before the first frame update
    void Start()
    {
        eff = transform.parent.GetComponent<PowerUp>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collWith)
    {
        if (collWith.GetComponent<Dread>())
        {
            if (collWith.GetComponent<Dread>().isAuto)
            {
                eff.SpeedUp(collWith);
            }
            else
            {
                eff.SlowDown(collWith);
            }
        }
        else if (collWith.GetComponent<QinController>())
        {
            if (collWith.GetComponent<QinController>().Auto)
            {
                eff.SpeedUp(collWith);
            }
            else
            {
                eff.SlowDown(collWith);
            }
        }
    }

}
