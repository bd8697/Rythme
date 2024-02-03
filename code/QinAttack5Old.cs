using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;


public class QinAttack5Old : MonoBehaviour
{
    Qin qin;
    [SerializeField] Material fireProjectileMat;
    [SerializeField] Color color;
    [SerializeField] Material[] fires;
    [SerializeField] int startMulti;
    [SerializeField] float attackSpeedInSeconds;
    [SerializeField][Range(1f,2f)] float warmUpFactor;

    float startPos;
    float startScale;
    float decay;
    float scaleDecay;
    float qinLightIntensity;

    int queuedAttacks = 0;
    List<GameObject> atk5qinFires = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        qin = gameObject.GetComponent<Qin>();

        startPos = Mathf.Max(Mathf.Abs(qin.QinFires[0].transform.localPosition.x), Mathf.Abs(qin.QinFires[0].transform.localPosition.y));
        startScale = qin.QinFires[0].transform.localScale.x;
        decay = startPos / (attackSpeedInSeconds * 60); // get distance to travel as decay per frame
        scaleDecay = (decay * (startScale / startPos)) / 3; // /2 to get to 0
        qinLightIntensity = qin.QinFires[0].transform.GetChild(0).GetComponent<HDAdditionalLightData>().intensity;

        atk5qinFires.Add(qin.QinFires[0]);
        atk5qinFires.Add(qin.QinFires[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if (qin.ProjectileFireContainer.transform.childCount != 0)
        {
            foreach (Transform child in qin.ProjectileFireContainer.transform)
            {
                GameObject fireProjectile = child.gameObject;
                HDAdditionalLightData light = fireProjectile.transform.GetChild(0).GetComponent<HDAdditionalLightData>();

                if(light.intensity < qinLightIntensity)
                {
                    light.intensity *= warmUpFactor * Mathf.Max(1f, Time.deltaTime * 60); // careful when multiplying by deltaTime
                }
                else if(light.intensity > qinLightIntensity) //onAttack trigger
                {
                    light.intensity = qinLightIntensity;
                    queuedAttacks--;
                    if (queuedAttacks == 0)
                    {
                        fires[0].SetVector("_Color", qin.FireColor * 1f);
                    }
                    transform.GetComponent<Spin>().MaxAngVel = 0.25f;
                } 
                else
                {
                    var pos = fireProjectile.transform.localPosition;
                    if (pos.x != 0)
                    {
                        pos.x -= Mathf.Sign(pos.x) * decay * fireProjectile.GetComponent<FireProjectileVars>().StartMulti * Time.deltaTime * 60;
                    }
                    else if (pos.y != 0)
                    {
                        pos.y -= Mathf.Sign(pos.y) * decay * fireProjectile.GetComponent<FireProjectileVars>().StartMulti * Time.deltaTime * 60;
                    }
                    fireProjectile.transform.localPosition = pos;

                    var scale = fireProjectile.transform.localScale;
                    scale.x -= scaleDecay * fireProjectile.GetComponent<FireProjectileVars>().StartMulti * Time.deltaTime * 60;
                    scale.y -= scaleDecay * fireProjectile.GetComponent<FireProjectileVars>().StartMulti * Time.deltaTime * 60;
                    fireProjectile.transform.localScale = scale;

                    if (Mathf.Abs(fireProjectile.transform.localPosition.x) < startPos && Mathf.Abs(fireProjectile.transform.localPosition.y) < startPos)
                    {
                        DestroyImmediate(fireProjectile);
                        if(qin.ProjectileFireContainer.transform.childCount == 0)
                        {
                            transform.GetComponent<Spin>().MaxAngVel = 4f;
                        }
                    }
                }
            }
        }
    }

    public void Attack5()
    {
        //List<GameObject> qinFires = new List<GameObject>();
        //if (Random.Range(0, 2) == 0)
        //{
        //    qinFires.Add(qin.QinFires[0]);
        //    qinFires.Add(qin.QinFires[1]);
        //    fires[0].SetVector("_Color", color * 5f);
        //}
        //else
        //{
        //    qinFires.Add(qin.QinFires[2]);
        //    qinFires.Add(qin.QinFires[3]);
        //    fires[1].SetVector("_Color", color * 5f);
        //}

        foreach (GameObject qinFire in atk5qinFires)
        {
            GameObject newFire = Instantiate(qinFire, qin.ProjectileFireContainer.transform);
            newFire.transform.localPosition = qinFire.transform.localPosition;
            newFire.transform.localRotation = qinFire.transform.localRotation;
            newFire.transform.localScale = qinFire.transform.localScale;
            newFire.GetComponent<MeshRenderer>().material = fireProjectileMat;
            newFire.AddComponent<FireProjectileVars>();
            newFire.GetComponent<FireProjectileVars>().StartMulti = startMulti;

            var pos = newFire.transform.localPosition;
            if (pos.x != 0)
            {
                pos.x *= startMulti;
            }
            else if (pos.y != 0)
            {
                pos.y *= startMulti;
            }
            newFire.transform.localPosition = pos;

            var scale = newFire.transform.localScale;
            scale.x *= startMulti / 2f;
            scale.y *= startMulti / 2f;
            newFire.transform.localScale = scale;

            newFire.transform.GetChild(0).gameObject.SetActive(true);
            HDAdditionalLightData light = newFire.transform.GetChild(0).GetComponent<HDAdditionalLightData>();
            light.intensity = 1f;

            newFire.GetComponent<CircleCollider2D>().enabled = true;


            fires[0].SetVector("_Color", color * 5f);
            queuedAttacks++;
        }
    }
}



