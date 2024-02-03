using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DreadAttack5 : MonoBehaviour
{

    [SerializeField] tentacleContainer container;
    [SerializeField] GameObject homingProjectileContainer;
    //[Range(0, 100)][SerializeField] float tenLength;
    //[Range(0, 10)] [SerializeField] float tenWidth;
    [SerializeField] Material projectileMat;
    [SerializeField] VisualEffect smoke;
    [SerializeField] [Range(0f, 1f)] float killDrag;
    [SerializeField] float cap;
    float tenCount;

    // Start is called before the first frame update
    void Start()
    {
        tenCount = container.Tentacles.Count;
    }

    // Update is called once per frame
    void Update()
    {
        Cap();
    }

    public void Attack5()
    {
        float rndIdx;
        do
        {
            rndIdx = Random.Range(0f, tenCount - 1);
        } while (container.Tentacles[(int)rndIdx].IsAttacking == true);
        Tentacle ten = container.Tentacles[(int)rndIdx];
        Tentacle projTen = Instantiate(ten, ten.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        projTen.material = projectileMat;
        projTen.meshRenderer.material = projTen.material;
        projTen.Pivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, 360f)));
        projTen.transform.parent = homingProjectileContainer.transform;
        projTen.Tip.tag = "tentacleProjectile"; // aka destroyable
        projTen.Tip.gameObject.layer = LayerMask.NameToLayer("TentacleTipProjectile");
        projTen.Width = ten.Width;
        projTen.Length = ten.Length;
        projTen.Detach();
        projTen.Animation = Tentacle.Animations.none;
        projTen.Drag *= (1f - killDrag);
        projTen.TargetRigidbody = gameObject.GetComponent<Dread>().Target.GetComponent<Rigidbody2D>();
        VisualEffect eff = Instantiate(smoke, ten.Joints[1].transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        eff.transform.parent = projTen.Joints[1].transform;
    }

    private void Cap()
    {
        if (homingProjectileContainer.transform.childCount > cap)
        {
            var startIdx = 0;
            var count = homingProjectileContainer.transform.childCount - cap; // no. of tentacles to destroy
            for (int i = startIdx; i < count; i++)
            {
                homingProjectileContainer.transform.GetChild(i).GetComponent<Tentacle>().startFade();
            }
        }
    }
}
