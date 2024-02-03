using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DreadAttack2 : MonoBehaviour
{
    Dread dread;
    [SerializeField] float pullForce = 5000f;
    [SerializeField] float dreadPushForce = 5000f;
    [SerializeField] float dragMultiplier;
    [SerializeField] VisualEffect dreadSmokeDash;
    [SerializeField] GameObject targetObject;

    float oldDrag;
    float newDrag;

    // Start is called before the first frame update
    void Start()
    {
        dread = gameObject.GetComponent<Dread>();
        dread.AutoMove.isDashing = false;
        oldDrag = dread.Tentacles.Tentacles[0].Drag;
        newDrag = oldDrag * dragMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        if (dread.AutoMove.isDashing == true)
        {
            foreach (Tentacle tentacle in dread.tentacles.Tentacles)
            {
                //tentacle.Segments[1].AddForce(transform.up.normalized * pullForce / 10 * Mathf.Max(0f, Mathf.Sign(tentacle.CoordsInDread.y)));
                tentacle.Segments[1].AddForce(transform.right.normalized * pullForce * tentacle.CoordsInDread.x);
            }
        }
    }

    public void Attack2()
    {
        //foreach (Tentacle tentacle in dread.tentacles.Tentacles)
        //{
        //    tentacle.Segments[1].AddForce(transform.up.normalized * pullForce * Mathf.Max(0f, Mathf.Sign(tentacle.CoordsInDread.y)));
        //    tentacle.Segments[1].AddForce(transform.right.normalized * pullForce * tentacle.CoordsInDread.x);
        //    //tentacle.Segments[1].AddForce(transform.right * pullForce);
        //    tentacle.Length *= 2f;
        //}
        StartCoroutine(Dash(dreadPushForce));
    }

    private IEnumerator Dash(float force)
    {
        GetComponent<Rigidbody2D>().velocity /= 3f;
        yield return new WaitForSeconds(dread.tentacles.Tentacles[0].AttackCooldown * 2);
        //foreach (Tentacle ten in dread.Tentacles.Tentacles)
        //{
        //    ten.Drag = newDrag;
        //}
        dreadSmokeDash.Play();
        Vector3 target = targetObject.transform.position;
        Vector3 objectPos = transform.position;
        target.x = target.x - objectPos.x;
        target.y = target.y - objectPos.y;
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        GetComponent<Rigidbody2D>().AddForce((targetObject.transform.position - transform.position) * force);
        yield return new WaitForSeconds(dread.tentacles.Tentacles[0].AttackCooldown * 2);
        //foreach (Tentacle ten in dread.Tentacles.Tentacles)
        //{
        //    ten.Drag = oldDrag;
        //}
        // setting it like this is different from setting it in the editor; each part needs to have a different drag. Mimic how it's done in TentacleEditor.cs
        dread.AutoMove.isDashing = false;
    }

}
