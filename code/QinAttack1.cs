using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QinAttack1 : MonoBehaviour
{
    Qin qin;
    [SerializeField] float force = 0f;
    [SerializeField] GameObject projectileContainer;
    [SerializeField] float fadeSpeed;


    // Start is called before the first frame update
    void Start()
    {
        qin = gameObject.GetComponent<Qin>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack1()
    {
        foreach(GameObject qinProjectile in qin.QinProjectiles)
        {
            GameObject newProj = Instantiate(qinProjectile, qinProjectile.transform.position, qinProjectile.transform.rotation, projectileContainer.transform);
            newProj.GetComponent<CapsuleCollider2D>().enabled = true;
            newProj.transform.localScale = transform.parent.transform.localScale;
            // newProj.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true); // enable light;
            Rigidbody2D newProjRB = newProj.AddComponent<Rigidbody2D>();
            newProjRB.gravityScale = 0;
            newProjRB.AddForce(newProj.transform.up.normalized * force);
            Color newPorjColor = newProj.GetComponent<SpriteRenderer>().color;
            newPorjColor.a = 1f;
            newProj.GetComponent<SpriteRenderer>().color = newPorjColor;
            // newProj.AddComponent<autoMove>();

            qinProjectile.GetComponent<spriteAlphaFade>().FadeObject(true, fadeSpeed); //todo ???
        }
    }
}


