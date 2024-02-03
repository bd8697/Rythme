using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantSpin : MonoBehaviour
{

    [SerializeField] float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
        transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + rotSpeed * (Time.deltaTime * GameState.DeltaTimeCorrection));
    }
}
