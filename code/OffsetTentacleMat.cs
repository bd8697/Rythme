using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetTentacleMat : MonoBehaviour
{
    [SerializeField] Material matLeft;
    [SerializeField] Material matMiddle;
    [SerializeField] Material matRight;
    [SerializeField] Material projectileMat;
    [SerializeField] public float scrollSpeedY = 0f;
    Vector2 offsetY;
    Vector2 matLeftOffset;
    Vector2 matMiddleOffset;
    Vector2 matRightOffset;
    Vector2 projectileMatOffset;

    [SerializeField] float matAlpha = 1f;
    public float MatAlpha { get => matAlpha; set { matAlpha = value; } }

    // Start is called before the first frame update
    void Start()
    {
        offsetY = new Vector2(0f, scrollSpeedY);
        matLeftOffset = new Vector2(0f, 0f);
        matMiddleOffset = new Vector2(0f, 0f);
        matRightOffset = new Vector2(0f, 0f);
        projectileMatOffset = new Vector2(0f, 0f);
        MatAlpha = 0.001f;
        projectileMat.SetFloat("myAlpha", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        matLeftOffset -= offsetY * Time.deltaTime;
        matMiddleOffset -= offsetY * Time.deltaTime;
        matRightOffset -= offsetY * Time.deltaTime;
        projectileMatOffset -= offsetY * Time.deltaTime;
        matLeft.SetFloat("myOffset", matLeftOffset.y);
        matMiddle.SetFloat("myOffset", matMiddleOffset.y);
        matRight.SetFloat("myOffset", matRightOffset.y);
        projectileMat.SetFloat("myOffset", projectileMatOffset.y);
        // matMiddle.mainTextureOffset -= offsetY * Time.deltaTime;

        AlphaBuildUp(matLeft, matMiddle, matRight);
    }

    private void AlphaBuildUp(params Material[] mats)
    {
        if (MatAlpha < 1f)
        {
            MatAlpha *= 1.1f;
            if (MatAlpha > 1f)
            {
                MatAlpha = 1f;
            }
            foreach(Material mat in mats)
            {
                mat.SetFloat("myAlpha", MatAlpha);
            }
        }
    }
}
