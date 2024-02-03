using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{

    [SerializeField] string qinProjectileTag;
    [SerializeField] string qinFireProjectileTag;
    [SerializeField] string dreadTag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collWith)
    {
            if (collWith.gameObject.tag == dreadTag)
            {
                var parent = collWith.gameObject.transform.parent.gameObject;
                Destroy(parent);
            }
            else if (collWith.gameObject.tag == qinProjectileTag)
            {
                Destroy(collWith.gameObject);
            }
            else if (collWith.gameObject.tag == qinFireProjectileTag)
            {
            Destroy(collWith.gameObject);
            }
    }
}
