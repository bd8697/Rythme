using Cubequad.Tentacles2D;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Qin : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    TMP_Text text;

    [SerializeField] GameObject projectileFireContainer;
    [SerializeField] GameObject firesContainer;
    [SerializeField] GameObject projectileContainer;
    [SerializeField] GameObject A3projectileContainer;

    [SerializeField] ScoreHandler scoreHandler;
    [SerializeField] float[] scores;

    [SerializeField] GameObject sensitivityVisualizer;
    [SerializeField] VisualEffect embersOnBeat;
    [SerializeField] VisualEffect slowTimeEmbers;
    [SerializeField] ChangePitch changePitch;
    [SerializeField] AudioSource dashSound1;
    [SerializeField] AudioSource dashSound2;
    [SerializeField] Color fireColor;
    [SerializeField] Color dashColor;
    [SerializeField] GameObject target;
    [SerializeField] QinController qinController;
    [SerializeField] ParticleSystem particles;
    [SerializeField] List<GameObject> qinProjectiles = new List<GameObject>();

    [SerializeField] List<GameObject> qinFires = new List<GameObject>();
    [SerializeField] Material[] fires;
    [SerializeField] GameObject fade;
    [SerializeField] float fadeSpeed;
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
    [SerializeField] float attackCooldown = 0f;

    public VisualEffect SlowTimeEmbers { get => slowTimeEmbers; set { slowTimeEmbers = value; } }
    public bool isAuto { get => qinController.Auto; set => qinController.Auto = value; }
    public bool isInvincible { get => invincible; set => invincible = value; }
    public QinController QinController { get => qinController; private set { qinController = value; } }
    public GameObject ProjectileFireContainer { get => projectileFireContainer; set { projectileFireContainer = value; } }
    public GameObject FiresContainer { get => firesContainer; set { firesContainer = value; } }
    public GameObject ProjectileContainer { get => projectileContainer; set { projectileContainer = value; } }
    public GameObject A3ProjectileContainer { get => A3projectileContainer; set { A3projectileContainer = value; } }


    public Material[] Fires { get => fires; set { fires = value; } }
    public ParticleSystem Particles { get => particles; private set { particles = value; } }
    public Color FireColor { get => fireColor; private set { fireColor = value; } }
    public Color DashColor { get => dashColor; private set { dashColor = value; } }
    public GameObject Target { get => target; private set { target = value; } }
    public float AttackCooldown { get => attackCooldown; private set { attackCooldown = value; } }

    public QinAttack1 Attack1 { get; private set; }
    public QinAttack2 Attack2 { get; private set; }
    public QinAttack3 Attack3 { get; private set; }
    public QinAttack4 Attack4 { get; private set; }
    public QinAttack5 Attack5 { get; private set; }

    public List<GameObject> QinProjectiles
    {
        get { return qinProjectiles; }
        set
        {
            for (int i = 0; i < qinProjectiles.Count; i++)
                qinProjectiles[i] = value[i];
        }
    }
    public List<GameObject> QinFires
    {
        get { return qinFires; }
        set
        {
            for (int i = 0; i < qinFires.Count; i++)
                qinFires[i] = value[i];
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Attack1 = gameObject.GetComponent<QinAttack1>();
        Attack2 = gameObject.GetComponent<QinAttack2>();
        Attack3 = gameObject.GetComponent<QinAttack3>();
        Attack4 = gameObject.GetComponent<QinAttack4>();
        Attack5 = gameObject.GetComponent<QinAttack5>();
        defense = gameObject.GetComponent<Defense>();
        move = QinController.GetComponent<Move>();
        AutoMove = QinController.GetComponent<autoMove>();

        audioOffset = changePitch.GetComponent<AudioVisualization>().AudioOffset;
        audioSource = changePitch.GetComponent<AudioSource>();
        audioVisualization = changePitch.GetComponent<AudioVisualization>();

        warmUpAttack1 = audioOffset;
        warmUpAttack2 = audioOffset;
        warmUpAttack3 = audioOffset;
        warmUpAttack4 = audioOffset;
        warmUpAttack5 = audioOffset;
        warmUpAttack1 -= attackCooldown;
        warmUpAttack2 -= attackCooldown * 2;
        warmUpAttack3 -= attackCooldown * 2;
        warmUpAttack4 -= attackCooldown * 4; //has to be 0 to prevent A4 overlapping
        warmUpAttack5 -= attackCooldown * 4;

        defenseOnBeat = false;

        text = scoreText.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (qinController.Auto)
            {
                //if (Input.GetKeyDown(KeyCode.F))
                //{
                //    //StartCoroutine(WaitThenAttack(1));
                //    callAttack(1);
                //}
                //if (Input.GetKeyDown(KeyCode.LeftControl))
                //{
                //   // StartCoroutine(WaitThenAttack(2));
                //    callAttack(2);
                //}
                //if (Input.GetKeyDown(KeyCode.CapsLock))
                //{
                //   // StartCoroutine(WaitThenAttack(3));
                //    callAttack(3);
                //}
                //if (Input.GetKeyDown(KeyCode.E))
                //{
                //   // StartCoroutine(WaitThenAttack(4));
                //    callAttack(4);
                //}
                //if (Input.GetKeyDown(KeyCode.Q))
                //{
                //    //StartCoroutine(WaitThenAttack(5));
                //    callAttack(5);
                //}
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space) && !isInvincible)
                {
                    text.text = "SPACE";
                    StartCoroutine(iFrames(invincibility));
                    defense.Defend(QinController.GetComponent<Rigidbody2D>());
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
                    fade.GetComponent<imageAlphaFade>().FadeObject(true, fadeSpeed, true, gameOver);
                }
            }
        }
        
    }

    void FixedUpdate()
    {

    }

    public void Auto()
    {
        qinController.Auto = true;
        sensitivityVisualizer.SetActive(true);
        qinController.GetComponent<Move>().enabled = false;
        qinController.GetComponent<autoMove>().enabled = true;
        Attack1.enabled = true;
        Attack2.enabled = true;
        Attack3.enabled = true;
        Attack4.enabled = true;
        Attack5.enabled = true;
        defense.enabled = false;
        ResetTrail();
    }

    public void Man()
    {
        qinController.Auto = false;
        sensitivityVisualizer.SetActive(false);
        qinController.GetComponent<Move>().enabled = true;
        qinController.GetComponent<autoMove>().enabled = false;
        Attack1.enabled = false;
        Attack2.enabled = false;
        Attack3.enabled = false;
        Attack4.enabled = false;
        Attack5.enabled = false;
        defense.enabled = true;
        ResetTrail();
        TakeTrashOut();
    }

    private void TakeTrashOut()
    {
        foreach (Transform fire in ProjectileFireContainer.transform)
        {
           Destroy(fire.gameObject);
        }
        foreach (Transform fire in FiresContainer.transform)
        {
            Destroy(fire.gameObject);
        }
        foreach (Transform fire in ProjectileContainer.transform)
        {
            Destroy(fire.gameObject);
        }
        foreach (Transform fire in A3ProjectileContainer.transform)
        {
            Destroy(fire.gameObject);
        }
        Attack3.StopAllCoroutines();
        foreach (Material fire in fires)
        {
            fire.SetVector("_Color", FireColor * 1f);
            fire.SetFloat("_GlobalAlpha", 1f);
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
        fade.GetComponent<imageAlphaFade>().FadeObject(true, fadeSpeed / 3f, true, gameOver);
    }

    public void ResetTrail()
    {
        particles.Clear();
        particles.Play();
    }

    private IEnumerator WaitThenAttack(int attack)
    {
        switch(attack) {
            case 1:
                yield return new WaitForSeconds(warmUpAttack4);
                Attack4.Attack4();
                break;
            case 2:
                yield return new WaitForSeconds(warmUpAttack5);
                Attack5.Attack5();
                break;
            case 3:
                yield return new WaitForSeconds(warmUpAttack1);
                Attack1.Attack1();
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
        foreach (Material fire in fires)
        {
            fire.SetVector("_Color", dashColor * 5f);
        }
        yield return new WaitForSeconds(secs);
        isInvincible = false;
        foreach (Material fire in fires)
        {
            fire.SetVector("_Color", FireColor * 1f);
        }

        move.CutVelocity(brake);
        if(!wasDefenseOnBeat)
        {
            target.GetComponent<autoMove>().AnglesToTarget(gameObject);
        }
    }

    public void callAttack(int attack)
    {
        if ((attack == 5 && AutoMove.isDashing) || (attack == 1 && Attack4.isAttacking)) // if already dashing or teleporting, change attack
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
        yield return new WaitForSeconds(audioOffset - defenseOnBeatDuration / 2 - attackCooldown);
        defenseOnBeatCounter++;
        defenseOnBeat = true;
        yield return new WaitForSeconds(defenseOnBeatDuration);
        defenseOnBeatCounter--;
        if(defenseOnBeatCounter == 0)
            defenseOnBeat = false;
    }

    private void HandleDefenseOnBeat()
    {
        target.GetComponent<autoMove>().rb2d.velocity /= 2;

        audioVisualization.CallStopLinearDecay();
        scoreHandler.MultiplyOnDef();

        embersOnBeat.Play();
    }

    public void AttackDash()
    {
        Attack2.Attack2();
    }
}