using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionOnDread : MonoBehaviour
{
    [SerializeField] Material matLeft;
    [SerializeField] Material matMiddle;
    [SerializeField] Material matRight;
    [SerializeField] Dread dread;
    [SerializeField] tentacleContainer tentacleContainer;
    [Range(-1f, 1f)] [SerializeField] float verticalOffset = 0.5f;
    // force shifts direction at 0 (middle of circle), adding offset increases force down range, subtracting increases force up range
    // verticalOffset = percentage of dreadRadius
    [Range(0f, 1f)][SerializeField] float horizontalOffset = 0.25f; // only positive values( 0 = no offset)
    [SerializeField] float forceDampenOnReversePercentage = 0.9f;

    [Range(0f, 0.5f)] [SerializeField] float sideMatsPer = 0.25f;

    public void CalculatePositionsOnDread()
    {
        float x, y;
        float dreadRadius = dread.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        Vector2 dreadCenter = new Vector2(dread.transform.position.x, dread.transform.position.y);

        foreach(Tentacle tentacle in tentacleContainer.Tentacles)
        {
            y = (tentacle.transform.position.y - dreadCenter.y + 
                verticalOffset * dreadRadius) / dreadRadius;
            x = (tentacle.transform.position.x - dreadCenter.x + 
                Mathf.Sign(tentacle.transform.position.x - dreadCenter.x) * horizontalOffset) / dreadRadius;
            x *= (tentacle.transform.position.y / (dreadCenter.y + dreadRadius));
            if(y < 0)
            {
                x *= (1f - forceDampenOnReversePercentage);
            }

            tentacle.CoordsInDread = new Vector2(x, y);

            if (x > 0)
            {
                if (y > 0)
                {
                    tentacle.tag = "topRight";
                    tentacleContainer.TopRight.Add(tentacle);
                }
                else
                {
                    tentacle.tag = "botRight";
                }
            }
            else
            {
                if (y > 0)
                {
                    tentacle.tag = "topLeft";
                    tentacleContainer.TopLeft.Add(tentacle);
                }
                else
                {
                    tentacle.tag = "botLeft";
                }
            }

            //if (x < -1 * (1 - sideMatsPer)) // 1 = radius of circle
            //{
            //    tentacle.material = matRight;
            //    tentacle.meshRenderer.material = matRight;
            //    // tentacle.GetComponent<MeshRenderer>().material = matRight;
            //}
            //else if (x > 1 * (1 - sideMatsPer)) // 1 = radius of circle
            //{
            //    tentacle.material = matLeft;
            //    tentacle.meshRenderer.material = matLeft;
            //    // tentacle.GetComponent<MeshRenderer>().material = matLeft;
            //}

        }

        //foreach (Tentacle tentacle in tentacleContainer.Tentacles)
        //{
        //    Debug.Log(tentacle.CoordsInDread.x);
        //}

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
