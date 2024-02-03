using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [Header("Global")]
    [SerializeField] private float multiplyOnDef;
    [SerializeField] private float divideTimeDeltaBy;
    public float ScoreMultiplier { get; set; }
    public float TimeRoundStart { get; set; }

    [Header("Score")]
    [SerializeField] float scoreAnimSpeed;
    [SerializeField] float effectLimitUp;
    [SerializeField] float effectLimitDown;
    [SerializeField] float effectLimitStart;

    [SerializeField] Color scoreDownColor;
    [SerializeField] Color scoreStartColor;
    [Range(1f, 0f)][SerializeField] float startPower;

    float score;
    int scoreAnimDirection = 0; // -1 = down; 0 = none; 1 = up;
    bool scoreAnimUp = true;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Material scoreTextMat;
    TMP_Text text;

    // public bool ScoreAnim { get => scoreAnim; set { scoreAnim = value; } }
    public float Score { get => score; set { score = value; } }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreAnimDirection = 0;
        text = scoreText.GetComponent<TMP_Text>();
        ScoreMultiplier = 1;
        TimeRoundStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        ScoreUp();
        ScoreDown();
        text.text = score.ToString();
    }

    private void ScoreUp()
    {
        if(scoreAnimDirection == 1)
        {
            if(scoreAnimUp)
            {
                text.fontSize += scoreAnimSpeed * Time.deltaTime * GameState.DeltaTimeCorrection * -1;
            }
            else
            {
                text.fontSize -= scoreAnimSpeed * Time.deltaTime * GameState.DeltaTimeCorrection * -1;
            }

            if (Mathf.Abs(text.fontSize) > effectLimitUp)
            {
                scoreAnimUp = false;
            }
            else if (Mathf.Abs(text.fontSize) < effectLimitStart)
            {
                text.fontSize = -effectLimitStart;
                scoreAnimUp = true;
                scoreAnimDirection = 0;

                scoreTextMat.SetFloat("_GlowPower", startPower);
            }
        }
    }

    public void ScoreDown()
    {
        if (scoreAnimDirection == -1)
        {
            if (scoreAnimUp)
            {
                text.fontSize -= scoreAnimSpeed * Time.deltaTime * GameState.DeltaTimeCorrection * -1;
            }
            else
            {
                text.fontSize += scoreAnimSpeed * Time.deltaTime * GameState.DeltaTimeCorrection * -1;
            }

            if (Mathf.Abs(text.fontSize) < effectLimitDown)
            {
                scoreAnimUp = false;
            }
            else if (Mathf.Abs(text.fontSize) > effectLimitStart)
            {
                text.fontSize = -effectLimitStart;
                scoreAnimUp = true;
                scoreAnimDirection = 0;

                scoreTextMat.SetFloat("_GlowPower", startPower);
                scoreTextMat.SetColor("_GlowColor", scoreStartColor);
            }
        }
    }

    public void addScore(float toAdd)
    {
        toAdd *= ScoreMultiplier;
        toAdd *= Mathf.Min(1 + (Time.time - TimeRoundStart) / (divideTimeDeltaBy * GameState.DeltaTimeCorrection), 10);
        score += (int)toAdd; //todo switch to float

        scoreAnimDirection = 1;
        scoreTextMat.SetFloat("_GlowPower", 1f);
        text.fontSize += scoreAnimSpeed * Time.deltaTime * GameState.DeltaTimeCorrection * -1;
    }

    public void subtractScore(float toSubtract)
    {
        scoreAnimDirection = -1;
        score -= toSubtract;
        scoreStartColor = scoreTextMat.GetColor("_GlowColor");
        scoreTextMat.SetFloat("_GlowPower", 1f);
        scoreTextMat.SetColor("_GlowColor", scoreDownColor);
        text.fontSize -= scoreAnimSpeed * Time.deltaTime * GameState.DeltaTimeCorrection * -1; // to trigger animation right now instead of waiting for Update()
    }

    public void MultiplyOnDef()
    {
        ScoreMultiplier += multiplyOnDef;
    }
}
