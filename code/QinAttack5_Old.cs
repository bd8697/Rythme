//using Cubequad.Tentacles2D;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Rendering.HighDefinition;


// RIP

//public class QinAttack5_Old : MonoBehaviour
//{
//    Qin qin;
//    QinController qinC;
//    [SerializeField] float push = 0f;
//    [SerializeField] GameObject fireProjectileContainer;
//    [SerializeField] Material fireProjectileMat;
//    [SerializeField] float scaleScale;
//    [SerializeField] float waveCount;
//    [SerializeField] float waveDelay;
//    [SerializeField] float forceMultiplier;
//    [SerializeField] float lightScale;
//    private float originalLightIntensity = 0f;
//    private float originalScale = 0f;
//    Spin qinSpin;
//    private float normalizedSpin;
//    private float spin;
//    private float count;
//    // private bool slowRotation = false;

//    // Start is called before the first frame update
//    void Start()
//    {
//        qin = gameObject.GetComponent<Qin>();
//        qinC = qin.QinController;
//        qinSpin = qin.GetComponent<Spin>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //  if(slowRotation)
//        //    {
//        //        // qinSpin.SudoAngularVelocity /= 1.5f;
//        //        Debug.Log(qinSpin.SudoAngularVelocity);
//        //    }
//        if (fireProjectileContainer.transform.childCount > 0)
//        {
//            int i = 0;
//            foreach (Transform child in fireProjectileContainer.transform)
//            {
//                //if(child.gameObject.transform.localScale.x < originalScale * 10)
//                //{
//                child.gameObject.transform.localScale = new Vector3(child.gameObject.transform.localScale.x * scaleScale, child.gameObject.transform.localScale.y * scaleScale, child.gameObject.transform.localScale.z);
//                //}
//                HDAdditionalLightData light = child.transform.GetChild(0).GetComponent<HDAdditionalLightData>();
//                //if (light.intensity < originalLightIntensity * 50)
//                //{
//                //    light.intensity *= lightScale;
//                //}
//                //child.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
//                child.gameObject.GetComponent<Rigidbody2D>().AddForce(child.gameObject.transform.up.normalized * child.gameObject.transform.localScale.x); //* 250
//                child.gameObject.GetComponent<Rigidbody2D>().AddForce(directionBasedOnSpin(child.gameObject, qinSpin.MaxAngVel, spin) * forceMultiplier); //* 250
//                                                                                                                                                          // child.gameObject.transform.Rotate(0f, 0f, qinSpin.SudoAngularVelocity / 20);
//                child.gameObject.transform.Rotate(0f, 0f, -normalizedSpin);
//                //if (Mathf.Abs(child.gameObject.GetComponent<Rigidbody2D>().angularVelocity) < 30)
//                //{
//                //   // child.gameObject.GetComponent<Rigidbody2D>().AddTorque(qin.GetComponent<Spin>().sudoAngularVelocity / child.gameObject.transform.localScale.x);
//                //}
//                i++;
//            }
//        }
//    }

//    public void Attack5()
//    {
//        //slowRotation = true;
//        count = 1;
//        spin = qinSpin.SudoAngularVelocity;
//        StartCoroutine(Fires());
//    }

//    private IEnumerator Fires()
//    {
//        //yield return new WaitForSeconds(qin.AttackCooldown);
//        //slowRotation = false;
//        foreach (GameObject qinFire in qin.QinFires)
//        {
//            GameObject newFire = Instantiate(qinFire, qinFire.transform.position, qinFire.transform.rotation, fireProjectileContainer.transform);
//            newFire.GetComponent<MeshRenderer>().material = fireProjectileMat;
//            newFire.transform.GetChild(0).gameObject.SetActive(true);
//            newFire.GetComponent<CircleCollider2D>().enabled = true;
//            newFire.transform.localScale = transform.localScale;
//            Rigidbody2D newProjRB = newFire.AddComponent<Rigidbody2D>();
//            newProjRB.gravityScale = 0;
//            newProjRB.AddForce(newFire.transform.up.normalized * push);
//            newProjRB.AddForce(qinC.GetComponent<Rigidbody2D>().velocity * 10);
//            // newProjRB.AddTorque(qin.GetComponent<Spin>().sudoAngularVelocity);
//            HDAdditionalLightData light = newFire.transform.GetChild(0).GetComponent<HDAdditionalLightData>();
//            // light.range *= 2;
//            // originalLightIntensity = light.intensity;
//            originalScale = newFire.transform.localScale.x;
//        }

//        if (count < waveCount)
//        {
//            yield return new WaitForSeconds(waveDelay);
//            count++;
//            StartCoroutine(Fires());
//        }
//        else
//        {
//            yield return new WaitForSeconds(0);
//            count = 1;
//        }
//        // qin.GetComponent<materialAlphaFade>().FadeObject(true, fires, 10f);
//    }

//    private void FaceWhereGoing(GameObject fireProj)
//    {
//        Vector2 moveDirection = fireProj.GetComponent<Rigidbody2D>().velocity;
//        if (moveDirection != Vector2.zero)
//        {
//            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
//            fireProj.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
//        }
//    }

//    private Vector3 directionBasedOnSpin(GameObject fireproj, float maxSpin, float setSpin)
//    {
//        Vector3 dir = fireproj.transform.right.normalized * 1 + fireproj.transform.up.normalized * 0;
//        normalizedSpin = -setSpin / maxSpin;
//        dir = fireproj.transform.right.normalized * normalizedSpin + fireproj.transform.up.normalized * (1 - Mathf.Abs(normalizedSpin));
//        return dir;
//    }
//}



