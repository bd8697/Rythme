using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] GameObject sudoParent;
    Rigidbody2D parentRB;
    float sudoAngularVelocity;
    [SerializeField] float maxAngVel;
    [SerializeField] float angularDrag;

    public float MaxAngVel { get => maxAngVel; set { maxAngVel = value; } }

    public float SudoAngularVelocity { get => sudoAngularVelocity; set { sudoAngularVelocity = value; } }

    // Start is called before the first frame update
    void Start()
    {
        sudoAngularVelocity = 0f;
        parentRB = sudoParent.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
            sudoAngularVelocity += parentRB.angularVelocity / angularDrag;
            sudoAngularVelocity = Mathf.Min(sudoAngularVelocity, maxAngVel); // clamp...
            sudoAngularVelocity = Mathf.Max(sudoAngularVelocity, -maxAngVel);

            gameObject.transform.Rotate(0f, 0f, sudoAngularVelocity * Time.deltaTime * GameState.DeltaTimeCorrection);
        // gameObject.transform.Rotate(0f, 0f, 5 * Time.deltaTime * 10);
    }
}
