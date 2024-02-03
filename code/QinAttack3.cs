using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QinAttack3 : MonoBehaviour
{
    [SerializeField] GameObject qinProj;
    [SerializeField] GameObject projContainer;
    [SerializeField] float spawnFrom;
    [SerializeField] float acceleration;
    [SerializeField] float slowPer;
    [SerializeField] float fadeSpeed;
    Qin qin;

    // Start is called before the first frame update
    void Start()
    {
        qin = gameObject.GetComponent<Qin>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in projContainer.transform)
        {
            GameObject proj = child.gameObject;
            Rigidbody2D rb2d = proj.GetComponent<Rigidbody2D>();

            if (Mathf.Abs(proj.transform.position.y) < Mathf.Abs(slowPer * spawnFrom)) //slow down for last x percentage of distance 
            {
                if (proj.GetComponent<SpriteRenderer>().color.a == 1f) // first tick
                {
                    rb2d.velocity /= 2f;
                    proj.GetComponent<CapsuleCollider2D>().enabled = false;
                    try
                    {
                        proj.GetComponent<spriteAlphaFade>().FadeObject(false, fadeSpeed, delegate { OnProjDestroy(proj); });
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e.ToString());
                    }

                    Destroy(proj.transform.GetChild(0).gameObject); // destroy fire
                }
            }
            rb2d.AddForce(proj.transform.up.normalized * acceleration * Time.deltaTime * GameState.DeltaTimeCorrection);
        }
    }

    public void Attack3()
    { 
        var pos = new Vector3(-qin.transform.position.x, -spawnFrom, 1f);
        GameObject newProj = Instantiate(qinProj, pos, Quaternion.Euler(0f, 0f, 0f), projContainer.transform);
        newProj.AddComponent<Rigidbody2D>().gravityScale = 0;
        //newProj.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true); // enable light
        StartCoroutine(EnableHitbox(newProj));
        var renderer = newProj.GetComponent<SpriteRenderer>();
        var color = renderer.color;
        color.a = 1f;
        renderer.color = color;

        pos = new Vector3(-qin.transform.position.x, spawnFrom, 1f);
        newProj = Instantiate(qinProj, pos, Quaternion.Euler(0f, 0f, -180f), projContainer.transform);
        newProj.AddComponent<Rigidbody2D>().gravityScale = 0;
        //newProj.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(EnableHitbox(newProj));
        renderer = newProj.GetComponent<SpriteRenderer>();
        color = renderer.color;
        color.a = 1f;
        renderer.color = color;
    }

    public void OnProjDestroy(GameObject GO) //called as a delegate after fadeOut
    {
        Destroy(GO);
    }

    IEnumerator EnableHitbox(GameObject proj)
    {
        yield return new WaitForSeconds(qin.AttackCooldown * 4);
        proj.GetComponent<CapsuleCollider2D>().enabled = true;
    }
}



