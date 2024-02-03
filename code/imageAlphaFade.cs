using Cubequad.Tentacles2D;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class imageAlphaFade : MonoBehaviour
{
    [SerializeField] AudioSource gameOverSound;
    [SerializeField] ChangePitch changePitch;
    [SerializeField] ClipFromPlaylist changeClip;
    [SerializeField] ScoreHandler scoreHandler;
    [SerializeField] Dread dread;
    [SerializeField] Qin qin;
    [SerializeField] QinController qinC;
    [Range(0f, 1f)][SerializeField] float minRevive;
    [Range(1f, 1.5f)] [SerializeField] float maxRevive;


    ParticleSystem.ShapeModule trailShape;
    ParticleSystem.MainModule trailMain;
    Image img;
    bool fadeIn = false;
    float alpha;
    Color color;
    bool fading = false;
    bool tryRevive;
    float fadeSpeed = 0f;
    bool fromQin;
    bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        trailShape = qinC.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().shape;
        trailMain = qinC.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().main;
    }


    // Update is called once per frame
    async Task Update()
    {
        if (fading)
        {
            if (fadeIn)
            {
                alpha += Time.deltaTime * fadeSpeed;
                TryRevive();

                if (alpha > 1.5f)
                {
                    if(gameOver)
                    {
                        if(fromQin)
                        {
                            qin.SlowTimeEmbers.Play();
                            DontDestroyOnLoad(qin.SlowTimeEmbers.transform.parent);
                        } 
                        else
                        {
                            dread.SlowTimeSmoke.Play();
                            DontDestroyOnLoad(dread.SlowTimeSmoke.transform.parent);
                        }
                        GameState gameState = FindObjectOfType<GameState>();
                        gameState.LastSceneIdx = SceneManager.GetActiveScene().buildIndex;
                        gameState.Score = scoreHandler.Score;
                        FindObjectOfType<SceneLoader>().LoadGameOverScene();
                    } 
                    else
                    {
                        fading = false;
                        await changeClip.ChangeClip();
                        alpha = 1.5f;
                        changePitch.PitchDown = false;
                        changePitch.PitchUp = true;
                        SetScale(fromQin);
                        SwitchPos();
                        SwitchControl(fromQin);
                        dread.MoveTentacles(qinC.transform.position);
                        scoreHandler.ScoreMultiplier = 1;
                        scoreHandler.TimeRoundStart = Time.time;

                        FadeObject(false, fadeSpeed, !fromQin, gameOver);
                    }
                }
            }
            else if (!fadeIn)
            {
                alpha -= Time.deltaTime * fadeSpeed;
                if (alpha < 0f)
                {
                    alpha = 0f;
                    fading = false;

                    qin.isInvincible = false;
                    dread.isInvincible = false;
                }
            }
            color.a = alpha;
            img.color = color;
        }
    }

    public void FadeObject(bool _fadeIn, float _fadeSpeed, bool _fromQin, bool _gameOver)
    {
        fading = true;
        fadeIn = _fadeIn;
        fadeSpeed = _fadeSpeed;
        fromQin = _fromQin;
        gameOver = _gameOver;
        img = gameObject.GetComponent<Image>();
        if (_fadeIn)
        {
            alpha = 0.01f;
            tryRevive = true;
            if(_gameOver)
            {
                gameOverSound.Play();
            }
        }
        else
        {
            alpha = 1f;
            tryRevive = false;
        }

        qin.isInvincible = true;
        dread.isInvincible = true;

        color = img.GetComponent<Image>().color;
    }

    private void SwitchPos()
    {
        Vector3 dreadPos = qinC.transform.position;
        dreadPos.z = dread.transform.position.z;
        Vector3 qinPos = dread.transform.position;
        qinPos.z = qinC.transform.position.z;

        qinC.transform.position = qinPos;
        dread.transform.position = dreadPos;
        
        //Quaternion auxRot = from.transform.rotation;
        //from.transform.rotation = to.transform.rotation;
        //to.transform.rotation = auxRot;

        //Vector2 auxVel = from.GetComponent<Rigidbody2D>().velocity;
        //from.GetComponent<Rigidbody2D>().velocity = to.GetComponent<Rigidbody2D>().velocity;
        //to.GetComponent<Rigidbody2D>().velocity = auxVel;
    }

    private void SwitchControl(bool fromQ)
    {
        if(fromQ)
        {
            qin.Auto();
            dread.Man();
        } 
        else
        {
            dread.Auto();
            qin.Man();
        }
    }
    private void SetScale(bool fromQ)
    {
        if (fromQ)
        {
            dread.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            foreach (Tentacle ten in dread.Tentacles.Tentacles)
            {
                ten.SetLength /= 2f;
                ten.Width /= 2f;
                ten.Length /= 2f;
            }

            qin.transform.localScale = new Vector3(1f, 1f, 1f);
            qinC.transform.localScale = new Vector3(1f, 1f, 1f);
            trailShape.radius *= 2;
            trailMain.startSize = new ParticleSystem.MinMaxCurve(trailMain.startSize.constantMin * 2f, trailMain.startSize.constantMax * 2f);
        }
        else
        {
            qin.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            qinC.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            trailShape.radius /= 2;
            trailMain.startSize = new ParticleSystem.MinMaxCurve(trailMain.startSize.constantMin / 2f, trailMain.startSize.constantMax / 2f);

            dread.transform.localScale = new Vector3(1f, 1f, 1f);
            foreach (Tentacle ten in dread.Tentacles.Tentacles)
            {
                ten.SetLength *= 2f;
                ten.Width *= 2f;
                ten.Length *= 2f;
            }
        }
    }

    private void TryRevive()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (tryRevive && alpha > minRevive && alpha < maxRevive)
            {
                gameOver = false;
            }
            tryRevive = false;
        }
    }
}
