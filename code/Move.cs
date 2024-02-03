using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    TMP_Text text;

    [SerializeField] float movementSpeed = 5.0f;
    [Range(1f, 2f)] [SerializeField] float slowFactor = 1.01f;
    [SerializeField] float angularSpeed = 150.0f;
    [Range(1f, 2f)][SerializeField] float brakeFactor = 1.05f;
    [Range(0f, 1f)] [SerializeField] float brakeToTurnAnglePerc;

    Rigidbody2D rb2d;

    float colSize;
    float scrH;
    float scrL;
    [SerializeField] GameObject colH;
    [SerializeField] GameObject colL;

    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public float AngularSpeed { get => angularSpeed; set => angularSpeed = value; }
    public float SlowFactor { get => slowFactor; set => slowFactor = value; }

    private bool accelerate;

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        text = scoreText.GetComponent<TMP_Text>();

        scrH = Camera.main.orthographicSize;
        scrL = scrH * Screen.width / Screen.height;
        colSize = GetComponent<Collider2D>().bounds.size.x;
    }

    void FixedUpdate()
    {
        Teleport();

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            text.text = "W";
            AddTorque(transform.up.y, transform.up.x, true, false);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            text.text = "S";
            AddTorque(transform.up.y, transform.up.x, false, true);
        }

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !Stuck())
        {
            text.text = "A";
            //  rb2d.AddForce(transform.up.normalized * movementSpeed);
            // rb2d.AddForce(-transform.right.normalized * movementSpeed * 5);
            // transform.Rotate(0, 0, Time.deltaTime * angularSpeed);
            AddTorque(transform.up.x, transform.up.y, false, false);
        }

        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !Stuck())
        {
            text.text = "D";
            AddTorque(transform.up.x, transform.up.y, true, true);
        }

        if(accelerate)
        {
            rb2d.AddForce(transform.up.normalized * movementSpeed);
            accelerate = false;
        }

        rb2d.velocity /= slowFactor;
        rb2d.angularVelocity /= brakeFactor;
        Vector2 velocity = rb2d.velocity;
        rb2d.velocity = Vector2.zero;
        rb2d.velocity = transform.up * velocity.magnitude;
    }

    private void AddTorque(float magnitudeDirection, float torqueDirection, bool magnitudeMirror, bool torqueMirror)
    {
        var magnitudeMultiplier = 1f;
        var torqueMultiplier = 1f;
        if(magnitudeMirror)
        {
            magnitudeMultiplier *= -1f;
        }
        if (torqueMirror)
        {
            torqueMultiplier *= -1f;
        }

        var magnitude = Mathf.Abs(Mathf.Sign(magnitudeDirection) + magnitudeMultiplier * Mathf.Abs(magnitudeDirection));
        if (magnitude > 1f + brakeToTurnAnglePerc)
        {
            rb2d.velocity /= Mathf.Sqrt(magnitude);
        }
        rb2d.AddTorque(torqueMultiplier * angularSpeed * Mathf.Sign(torqueDirection) * magnitude);

        accelerate = true;
    }

    public void CutVelocity(float factor)
    {
        rb2d.velocity /= factor;
    }

    private bool Stuck()
    {
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && transform.up.y <= 0f)
            {
            return true;
            }
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && transform.up.y >= 0f)
            {
            return true;
            }
        return false;
    }

    private void Teleport()
    {
        if(transform.position.x > scrL + colSize)
        {
            var pos = transform.position;
            pos.x = colL.transform.position.x - colL.GetComponent<Collider2D>().bounds.size.x / 2;
            transform.position = pos;
        }
        if (transform.position.x < -scrL - colSize)
        {
            var pos = transform.position;
            pos.x = -colL.transform.position.x + colL.GetComponent<Collider2D>().bounds.size.x / 2;
            transform.position = pos;
        }
        if (transform.position.y > scrH + colSize)
        {
            var pos = transform.position;
            pos.y = colH.transform.position.y - colH.GetComponent<Collider2D>().bounds.size.y / 2;
            transform.position = pos;
        }
        if (transform.position.y < -scrH - colSize)
        {
            var pos = transform.position;
            pos.y = -colH.transform.position.y + colH.GetComponent<Collider2D>().bounds.size.y / 2;
            transform.position = pos;
        }
    }
}
