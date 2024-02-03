using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Range(0f, 1f)] [SerializeField] float scaleScale;
    [Range(0f, 1f)] [SerializeField] float timeScale;
    [Range(0f, 1f)] [SerializeField] float pitchScale;
    [SerializeField] float effectDuration = 5f;
    [SerializeField] float moveY = 0f;
    [SerializeField] float odds = 0.00003f; // 0.00003f (3e-05) -> ~ once in 10 minutes ( 1 in 10 minutes * 60 seconds * 60 fps)
    [SerializeField] ConstantMove powerUp;
    [SerializeField] AudioSource audioSource;
    AudioSource fakeAudio;

    float rnd;
    bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        fakeAudio = audioSource.transform.parent.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        rnd = Random.Range(0f, 100f);
        if(rnd < odds && transform.childCount < 1 && !triggered)
        {
            StartCoroutine(Triggered());
            float halfHeight = Camera.main.orthographicSize;
            float halfWidth = halfHeight * Screen.width / Screen.height;
            var pos = new Vector3(Random.Range(-halfHeight, halfHeight), halfHeight * 1.25f, 0f);
            var powerUpInstance = Instantiate(powerUp, pos, Quaternion.identity, transform);
            powerUpInstance.MoveY = moveY;
        }
    }

    public void SpeedUp(Collider2D collWith)
    {
        Transform toScale = null;
        tentacleContainer tentacles = null;
        if(collWith.GetComponent<QinController>())
        {
            toScale = collWith.transform.parent.transform;
        }
        else if (collWith.GetComponent<Dread>())
        {
            tentacles = collWith.GetComponent<Dread>().Tentacles;
            foreach (Tentacle ten in tentacles.Tentacles)
            {
                ten.Width /= 2f;
                ten.Length /= 2f;
            }
            toScale = collWith.transform;
        }
        toScale.localScale = new Vector3((1f - scaleScale), (1f - scaleScale), 1f);
        audioSource.pitch = (1f + pitchScale);
        fakeAudio.pitch = (1f + pitchScale);
        StartCoroutine(ResetScale(toScale, tentacles));
    }

    public void SlowDown(Collider2D collWith)
    {
        Move move = collWith.GetComponent<Move>();
        move.MovementSpeed *= 2f;
        move.AngularSpeed *= 2f;
        Time.timeScale = (1f - timeScale);
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        audioSource.pitch = (1f - pitchScale);
        fakeAudio.pitch = (1f - pitchScale);
        StartCoroutine(ResetSpeed(move));
    }

    private IEnumerator ResetSpeed(Move move)
    {
        yield return new WaitForSeconds(effectDuration * (1f -timeScale));
        move.MovementSpeed /= 2f;
        move.AngularSpeed /= 2f;
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02F;
        audioSource.pitch = 1f;
        fakeAudio.pitch = 1f;
    }

    private IEnumerator Triggered()
    {
        triggered = true;
        yield return new WaitForSeconds(effectDuration * (1f - timeScale));
        triggered = false;
    }

    private IEnumerator ResetScale(Transform toScale, tentacleContainer tentacles)
    {
        yield return new WaitForSeconds(effectDuration);
        if (tentacles)
            ResetTentacles(tentacles);
        toScale.localScale = new Vector3(1f, 1f, 1f);
        audioSource.pitch = 1f;
        fakeAudio.pitch = 1f;

    }

    private void ResetTentacles(tentacleContainer tentacles)
    {
        foreach (Tentacle ten in tentacles.Tentacles)
        {
            ten.Width *= 2f;
            ten.Length *= 2f;
        }
    }

}
