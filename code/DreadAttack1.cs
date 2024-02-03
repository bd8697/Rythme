using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreadAttack1 : MonoBehaviour
{
    Dread dread;
    [SerializeField] float amplitudeMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        dread = gameObject.GetComponent<Dread>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack1()
    {
        string side = dread.targetOnSide();
        List<Tentacle> parent = null;

        if (side == "left")
        {
            parent = dread.tentacles.TopLeft;
        }
        else if (side == "right")
        {
            parent = dread.tentacles.TopRight;
        }

        int rndIdx = Random.Range(0, parent.Count);
        int idx = rndIdx;
        Tentacle ten;
        do
        {
            ten = parent[idx];
            idx++;
            if (idx == parent.Count)
            {
                idx = 0;
            }
            if (idx == rndIdx)
            {
                idx = -1;
                break;
            }
        } while (ten.IsAttacking == true || ten.TargetRigidbody != null);

        if(idx != -1)
        {
            StartCoroutine(ReachTargetThenRetract(ten));
        }
    }

    private IEnumerator ReachTargetThenRetract(Tentacle ten)
    {
        ten.TargetRigidbody = dread.Target.GetComponent<Rigidbody2D>();
        ten.Amplitude *= amplitudeMultiplier;

        yield return new WaitForSeconds(ten.AttackCooldown * 3);
        ten.IsAttacking = true;
        yield return new WaitForSeconds(ten.AttackCooldown);
        ten.Amplitude /= amplitudeMultiplier;
        ten.TargetRigidbody = null;
        // ten.IsAttacking = false; // handled in Tentacle.cs (ReachTarget())
    }
}
