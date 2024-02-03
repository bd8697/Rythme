using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DreadAttack4 : MonoBehaviour
{
    [Range(0, 100)] [SerializeField] int projectileCount;
    [SerializeField] tentacleContainer container;
    [SerializeField] GameObject projectileContainer;
    [SerializeField] Material projectileMat; // todo: test for all three mats
    [SerializeField][Range(0f, 1f)] float killDrag;
    [SerializeField] float cap;
    string tag;
    int layer;


    private float tenCount;
    // Start is called before the first frame update
    void Start()
    {
        tenCount = container.Tentacles.Count;
        tag = "tentacleProjectile";
        layer = LayerMask.NameToLayer("TentacleTipProjectile");
    }

    // Update is called once per frame
    void Update()
    {
        Cap();
    }

    public void Attack4()
    {
        float deg = 0f;
        //gameObject.GetComponent<OffsetTentacleMat>().MatAlpha = 0.001f;

        for (int i = 1; i <= projectileCount; i++)
        {
            float rndIdx;
            deg = (float)i / projectileCount * 360f;
            do
            {
                rndIdx = Random.Range(0f, tenCount - 1);
            } while (container.Tentacles[(int)rndIdx].IsAttacking == true);
            
            Tentacle ten = container.Tentacles[(int)rndIdx];
            Tentacle projTen = Instantiate(ten, ten.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            projTen.Width = ten.Width;
            projTen.Length = ten.Length;
            projTen.Tip.tag = tag;
            projTen.Tip.gameObject.layer = layer;
            projTen.material = projectileMat;
            projTen.meshRenderer.material = projTen.material;
            projTen.Pivot.transform.rotation = Quaternion.Euler(new Vector3(0, 0, deg));
            projTen.transform.parent = projectileContainer.transform;
            projTen.Detach();
            projTen.Animation = Tentacle.Animations.swing;
            projTen.Drag *= (1f - killDrag);
        }
    }

    //public void Attack4()
    //{
    //    float deg = 0f;
    //    for (int i = 1; i <= projectileCount; i++)
    //    {
    //        deg = (float)i / projectileCount * 360f;
    //        VisualEffect eff = Instantiate(smoke, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
    //        eff.gameObject.layer = layer;
    //        eff.transform.rotation = Quaternion.Euler(new Vector3(0, 0, deg));
    //        // eff.transform.parent = projectileContainer.transform;
    //    }
    //}

    private void Cap()
    {
        if (projectileContainer.transform.childCount > cap)
        {
            var startIdx = 0;
            var count = projectileContainer.transform.childCount - cap; // no. of tentacles to destroy
            for (int i = startIdx; i < count; i++)
            {
                projectileContainer.transform.GetChild(i).GetComponent<Tentacle>().startFade();
            }
        }
    }
}
