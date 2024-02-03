using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Dread : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    TMP_Text text;

    [SerializeField] GameObject[] containers;
    [SerializeField] ScoreHandler scoreHandler;
    [SerializeField] float[] scores;

    [SerializeField] GameObject target;
    [SerializeField] GameObject fade;
    [SerializeField] float fadeSpeed;
    [SerializeField] GameObject sensitivityVisualizer;
    [SerializeField] ChangePitch changePitch;
    [SerializeField] AudioSource dashSound1;
    [SerializeField] AudioSource dashSound2;
    [SerializeField] ValueFade valueFade;
    [SerializeField] VisualEffect smokeOnBeat;
    [SerializeField] VisualEffect embersOnCollision;
    [SerializeField] VisualEffect dreadSmokeDash;
    [SerializeField] VisualEffect slowTimeSmoke;
    [SerializeField] float invincibility;
    [SerializeField] float brake;

    [SerializeField] float warmUpAttack1;
    [SerializeField] float warmUpAttack2;
    [SerializeField] float warmUpAttack3;
    [SerializeField] float warmUpAttack4;
    [SerializeField] float warmUpAttack5;

    [Header("DefenseOnBeat")]
    bool defenseOnBeat;
    float defenseOnBeatCounter = 0f;
    [Range(0f, 1f)][SerializeField] float defenseOnBeatDuration;

    float audioOffset;
    AudioSource audioSource;
    AudioVisualization audioVisualization;
    Defense defense;
    Move move;
    public autoMove AutoMove { get; set; }
    float time = 0f;
    bool invincible;
    [SerializeField] public tentacleContainer tentacles;
    List<GameObject> tentacleBases = new List<GameObject>();
    bool auto;

    public VisualEffect SlowTimeSmoke { get => slowTimeSmoke; set => slowTimeSmoke = value; }
    public tentacleContainer Tentacles { get => tentacles; set => tentacles = value; }
    public bool isAuto { get => auto; set => auto = value; }
    public bool isInvincible { get => invincible; set => invincible = value; }
    public DreadAttack1 Attack1 { get; private set; }
    public DreadAttack2 Attack2 { get; private set; }
    public DreadAttack3 Attack3 { get; private set; }
    public DreadAttack4 Attack4 { get; private set; }
    public DreadAttack5 Attack5 { get; private set; }
    public GameObject Target { get => target; private set { target = value; } }

    public List<GameObject> TentacleBases
    {
        get { return tentacleBases; }
        set
        {
            for (int i = 0; i < tentacleBases.Count; i++)
                tentacleBases[i] = value[i];
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Attack1 = gameObject.GetComponent<DreadAttack1>();
        Attack2 = gameObject.GetComponent<DreadAttack2>();
        Attack3 = gameObject.GetComponent<DreadAttack3>();
        Attack4 = gameObject.GetComponent<DreadAttack4>();
        Attack5 = gameObject.GetComponent<DreadAttack5>();
        defense = gameObject.GetComponent<Defense>();
        move = gameObject.GetComponent<Move>();
        AutoMove = gameObject.GetComponent<autoMove>();
        GetComponent<SetPositionOnDread>().CalculatePositionsOnDread();

        auto = true;

        audioOffset = changePitch.GetComponent<AudioVisualization>().AudioOffset;
        audioSource = changePitch.GetComponent<AudioSource>();
        audioVisualization = changePitch.GetComponent<AudioVisualization>();

        warmUpAttack1 = audioOffset;
        warmUpAttack2 = audioOffset;
        warmUpAttack3 = audioOffset;
        warmUpAttack4 = audioOffset;
        warmUpAttack5 = audioOffset;
        warmUpAttack1 -= tentacles.Tentacles[0].AttackCooldown * 2;
        warmUpAttack2 -= tentacles.Tentacles[0].AttackCooldown;
        warmUpAttack3 -= tentacles.Tentacles[0].AttackCooldown;
        warmUpAttack4 -= tentacles.Tentacles[0].AttackCooldown;
        warmUpAttack5 -= tentacles.Tentacles[0].AttackCooldown;

        defenseOnBeat = false;

        text = scoreText.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (auto)
            {
                //if (Input.GetKeyDown(KeyCode.F))
                //{
                //    StartCoroutine(WaitThenAttack(1));
                //}
                //if (Input.GetKeyDown(KeyCode.LeftControl))
                //{
                //    StartCoroutine(WaitThenAttack(2));
                //}
                //if (Input.GetKeyDown(KeyCode.CapsLock))
                //{
                //    StartCoroutine(WaitThenAttack(3));
                //}
                //if (Input.GetKeyDown(KeyCode.E))
                //{
                //    StartCoroutine(WaitThenAttack(4));
                //}
                //if (Input.GetKeyDown(KeyCode.Q))
                //{
                //    callAttack(5);
                //}
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space) && !isInvincible)
                {
                    text.text = "SPACE";
                    StartCoroutine(iFrames(invincibility));
                    dreadSmokeDash.Play();
                    defense.Defend(GetComponent<Rigidbody2D>());
                    if (defenseOnBeat)
                    {
                        dashSound2.Play();
                        HandleDefenseOnBeat();
                    }
                    else
                    {
                        dashSound1.Play();
                    }
                }
                if ((Input.GetKeyDown(KeyCode.LeftShift) && fade.GetComponent<Image>().color.a <= 0f) || (!audioSource.isPlaying && Time.timeScale == 1f))
                {
                    text.text = "SHIFT";
                    changePitch.PitchDown = true;

                    var gameOver = false;
                    fade.GetComponent<imageAlphaFade>().FadeObject(true, fadeSpeed, false, gameOver);
                }
            }
        }
           
    }

    void FixedUpdate()
    {
        GetComponent<AddPullToTentacles>().AddForceToTentacles();
    }

    void OnTriggerEnter2D(Collider2D collWith)
    {
        //Debug.Log(collWith);
        switch(collWith.gameObject.tag)
        {
            case "top":
            case "bot":
                {
                    foreach (Tentacle tentacle in tentacles.Tentacles)
                    {
                        tentacle.transform.position -= new Vector3(0, collWith.gameObject.transform.position.y * 1.5f, 0);
                    }
                    transform.position -= new Vector3(0, collWith.gameObject.transform.position.y * 1.5f, 0);

                    if (!auto)
                    {
                        target.GetComponent<QinController>().qin.AttackDash();
                    }
                    break;
                }

            case "left":
            case "right":
                {
                    foreach (Tentacle tentacle in tentacles.Tentacles)
                    {
                        tentacle.transform.position -= new Vector3(collWith.gameObject.transform.position.x * 1.7f, 0, 0);
                    }
                    transform.position -= new Vector3(collWith.gameObject.transform.position.x * 1.7f, 0, 0);

                    if(!auto)
                    {
                        target.GetComponent<QinController>().qin.AttackDash();
                    }
                    break;
                }
            case "qinController":
            case "QinProjectile":
            case "QinFireProjectile":
                {
                    if (!isAuto && !isInvincible)
                    {
                        GetComponent<AudioSource>().Play();
                        embersOnCollision.transform.position = collWith.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(transform.position);
                        embersOnCollision.Play();
                        GetComponent<Move>().SlowFactor *= 2;
                        GetComponent<CircleCollider2D>().enabled = false;
                        GameOver();
                    }
                    break;
                }
        }
    }

    public string targetOnSide()
    {
        float distance = Vector2.Distance(gameObject.transform.position, target.transform.position);
        float distanceRightOffset = Vector2.Distance(gameObject.transform.position + transform.right.normalized / 100, target.transform.position);
        if (distanceRightOffset <= distance)
        {
            return "right";

        }
        else
        {
            return "left";
        }
    }

    public void Auto()
    {
        auto = true;
        sensitivityVisualizer.SetActive(true);
        gameObject.GetComponent<Move>().enabled = false;
        gameObject.GetComponent<autoMove>().enabled = true;
        Attack1.enabled = true;
        Attack2.enabled = true;
        Attack3.enabled = true;
        Attack4.enabled = true;
        Attack5.enabled = true;
        defense.enabled = false;
        foreach (Tentacle ten in Tentacles.Tentacles)
        {
            ten.Tip.GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    public void Man()
    {
        auto = false;
        sensitivityVisualizer.SetActive(false);
        gameObject.GetComponent<Move>().enabled = true;
        gameObject.GetComponent<autoMove>().enabled = false;
        Attack1.enabled = false;
        Attack2.enabled = false;
        Attack3.enabled = false;
        Attack4.enabled = false;
        Attack5.enabled = false;
        defense.enabled = true;
        TakeTrashOut();
        foreach (Tentacle ten in Tentacles.Tentacles)
        {
            ten.Tip.GetComponent<CircleCollider2D>().enabled = false;
            // ten.Drag *= 1f; // not needed, but this would actually double it. Tentacle editor is broken af
        }
    }

    public void MoveTentacles(Vector3 newPos)
    {
        newPos.z = 0;
        foreach (Tentacle tentacle in tentacles.Tentacles)
        {
            tentacle.transform.position -= (newPos - gameObject.transform.position);
        }
    }

    private void TakeTrashOut()
    {
        foreach(GameObject container in containers)
        {
            foreach (Transform transform in container.transform)
            {
                var child = transform.gameObject;
                Destroy(child); // :(
            }
        }
        StopAllCoroutines();
    }

    public void GameOver()
    {
        if (scoreHandler.Score > myPlayerPrefs.GetMasterScore())
        {
            myPlayerPrefs.SetMasterScore(scoreHandler.Score);
        }
        myPlayerPrefs.SetMasterDeathCount();

        changePitch.PitchDown = true;

        var gameOver = true;
        fade.GetComponent<imageAlphaFade>().FadeObject(true, fadeSpeed / 3f, false, gameOver);
    }

    private IEnumerator WaitThenAttack(int attack)
    {
        switch (attack)
        {
            case 1:
                yield return new WaitForSeconds(warmUpAttack4);
                Attack4.Attack4();
                break;
            case 2:
                yield return new WaitForSeconds(warmUpAttack1);
                Attack1.Attack1();
                break;
            case 3:
                yield return new WaitForSeconds(warmUpAttack5);
                Attack5.Attack5();
                break;
            case 4:
                yield return new WaitForSeconds(warmUpAttack3);
                Attack3.Attack3();
                break;
            case 5:
                AutoMove.isDashing = true;
                yield return new WaitForSeconds(warmUpAttack2);
                Attack2.Attack2();
                break;
        }
        scoreHandler.addScore(scores[attack - 1]);
    }

    private IEnumerator iFrames(float secs)
    {
        var wasDefenseOnBeat = defenseOnBeat;
        isInvincible = true;
        yield return new WaitForSeconds(secs);
        isInvincible = false;
        move.CutVelocity(brake);
        if(!wasDefenseOnBeat)
        {
            target.GetComponent<autoMove>().AnglesToTarget(gameObject);
        }
    }

    public void callAttack(int attack)
    {
        if(attack == 5 && AutoMove.isDashing) // if already dashing, change attack
        {
            attack = 4;
        }
        StartCoroutine(WaitThenAttack(attack));
    }
    public void callDefenseOnBeat()
    {
        StartCoroutine(SetDefenseOnBeat());
    }

    private IEnumerator SetDefenseOnBeat()
    {
        yield return new WaitForSeconds(audioOffset - defenseOnBeatDuration / 2 - tentacles.Tentacles[0].AttackCooldown);
        defenseOnBeatCounter++;
        defenseOnBeat = true;
        yield return new WaitForSeconds(defenseOnBeatDuration);
        defenseOnBeatCounter--;
        if (defenseOnBeatCounter == 0)
            defenseOnBeat = false;
    }

    private void HandleDefenseOnBeat()
    {
        target.GetComponent<autoMove>().rb2d.velocity /= 2;

        audioVisualization.CallStopLinearDecay();
        scoreHandler.MultiplyOnDef();

        smokeOnBeat.Play();

        //var changeProp = valueFade.GetComponent<ChangePropWhileVal>();
        //changeProp.ChangeProp();
        //valueFade.FadeValue(true, changeProp.ChangeProp);
    }

    public void AttackDash()
    {
        Attack2.Attack2();
    }
}
