using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpStars : MonoBehaviour
{

    [SerializeField] float minLifetime;
    [SerializeField] float maxLifetime;
    // Start is called before the first frame update
    void Start()
    {
        var rnd = Random.Range(minLifetime, maxLifetime);
        var target = -transform.position.y;
        var main = gameObject.GetComponent<ParticleSystem>().main;
        main.startLifetime = rnd;
        main.startSpeed = rnd;
        main.startSize = rnd;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
