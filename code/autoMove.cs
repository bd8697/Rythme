using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class autoMove : MonoBehaviour
{
    [Range(1f, 2f)] [SerializeField] float brakeFactor = 1.05f;
    [Range(1f, 2f)] [SerializeField] float slowFactor = 1.01f;

    public Rigidbody2D rb2d;
    [SerializeField] float minRotationDuration = 1f;
    [SerializeField] float maxRotationDuration = 5f;
    [SerializeField] float minAccelerationDuration = 1f;
    [SerializeField] float maxAccelerationDuration = 5f;
    [SerializeField] float minAcceleration = 5f;
    [SerializeField] float maxAcceleration = 15f;
    [SerializeField] float minAngularSpeed = 5f;
    [SerializeField] float maxAngularSpeed = 15f;
    [SerializeField] float awayFromEdgeMulti;
    float rotationDirection = 1f;
    float rotationDuration = 0f;
    float acceleration = 0f;
    public float angularAcceleration = 0f;
    float accelerationDuration = 0f;
    bool turningTowardsTarget = false;
    float screenHeight;
    float screenWidth;
    public bool isDashing { get; set; }

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(Rotate());
        StartCoroutine(Accelerate());
        screenHeight = Camera.main.orthographicSize * 2.0f;
        screenWidth = screenHeight * Camera.main.aspect;
    }

    private IEnumerator Rotate()
    {
        rotationDuration = Random.Range(minRotationDuration, maxRotationDuration);
        if (!turningTowardsTarget)
        {
            angularAcceleration = Random.Range(minAngularSpeed, maxAngularSpeed);
            rotationDirection *= -1;
        }
        yield return new WaitForSeconds(rotationDuration);
        StartCoroutine(Rotate());
    }

    private IEnumerator Accelerate()
    {
        accelerationDuration = Random.Range(minAccelerationDuration, maxAccelerationDuration);
        if (!turningTowardsTarget && !isDashing)
        {
            acceleration = Random.Range(minAcceleration, maxAcceleration);
        }
        yield return new WaitForSeconds(accelerationDuration);
        StartCoroutine(Accelerate());
    }

    public void AnglesToTarget(GameObject target)
    {
        Vector3 thisPos = transform.position;
        Vector3 targetPos = target.transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        var angle1 = Mathf.Atan2(transform.up.x, transform.up.y) * Mathf.Rad2Deg;
        var angle2 = Mathf.Atan2(targetPos.x, targetPos.y) * Mathf.Rad2Deg;
        var deltaAngle = Mathf.DeltaAngle(angle1, angle2);
        StartCoroutine(TurnToTargetOnDash(-deltaAngle / 2.75f, 0.5f)); // #magicNumberLivesMatter
    }
    public IEnumerator TurnToTargetOnDash(float torque, float duration)
    {
        angularAcceleration = torque;
        rb2d.velocity /= 2;
        acceleration = maxAcceleration * 2;
        turningTowardsTarget = true;
        yield return new WaitForSeconds(duration);
        acceleration /= 2;
        turningTowardsTarget = false;
    }

    void FixedUpdate()
    {
        if (!turningTowardsTarget)
        {
            AwayFromEdge();
        }
        if (isDashing)
        {
            angularAcceleration = 0;
        }
        rb2d.AddTorque(angularAcceleration * rotationDirection);
        rb2d.AddForce(transform.up.normalized * acceleration);

        rb2d.velocity /= slowFactor;
        rb2d.angularVelocity /= brakeFactor;
        Vector2 velocity = rb2d.velocity;
        rb2d.velocity = Vector2.zero;
        rb2d.velocity = transform.up * velocity.magnitude;
    }

    private void AwayFromEdge() //after 150 deleted lines...
    {

        var position = transform.position;
        if ((transform.up.x < 0f && position.x < 0f) || (transform.up.x > 0f && position.x > 0f))
        {
            angularAcceleration = maxAngularSpeed * awayFromEdgeMulti * (Mathf.Abs(position.x) / (screenWidth / 2));
        }
        else if ((transform.up.y > 0f && position.y > 0f) || (transform.up.y < 0f && position.y < 0f))
        {
            angularAcceleration = maxAngularSpeed * awayFromEdgeMulti * (Mathf.Abs(position.y) / (screenHeight / 2));
        }
        else
        {
            angularAcceleration = 0;
        }
    }
}
