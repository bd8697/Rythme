using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QinAttack2 : MonoBehaviour
{
    QinController qinC; 
    Qin qin;
    [SerializeField] float qinPushForce = 5000f;

    // Start is called before the first frame update
    void Start()
    {
        qin = gameObject.GetComponent<Qin>();
        qinC = qin.QinController;
        foreach (Material fire in qin.Fires)
        {
            fire.SetVector("_Color", qin.FireColor * 1f);
            fire.SetFloat("_GlobalAlpha", 1f);
        }

        qin.AutoMove.isDashing = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack2()
    {
        StartCoroutine(WarmUp());
    }

    private IEnumerator WarmUp()
    {
        qinC.GetComponent<Rigidbody2D>().velocity /= 2;
        qinC.GetComponent<Rigidbody2D>().angularVelocity /= 2;
        foreach (Material fire in qin.Fires)
        {
            fire.SetVector("_Color", qin.DashColor * 5f);
        }
        // fire.SetVector("_Color", new Color(0.6901f, 0.2352f, 0.0313f, 0f) * 1f);
        yield return new WaitForSeconds(qin.AttackCooldown * 2);
        Dash();
        yield return new WaitForSeconds(qin.AttackCooldown * 2);
        qin.AutoMove.isDashing = false;
    }

    private void Dash()
    {
        foreach (Material fire in qin.Fires)
        {
            fire.SetVector("_Color", qin.FireColor * 1f);
        }
        Vector3 target = qin.Target.transform.position;
        Vector3 objectPos = qinC.transform.position;
        target.x = target.x - objectPos.x;
        target.y = target.y - objectPos.y;
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        qinC.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        qinC.GetComponent<Rigidbody2D>().AddForce((qin.Target.transform.position - qinC.transform.position) * qinPushForce);
    }

}

