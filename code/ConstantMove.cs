using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMove : MonoBehaviour
{
    [SerializeField] float moveX = 0f;
    [SerializeField] float moveY = 0f;

    public float MoveX { get => moveX; set { moveX = value; } }
    public float MoveY { get => moveY; set { moveY = value; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moveX != 0)
        {
            transform.position = new Vector3(transform.position.x + moveX * (Time.deltaTime * GameState.DeltaTimeCorrection), transform.position.y, transform.position.z);
        }

        if (moveY != 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + moveY * (Time.deltaTime * GameState.DeltaTimeCorrection), transform.position.z);
        }
    }

    IEnumerator OnTriggerEnter2D(Collider2D collWith)
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
