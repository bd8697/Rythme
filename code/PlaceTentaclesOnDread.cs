 using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTentaclesOnDread : MonoBehaviour
{
    [SerializeField] Dread dread;
    [SerializeField] GameObject tentacleBaseSprite;
    [SerializeField] Tentacle tentaclePrefab;
    [SerializeField] tentacleContainer tentacleContainer;
    [SerializeField] int tentacleCount = 50;
    [SerializeField] float gap = 5f;
    [SerializeField] float tentacleBaseRadius = 1f;
    [SerializeField] int stepLimit = 1000;
    [Range(0.01f, 2f)] [SerializeField] float restrictShapeFactor = 2f;

    float chanceOfDestroy = 0f;
    int currTentacleCount = 0;
    int steps = 0;
   // private List<GameObject> tentacleBases = FindObjectOfType<Dread>().TentacleBases;
  //  private List<GameObject> tentacles = FindObjectOfType<tentacleContainer>().Tentacles;

    // Start is called before the first frame update
    void Awake()
    {
        float dreadRadius = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        //dreadRadius *= 0.86f; // dont spawn on the circumference

      while(currTentacleCount < tentacleCount)
        {
            steps++;
            if(steps > stepLimit)
            {
                break;
            }
            bool overlapping = false;

            float a = Random.Range(0.0f, 1.0f) * 2 * Mathf.PI; // random point on circumference of circle with radius 1
            float r = dreadRadius * Mathf.Sqrt(Random.Range(0.0f, 1.0f));
            float x = r * Mathf.Cos(a);
            float y = r * Mathf.Sin(a);

            //chanceOfDestroy = ((dreadRadius * 2 - (dreadRadius + y)) / 2 + (dreadRadius * Mathf.Abs(x / dreadRadius))) / 2 / dreadRadius; //broken
            chanceOfDestroy = ((2 - (1 + y / dreadRadius)) / 2 + (1 * Mathf.Abs(x / dreadRadius))) / restrictShapeFactor; //calculated for dread with radius of 1 // still broken af and even more dumb than broken

            if (Random.Range(0.0f, 1.0f) < chanceOfDestroy)
            {
                continue;
            }

            //if (y > 0 && Mathf.Abs(x) + Mathf.Abs(y) > dreadRadius)
            //{
            //    continue;
            //}

            x += transform.position.x;
            y += transform.position.y;

            foreach (GameObject Base in dread.TentacleBases)
            {
                //Vector3 difference = new Vector3(
                //    Base.transform.position.x - x,
                //    Base.transform.position.y - y,
                //    0f);

                //float distance = Mathf.Sqrt(
                //    Mathf.Pow(difference.x, 2f) +
                //    Mathf.Pow(difference.y, 2f) +
                //    Mathf.Pow(difference.z, 2f));

                float distance = Vector3.Distance(Base.transform.position, new Vector3(x, y, 0f));
                if (distance < tentacleBaseRadius * 2) 
                {
                    overlapping = true;
                }
            }

            if(!overlapping)
            {
                var tentacleBasePosition = new Vector3(x, y, 0f);
                GameObject tentacleBase = Instantiate(tentacleBaseSprite, tentacleBasePosition, Quaternion.identity) as GameObject;
                tentacleBase.transform.localScale *= tentacleBaseRadius;
                tentacleBase.transform.parent = gameObject.transform;
                dread.TentacleBases.Add(tentacleBase);

                Tentacle tentacle = Instantiate(tentaclePrefab, tentacleBasePosition, Quaternion.identity);
                tentacle.Width = tentacleBaseRadius * 3;
                tentacle.Length = tentacle.Width * 10f;
                // tentacle.transform.localScale *= tentacleBaseRadius;
                tentacle.transform.parent = tentacleContainer.transform;
                tentacle.GetComponent<Tentacle>().ParentRigidbody = tentacleBase.GetComponent<Rigidbody2D>();
                tentacleContainer.Tentacles.Add(tentacle);

                currTentacleCount++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
