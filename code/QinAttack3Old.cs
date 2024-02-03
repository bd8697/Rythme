using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QinAttack3Old : MonoBehaviour
{
    [SerializeField] Material[] fires;
    [SerializeField] int durationMultiplier;
    [SerializeField] int fadeSpeed;
    Qin qin;

    int queue = 0;

    // Start is called before the first frame update
    void Start()
    {
        qin = gameObject.GetComponent<Qin>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack3()
    {
        StartCoroutine(Fade());
    }

    public IEnumerator Fade()
    {
        foreach (GameObject qinProjectile in qin.QinProjectiles)
        {
            qinProjectile.GetComponent<spriteAlphaFade>().FadeObject(false, fadeSpeed);
        }
        qin.GetComponent<spriteAlphaFade>().FadeObject(false, fadeSpeed);
        qin.GetComponent<materialAlphaFade>().FadeObject(false, fires, fadeSpeed);

        queue++;
        yield return new WaitForSeconds(qin.AttackCooldown * durationMultiplier);
        queue--;

        if (queue == 0)
        {
            foreach (GameObject qinProjectile in qin.QinProjectiles)
            {
                qinProjectile.GetComponent<spriteAlphaFade>().FadeObject(true, fadeSpeed);
            }
            qin.GetComponent<spriteAlphaFade>().FadeObject(true, fadeSpeed);
            qin.GetComponent<materialAlphaFade>().FadeObject(true, fires, fadeSpeed);
        }
    }
}



