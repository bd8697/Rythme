using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class QinController : MonoBehaviour
{
    private bool auto;
    [SerializeField] public Qin qin;
    [SerializeField] GameObject DreadHomingProjectileContainer;
    [SerializeField] VisualEffect smokeOnCollision;

    public bool Auto { get => auto; set { auto = value; } }

    // Start is called before the first frame update
    void Start()
    {
        auto = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collWith)
    {
        qin.ResetTrail();
        switch (collWith.gameObject.tag)
        {
            case "top":
            case "bot":
                {
                    CancelHomingOnTeleport();
                    transform.position -= new Vector3(0, collWith.gameObject.transform.position.y * 1.5f, 0);
                    if(!Auto)
                    {
                        qin.Target.GetComponent<Dread>().AttackDash();
                    }
                    break;
                }

            case "left":
            case "right":
                {
                    CancelHomingOnTeleport();
                    transform.position -= new Vector3(collWith.gameObject.transform.position.x * 1.7f, 0, 0);
                    if (!Auto)
                    {
                        qin.Target.GetComponent<Dread>().AttackDash();
                    }
                    break;
                }

            case "tentacleTip":
            case "tentacleProjectile":
                {
                    if(!qin.isAuto && !qin.isInvincible)
                    {
                        GetComponent<AudioSource>().Play();
                        smokeOnCollision.transform.position = collWith.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(transform.position);
                        
                        Vector3 target = transform.position;
                        Vector3 objectPos = collWith.transform.position;
                        target.x = target.x - objectPos.x;
                        target.y = target.y - objectPos.y;
                        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
                        smokeOnCollision.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
                        smokeOnCollision.Play();

                        GetComponent<Move>().SlowFactor *= 2;
                        GetComponent<CircleCollider2D>().enabled = false;
                        qin.GameOver();
                    }
                    break;
                }
        }
    }

    private void CancelHomingOnTeleport()
    {
        foreach (Transform proj in DreadHomingProjectileContainer.transform)
        {
           Tentacle ten = proj.gameObject.GetComponent<Tentacle>();

            if (ten.IsTargetSet)
            {
                ten.StoredDirection = ((Vector2)ten.TargetTransform.position - ten.Tip.position).normalized;
                ten.TargetTransform = null;
                ten.TargetRigidbody = null;
            }
        }
    }
}
