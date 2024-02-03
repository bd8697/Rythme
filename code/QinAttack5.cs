using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;


public class QinAttack5 : MonoBehaviour
{
    Qin qin;
    [SerializeField] Material[] fireProjectileMats;
    [SerializeField] Color color;
    [SerializeField] Material[] fires;
    [SerializeField] [Range(0f, 1f)] float warmUpFactor;
    [SerializeField] float scaleMultiplier;
    [SerializeField] float scaleMultiplierMultiplier;

    List<GameObject> atk5qinFires = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        qin = gameObject.GetComponent<Qin>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (qin.ProjectileFireContainer.transform.childCount != 0)
        {
            foreach (Transform child in qin.ProjectileFireContainer.transform)
            {
                GameObject fireProjectile = child.gameObject;

                if (fireProjectile.transform.localScale.x < scaleMultiplier)
                {
                    fireProjectile.transform.localScale *= (1 + warmUpFactor);
                    var pos = fireProjectile.transform.localPosition;
                    if (pos.y != 0)
                    {
                        pos.y *= (1 + warmUpFactor / scaleMultiplierMultiplier);
                    } else if (pos.x != 0)
                    {
                        pos.x *= (1 + warmUpFactor / scaleMultiplierMultiplier);
                    }
                    fireProjectile.transform.localPosition = pos;

                    if(fireProjectile.transform.localScale.x > scaleMultiplier / scaleMultiplierMultiplier)
                    {
                        var material = fireProjectile.GetComponent<MeshRenderer>().material;
                        var newAlpha = material.GetFloat("_GlobalAlpha");
                        newAlpha /= 1 + warmUpFactor * scaleMultiplierMultiplier * 2f;
                        material.SetFloat("_GlobalAlpha", Mathf.Max(0, newAlpha));

                        CircleCollider2D collider = fireProjectile.GetComponent<CircleCollider2D>();
                        if (collider.enabled)
                            collider.enabled = false;
                    }
                }
                else
                {
                    Destroy(fireProjectile);
                }
            }
        }
    }

    public void Attack5()
    {
        var rnd = Random.Range(0, 2);
       if (rnd == 0)
        {
            atk5qinFires.Add(qin.QinFires[0]);
            atk5qinFires.Add(qin.QinFires[1]);
        } else
        {
            atk5qinFires.Add(qin.QinFires[2]);
            atk5qinFires.Add(qin.QinFires[3]);
        }
        foreach(var fire in atk5qinFires)
        {
            GameObject newFire = Instantiate(fire, qin.ProjectileFireContainer.transform);
            newFire.transform.localPosition = fire.transform.localPosition;
            newFire.transform.localRotation = fire.transform.localRotation;
            newFire.transform.localScale = fire.transform.localScale;
            newFire.GetComponent<MeshRenderer>().material = fireProjectileMats[rnd];

            newFire.GetComponent<CircleCollider2D>().enabled = true;
        }
            atk5qinFires.Clear();
        }
}
