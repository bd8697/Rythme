using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPullToTentacles : MonoBehaviour
{
    [SerializeField] float pullUp = 0f;
    [SerializeField] float pullRight = 0f;
    [SerializeField] float projectileForce = 0f;
    [SerializeField] float homingProjectileForce = 0f;
    [SerializeField] float homingLimit = 5f;
    [SerializeField] Dread dread;
    [SerializeField] tentacleContainer tentacleContainer;
    [SerializeField] GameObject projectileContainer;
    [SerializeField] GameObject containerAttack3;
    [SerializeField] GameObject homingProjectileContainer;

    public void AddForceToTentacles()
    {
        float dreadRadius = dread.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        foreach (Tentacle tentacle in tentacleContainer.Tentacles)
        {
                tentacle.Segments[0].AddForce(dread.transform.up.normalized * pullUp *
                    (dreadRadius * 2 + tentacle.CoordsInDread.y) * Time.deltaTime * GameState.DeltaTimeCorrection);
                tentacle.Segments[1].AddForce(dread.transform.right.normalized * pullRight *
                    tentacle.CoordsInDread.x * Time.deltaTime * GameState.DeltaTimeCorrection); // * Mathf.Sign(tentacle.CoordsInDread.y) to disable inverse pull on negative y
                //tentacle.Tip.AddForce(dread.transform.right.normalized * -pullRight *
                //    tentacle.CoordsInDread.x * Time.deltaTime * deltaTimeCorection);

        }

        //foreach (Transform child in containerAttack3.transform)
        //{
        //    Tentacle ten = child.gameObject.GetComponent<Tentacle>();
        //    ten.Pivot.AddForce((ten.Pivot.position - (Vector2)dread.transform.position).normalized * -1 * pullUp * Time.deltaTime);
        //    Debug.Log((ten.Pivot.position - (Vector2)dread.transform.position).normalized * -1 * pullUp * Time.deltaTime);
        //}

        foreach (Transform child in projectileContainer.transform)
        {
            Tentacle ten = child.gameObject.GetComponent<Tentacle>();
            ten.Tip.AddForce(ten.Pivot.transform.up.normalized * projectileForce * Time.deltaTime * GameState.DeltaTimeCorrection);
            // ten.Pivot.AddForce(ten.Pivot.transform.up.normalized * projectileForce * Time.deltaTime * 100);
        }
        
        foreach (Transform child in homingProjectileContainer.transform)
        {
            Tentacle ten = child.gameObject.GetComponent<Tentacle>();

            if(ten.IsTargetSet)
            {
                if (Vector2.Distance(ten.TargetTransform.position, ten.Tip.position) > homingLimit)
                {
                    ten.Tip.AddForce(((Vector2)ten.TargetTransform.position - ten.Tip.position).normalized * Time.deltaTime * GameState.DeltaTimeCorrection * homingProjectileForce);
                    // ten.Pivot.AddForce(((Vector2)ten.TargetTransform.position - ten.Tip.position).normalized * Time.deltaTime * 100 * homingProjectileForce);
                }
                else
                {
                    ten.StoredDirection = ((Vector2)ten.TargetTransform.position - ten.Tip.position).normalized;
                    ten.TargetTransform = null;
                    ten.TargetRigidbody = null;
                }
            } 
            else
            {
                ten.Tip.AddForce(ten.StoredDirection * Time.deltaTime * GameState.DeltaTimeCorrection * homingProjectileForce);
                // ten.Pivot.AddForce(ten.StoredDirection * Time.deltaTime * 100 * homingProjectileForce);
            }
        }

        //foreach (Tentacle tentacle in tentacleContainer.Tentacles)
        //{
        //    Debug.Log(tentacle.CoordsInDread.x);
        //}

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
