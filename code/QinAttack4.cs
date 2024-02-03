using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QinAttack4 : MonoBehaviour
{
    [SerializeField] GameObject fireContainer;
    [SerializeField] Material[] firesEffects;
    [SerializeField] Material[] allEffects;
    [SerializeField] int fadeSpeed;
    [SerializeField] int durationMultiplier;
    Qin qin;
    QinController qinC;

    public bool isAttacking { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        qin = gameObject.GetComponent<Qin>();
        qinC = qin.QinController;
        foreach(Material mat in allEffects) // in case game stopped during fading
        {
            if (mat.HasProperty("_GlobalAlpha"))
            {
                mat.SetFloat("_GlobalAlpha", 1f); // fades fires
            }
            else if (mat.HasProperty("_Color"))
            {
                Color color = mat.color;
                color.a = 1f;
                mat.SetColor("_Color", color); // fades trail
            }
        }
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(fireContainer.transform.childCount > 0)
        {
            for(int i = 0; i < qin.QinFires.Count; i++)
            {
                fireContainer.transform.GetChild(i).gameObject.transform.position = qin.QinFires[i].transform.position * -1;
                fireContainer.transform.GetChild(i).gameObject.transform.rotation = qin.QinFires[i].transform.rotation * new Quaternion(0f, 0f, 180f, 0f);
            }
        }
    }

    public void Attack4()
    {
        if(!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(Teleport());
        }
    }

    public IEnumerator Teleport()
    {
        qin.GetComponent<materialAlphaFade>().FadeObject(true, firesEffects, fadeSpeed);
        foreach (GameObject qinFire in qin.QinFires)
        {
            GameObject newFire = Instantiate(qinFire, qinFire.transform.position * -1, qinFire.transform.rotation, fireContainer.transform);
            newFire.transform.localScale = qinFire.transform.localScale;
            newFire.transform.GetChild(0).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(qin.AttackCooldown * durationMultiplier);

            qin.Particles.Clear();
            qinC.transform.position = new Vector3(qinC.transform.position.x * -1, qinC.transform.position.y * -1, qinC.transform.position.z);
            qinC.transform.rotation *= new Quaternion(0f, 0f, -1f, 0f);
            foreach (Transform child in fireContainer.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (GameObject qinProjectile in qin.QinProjectiles)
            {
                qinProjectile.GetComponent<spriteAlphaFade>().FadeObject(true, fadeSpeed);
            }
            qin.GetComponent<spriteAlphaFade>().FadeObject(true, fadeSpeed);
            qin.GetComponent<materialAlphaFade>().FadeObject(true, allEffects, fadeSpeed);

        yield return new WaitForSeconds(qin.AttackCooldown);
        isAttacking = false;
    }
}
