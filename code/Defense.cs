using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : MonoBehaviour
{

    [SerializeField] float pushForce;

    public void Defend(Rigidbody2D defender)
    {
        int inputs = 0;
        int degrees = 0;
        defender.transform.rotation = Quaternion.identity;

        if (Input.GetKey(KeyCode.W))
        {
            degrees += 0;
            inputs++;
        }
        if (Input.GetKey(KeyCode.A))
        {
            degrees += 90;
            inputs++;
        }
        if (Input.GetKey(KeyCode.D))
        {
            degrees -= 90;
            inputs++;
        }
        if (Input.GetKey(KeyCode.S) && inputs < 2)
        {
            if (Input.GetKey(KeyCode.D)) // need to change direction whenever negative angle applied
                degrees -= 180;
            else
                degrees += 180;
            inputs++;
        }

        // rb2d.velocity = new Vector2(0, 0);
        if(inputs > 0)
        {
            defender.transform.Rotate(0, 0, degrees / inputs);
            defender.angularVelocity = 0f;
            defender.AddForce(defender.transform.up.normalized * pushForce);
        } 
    }
}
