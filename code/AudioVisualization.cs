using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Visualization
{
    LINE,
    RING,
    RINGWITHCENTER,
    DOUBLELINE,
    CIRCLE
}
public class AudioVisualization : MonoBehaviour
{
    [SerializeField] float mySens;
    private float minFreqVar = 1f;
    private float maxFreqVar = 2.5f; //todo: ehm... higher number, lower sens. Probnably 2 is best, but for now, more false positives are better than fewer false negatives
    private float totalSum = 0;
    private float averageSumSoFar = 0;
    private float sumCount = 0;
    private float difficulty;

    //Material for our LineRenderer
    public Material material;
    public Visualization visualizationMode;
    public Gradient[] Gradients;

    public Gradient colorGradient = new Gradient();
    public Gradient optionA = new Gradient();

    //If we are displaying a Ring we need a radius
    public float minimumRadius;
    public float bufferAreaSize;
    public float lineMultiplier;
    public float spectralFluxBufferSize;
    public float spectralFluxMultiplier;
    public float smoothingSpeed;
    public float maximumScale;
    public int segments;
    public float sampledPercentage;

    //LineRenderer Prefab
    public GameObject lineRendererPrefab;

    //Calculated Values
    private float averagePower;
    private float db;
    private float pitch;

    private AudioSource audioSource;
    private OnBeatEvent onBeat;
    private float[] samples;
    private float sampleRate;

    private float[] lineScales;
    private LineRenderer circleRenderer;
    private LineRenderer[] lines;
    private LineRenderer[] doubledLines;

    private Vector3[] circlePositions;

    private float currentRadius;
    private float currentColorValue;

    [Header("BeatDetection")]
    public int minBeatsForMainIdx = 10;
    public int historyLength = 1000;
    public int varianceHistoryLength = 10;
    public float fakeBeatTimer = 10f;
    public float sensitivityDecayOnSwitch = 0.25f;
    public float progressMultiplier = 1f;
    public float minBeatSensitivity = 2f;
    public float maxBeatSensitivity = 8f;
    public float startProgressMulti;
    public float endProgressMulti;
    public float initialDecayPerFrame;
    public float linearDecayMultiplier;

    public float initProgressMulti;
    private int switchCounter = -2;
    private float defenseOnBeatDecayGraceCounter;
    private Queue[] histories;
    private Queue[] freqVarianceHistories;
    private Queue<float>[] spectralFluxHistories;
    private int historyCounter = 0;
    private int spectralFluxHistoryCounter = 0;
    private Queue[] varianceHistories;
    private float[] spectrum;
    private float[] spectrumTotals;
    private float[] spectrumFreqVarianceTotals;
    private float[] spectrumAvgs;
    private float[] spectrumFreqVarianceAvgs;
    private float[] spectrumDeltas;
    private float[] spectrumTimeLastBeat;
    private float[] spectrumDeltaTimeLastBeat;
    private float[] spectrumTotalVariance;
    private float[] spectrumAvgVariance;
    private bool[] spectrumBeatCooldown;
    [SerializeField]//for debugging
    private float[] spectrumSensitivity;
    private float[] spectrumBeatsSoFar;
    private int spectrumItersSoFar = 0;
    [SerializeField]//for debugging
    private int spectrumIdxOfMainBeat = 0;
    [SerializeField] private int[] idxsAllowedAsMainBeat;
    private float minAvgVariance = 0;
    private int beatCount = 0;
    private float timeAwoke = 0f;
    [SerializeField]//for debugging
    private float decayPerFrame;
    bool beatCooldown = false;
    bool mainBeatCooldown = false;

    [SerializeField] private float defenseOnBeatDecayGrace;
    public bool linearDecay = false;
    public bool balancingDecay = true;
    public float beatCooldownValue;
    public float volumeMultiplier;
    public float audioOffset;
    public float AudioOffset { get => audioOffset; private set { audioOffset = value; } }


    private void Awake()
    {
        difficulty = myPlayerPrefs.GetMasterDifficulty();

        //Getting our Audiosource and setting up our samples,spectrum and samplerate
        audioSource = GetComponent<AudioSource>();

        audioSource.volume = audioSource.volume / volumeMultiplier;

        onBeat = GetComponent<OnBeatEvent>();
        sampleRate = AudioSettings.outputSampleRate;
        initProgressMulti = startProgressMulti;

        Init();

        switch (visualizationMode)
        {
            case Visualization.LINE:
                InitializeLine();
                break;

            case Visualization.RING:
                InitializeRing();
                break;

            case Visualization.RINGWITHCENTER:
                InitializeRing();
                break;

            case Visualization.DOUBLELINE:
                InitializeDoubleLine();
                break;

            case Visualization.CIRCLE:
                InitializeCircle();
                break;
        }
    }

    public void Init()
    {
        StopAllCoroutines();
        beatCooldown = false;
        mainBeatCooldown = false;
        if (difficulty == 1)
        {
            switchCounter++;
            startProgressMulti = Mathf.Max(initProgressMulti / 5, initProgressMulti - (sensitivityDecayOnSwitch * switchCounter));
            progressMultiplier = startProgressMulti;
        }
        else
        {
            progressMultiplier = 1f;
        }

        audioSource.timeSamples = (int)(44100 * audioOffset); // 44.1 kHz, more accurate than working with seconds. TODO: test with audioSource.time
        samples = new float[1024];
        spectrum = new float[1024];
        spectrumAvgs = new float[segments];
        spectrumFreqVarianceAvgs = new float[segments];
        spectrumDeltas = new float[segments];
        spectrumTotals = new float[segments];
        spectrumFreqVarianceTotals = new float[segments];
        spectrumTimeLastBeat = new float[segments];
        spectrumDeltaTimeLastBeat = new float[segments];
        spectrumSensitivity = new float[segments];
        spectrumTotalVariance = new float[segments];
        spectrumAvgVariance = new float[segments];
        spectrumBeatsSoFar = new float[segments];
        spectrumBeatCooldown = new bool[segments];

        timeAwoke = Time.time;
        for (int i = 0; i < spectrumAvgs.Length; i++)
        {
            spectrumAvgs[i] = 0;
            spectrumFreqVarianceAvgs[i] = 1;
            spectrumDeltas[i] = 0;
            spectrumTotals[i] = 0;
            spectrumFreqVarianceTotals[i] = 0;
            spectrumTimeLastBeat[i] = timeAwoke;
            spectrumDeltaTimeLastBeat[i] = 0;
            spectrumSensitivity[i] = 0;
            spectrumTotalVariance[i] = 0;
            spectrumAvgVariance[i] = 0;
            spectrumSensitivity[i] = maxBeatSensitivity;
            spectrumBeatCooldown[i] = false;

            minAvgVariance = 10f;
        }

        defenseOnBeatDecayGraceCounter = 0;
        historyCounter = 0;
        spectralFluxHistoryCounter = 0;
        histories = new Queue[segments - 1];
        freqVarianceHistories = new Queue[segments - 1];
        spectralFluxHistories = new Queue<float>[segments - 1];
        varianceHistories = new Queue[segments - 1];
        for (int i = 0; i < histories.Length; i++)
        {
            histories[i] = new Queue();
            freqVarianceHistories[i] = new Queue();
            spectralFluxHistories[i] = new Queue<float>();
            spectralFluxHistories[i].Enqueue(1);
        }
        for (int i = 0; i < varianceHistories.Length; i++)
        {
            varianceHistories[i] = new Queue();
            for (int j = 0; j < varianceHistoryLength; j++)
            {
                varianceHistories[i].Enqueue(0f);
            }
        }

        for (int i = 1; i < segments; i++)
        {
            StartCoroutine(MaxDeltaTime(i));
        }

        initialDecayPerFrame = (startProgressMulti / (audioSource.clip.length * 50f)) * ((startProgressMulti - endProgressMulti) / startProgressMulti); //expecting 50 fps (default of fixedUpdate). for lower fps, slower decay, which is fair.
        decayPerFrame = initialDecayPerFrame;

        //We need to instantiate differently depending on which 
        //visualization mode we choose
    }

    private void FixedUpdate()
    {
        if (linearDecay)
        {
            // decayPerFrame *= linearDecayMultiplier;
            if (difficulty == 1)
            {
                progressMultiplier -= decayPerFrame * audioSource.pitch;
                // todo: learn form this: the longer we go without any beat, the bigger the penalty on sensitivity will be. This wil (hopefully) have a gradual decay on consistent-beat songs and will make low-sensitivity songs work, accelerating the decaying process on them.
            }
        }
        AnalyzeAudio();
        CalculateLineScales();
    }

    private void Update()
    {
        //if (visualizationMode == Visualization.LINE)
        //{
        //    UpdateLine();
        //}
        //else if (visualizationMode == Visualization.RING)
        //{
        //    UpdateRing();
        //}
        //else if (visualizationMode == Visualization.RINGWITHCENTER)
        //{
        //    UpdateCenter();
        //    UpdateRing();
        //}
        //else if (visualizationMode == Visualization.DOUBLELINE)
        //{
        //    UpdateDoubleLine();
        //}
        //else if (visualizationMode == Visualization.CIRCLE)
        //{
        //    UpdateCircle();
        //}
    }

    private void InitializeLine()
    {
        Vector3 newCameraPos = Camera.main.transform.position;
        newCameraPos.x += 74f;
        newCameraPos.y += 20f;
        Camera.main.transform.position = newCameraPos;

        //First we need to initialize our Arrays, one for the scales
        //we are going to need to scale each line depending on the spectrum
        //value and the other one holds reference to all of our LineRenderers.
        lineScales = new float[segments];
        lines = new LineRenderer[lineScales.Length];

        //We are looping through our lineRenderers to instantiate
        //as many of them as we set as our segments.
        for (int i = 0; i < lines.Length; i++)
        {
            GameObject go = Instantiate(lineRendererPrefab);
            go.transform.position = Vector3.zero;

            LineRenderer line = go.GetComponent<LineRenderer>();
            line.sharedMaterial = material;

            line.positionCount = 2;
            line.useWorldSpace = true;
            lines[i] = line;
        }
    }

    private void InitializeDoubleLine()
    {

        Vector3 newCameraPos = Camera.main.transform.position;
        newCameraPos.x += 74f;
        newCameraPos.y -= 1f;
        Camera.main.transform.position = newCameraPos;

        //First we need to initialize our Arrays, one for the scales
        //we are going to need to scale each line depending on the spectrum
        //value and the other one holds reference to all of our LineRenderers.
        lineScales = new float[segments];
        lines = new LineRenderer[lineScales.Length];
        doubledLines = new LineRenderer[lineScales.Length];

        //We are looping through our lineRenderers to instantiate
        //as many of them as we set as our segments.
        for (int i = 0; i < lines.Length; i++)
        {
            GameObject go = Instantiate(lineRendererPrefab);
            GameObject doubledGo = Instantiate(lineRendererPrefab);

            go.transform.position = Vector3.zero;
            doubledGo.transform.position = Vector3.zero;

            LineRenderer line = go.GetComponent<LineRenderer>();
            line.sharedMaterial = material;

            LineRenderer doubledLine = doubledGo.GetComponent<LineRenderer>();
            doubledLine.sharedMaterial = material;

            line.positionCount = 2;
            line.useWorldSpace = true;

            doubledLine.positionCount = 2;
            doubledLine.useWorldSpace = true;

            lines[i] = line;
            doubledLines[i] = doubledLine;
        }
    }

    private void InitializeRing()
    {
        //First we need to initialize our Arrays, one for the scales
        //we are going to need to scale each line depending on the spectrum
        //value and the other one holds reference to all of our LineRenderers.
        lineScales = new float[segments];
        lines = new LineRenderer[lineScales.Length];

        //We are looping through our lineRenderers to instantiate
        //as many of them as we set as our segments amount plus one.
        for (int i = 0; i < lines.Length; i++)
        {
            GameObject go = Instantiate(lineRendererPrefab);
            go.transform.position = Vector3.zero;

            LineRenderer line = go.GetComponent<LineRenderer>();
            line.sharedMaterial = material;

            line.positionCount = 2;
            line.useWorldSpace = true;
            lines[i] = line;
        }

        currentRadius = minimumRadius;
    }

    private void InitializeCircle()
    {
        lineScales = new float[segments + 1];

        GameObject go = Instantiate(lineRendererPrefab);
        go.transform.position = Vector3.zero;

        circleRenderer = go.GetComponent<LineRenderer>();
        circleRenderer.sharedMaterial = material;

        circleRenderer.positionCount = segments + 1;
        circleRenderer.useWorldSpace = true;

        circleRenderer.startWidth = circleRenderer.endWidth = 0.5f;

        float x;
        float y;
        float z = 0f;
        circlePositions = new Vector3[segments + 1];
        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * minimumRadius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * minimumRadius;

            circleRenderer.SetPosition(i, new Vector3(x, y, z));
            circlePositions[i] = new Vector3(x, y, z);
            angle += (360f / segments);
        }

        circleRenderer.colorGradient = colorGradient;
    }

    private void UpdateLine()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].SetPosition(0, Vector3.right * i);
            lines[i].SetPosition(1, (Vector3.right * i) + Vector3.up * (bufferAreaSize + lineScales[i]));

            lines[i].startWidth = 1f;
            lines[i].endWidth = 1f;

            //Changing the color of the Material depending on the linescale
            lines[i].startColor = colorGradient.Evaluate(0);
            lines[i].endColor = colorGradient.Evaluate((lineScales[i] - 1) / (maximumScale - 1f));
        }

    }

    private void UpdateDoubleLine()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].SetPosition(0, Vector3.right * i);
            lines[i].SetPosition(1, (Vector3.right * i) + Vector3.up * (bufferAreaSize + lineScales[i]));

            doubledLines[i].SetPosition(0, Vector3.right * i);
            doubledLines[i].SetPosition(1, (Vector3.right * i) + Vector3.down * (bufferAreaSize + lineScales[i]));

            lines[i].startWidth = doubledLines[i].startWidth = 3f;
            lines[i].endWidth = doubledLines[i].endWidth = 3f;

            lines[i].startColor = doubledLines[i].startColor = colorGradient.Evaluate(0);
            lines[i].endColor = doubledLines[i].endColor = colorGradient.Evaluate((lineScales[i] - 1) / (maximumScale - 1f));

        }
    }

    private void UpdateRing()
    {

        for (int i = 0; i < lines.Length; i++)
        {
            float t = i / (lines.Length - 0f); // if you delete - 0f, it breaks. 0 > 0 confirmed lol (cause with 0f, it converts the result to float, but im not changing this ^^)
            float a = t * Mathf.PI * 2f;

            Vector2 direction = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
            //float currentRadius = minimumRadius;
            float maxRadius = (currentRadius + bufferAreaSize + lineScales[i]);

            lines[i].SetPosition(0, direction * currentRadius);
            lines[i].SetPosition(1, direction * maxRadius);

            //Calculating the spacing between two lines to avoid a weird shape
            lines[i].startWidth = Spacing(currentRadius);
            lines[i].endWidth = Spacing(maxRadius);

            //Changing the color of the Material depending on the linescale
            lines[i].startColor = colorGradient.Evaluate(0);
            lines[i].endColor = colorGradient.Evaluate((lineScales[i] - 1) / (maximumScale - 1f));
        }
    }

    private void UpdateCircle()
    {
        for (int i = 0; i < circlePositions.Length; i++)
        {
            float t = i / (segments - 2f);
            float a = t * Mathf.PI * 2f;

            Vector2 direction = new Vector2(Mathf.Cos(a), Mathf.Sin(a));

            float maxRadius = (minimumRadius + bufferAreaSize + lineScales[i]);

            Vector3 changedY = direction * maxRadius;

            if (i == circlePositions.Length - 1)
            {
                circleRenderer.SetPosition(i, circlePositions[0]);
            }
            else
            {
                circleRenderer.SetPosition(i, changedY);
            }

            circlePositions[i] = circleRenderer.GetPosition(i);

        }
        //material.SetFloat("a", maximumScale);
        //material.SetFloat("b", minimumRadius);
    }

    public void AddRadiusWidth()
    {
        //   colorGradient = optionC;
        // if (colorGradient == optionA)
        //{
        //     colorGradient = optionC;
        // } else
        // {
        //     colorGradient = optionA;
        // }
    }

    public IEnumerator Beat(int spectrumNo)
    {
        colorGradient = Gradients[spectrumNo];
        if (spectrumNo == spectrumIdxOfMainBeat)
        {
            mainBeatCooldown = true;
        }
        beatCooldown = true;
        onBeat.OnBeat(spectrumNo);
        yield return new WaitForSeconds(beatCooldownValue);
        beatCount++;
        if (spectrumNo == spectrumIdxOfMainBeat)
        {
            mainBeatCooldown = false;
        }
        beatCooldown = false;
        colorGradient = optionA;
    }

    public void HitBeat(int spectrumNo)
    {
        //if (spectrumNo == spectrumIdxOfMainBeat)
        StartCoroutine(Beat(spectrumNo));
    }

    public void UpdateCenter()
    {

    }

    public void ChangeColorOnBeat()
    {
        currentColorValue += .1f;

        if (currentColorValue > 1)
        {
            currentColorValue = 0;
        }
    }

    private float Spacing(float radius)
    {
        float c = 2 * Mathf.PI * radius;
        float n = lines.Length;
        return c / n;
    }

    private void AnalyzeAudio()
    {
        audioSource.GetOutputData(samples, 0); // this is done is less than a frame. How, I don't know...

        //Getting the average power by getting the sum of all the squared samples
        float sum = 0;

        for (int i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }

        averagePower = Mathf.Sqrt(sum / samples.Length);

        //Getting the DB Value
        db = 20 * Mathf.Log10(averagePower * 0.1f);

        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);



        ////Getting the pitch
        //float maxV = 0;
        //int maxN = 0;

        //for (int i = 0; i < samples.Length; i++)
        //{
        //    if (!(spectrum[i] > maxV) || !(spectrum[i] > .0f))
        //    {
        //        continue;
        //    }

        //    maxV = spectrum[i];
        //    maxN = i;
        //}

        //float frequenceN = maxN;
        //if (maxN > 0 && maxN < samples.Length - 1)
        //{
        //    float dl = spectrum[maxN - 1] / spectrum[maxN];
        //    float dr = spectrum[maxN + 1] / spectrum[maxN];
        //    frequenceN += .5f * (dr * dr - dl * dl);
        //}

        //pitch = frequenceN * (sampleRate / 2) / samples.Length;
        //Debug.Log(pitch);

        //if (pitch > 500f)
        //{
        //    HitBeat();
        //}

    }

    private void CalculateLineScales()
    {
        int index = 1; // set to 0 to also apply to the first segment
        int spectralIndex = 0;
        float maxDelta = 0;
        int idxOfMaxDelta = 0;
        //int averageSize = 1;
        int averageSize = (int)Mathf.Abs(samples.Length * sampledPercentage); //length of spectrum that will be analyzed; each step is about 23Hz, since the size chosen for the spectrum is 1024.
        averageSize /= segments;
        spectralIndex = averageSize; // ignores the first segment
        if (averageSize < 1)
        {
            averageSize = 1;
        }
        spectrumItersSoFar++;
        if (historyCounter < historyLength)
            historyCounter++;
        if (spectralFluxHistoryCounter < spectralFluxBufferSize)
            spectralFluxHistoryCounter++;

        while (index < segments)
        {
            int i = 0;
            float sum = 0;

            while (i < averageSize)
            {
                sum += spectrum[spectralIndex];
                spectralIndex++;

                i++;
            }


            sum *= lineMultiplier;
            sum *= volumeMultiplier;
            var spectralFluxDelta = sum / spectralFluxHistories[index - 1].Average();
            //if(spectralFluxDelta > 2)
            //Debug.Log(spectralFluxDelta);
            sum *= Mathf.Max(1f, spectralFluxDelta) * spectralFluxMultiplier;

            if (spectralFluxHistoryCounter == spectralFluxBufferSize)
            {
                spectralFluxHistories[index - 1].Dequeue();
            }
            if (historyCounter == historyLength)
            {
                float dequed = (float)histories[index - 1].Dequeue();
                spectrumTotals[index] -= dequed;

                float freqVarianceDequed = (float)freqVarianceHistories[index - 1].Dequeue();
                spectrumFreqVarianceTotals[index] -= freqVarianceDequed; //dequeue last recorded onset, considering its deviation to the average at it's recording time

            }
            float sumFreqVariance = Mathf.Clamp(sum / spectrumAvgs[index], 1f, 100f);
            spectralFluxHistories[index - 1].Enqueue(sum);
            histories[index - 1].Enqueue(sum);
            freqVarianceHistories[index - 1].Enqueue(sumFreqVariance);
            spectrumTotals[index] += sum;
            spectrumAvgs[index] = spectrumTotals[index] / historyCounter;
            spectrumFreqVarianceTotals[index] += sumFreqVariance;
            spectrumFreqVarianceAvgs[index] = spectrumFreqVarianceTotals[index] / historyCounter;
            // if(spectrumFreqVarianceAvgs[index] > 2.5f)
            //Debug.Log(spectrumFreqVarianceAvgs[index]);
            spectrumDeltas[index] = sum / spectrumAvgs[index]; // delta <-> freqVariance
            sum /= averageSize;
            float yScale = sum;
            lineScales[index] -= Time.deltaTime * smoothingSpeed;

            //Debug.Log(spectrumFreqVarianceAvgs[index]);
            //if (index == 5)
            //Debug.Log("---");

            if (lineScales[index] < yScale)
            {
                lineScales[index] = yScale;
            }

            if (lineScales[index] > maximumScale)
            {
                lineScales[index] = maximumScale;
            }
            index++;
        }
        if (spectrumDeltas[spectrumIdxOfMainBeat] > spectrumSensitivity[spectrumIdxOfMainBeat])
        {
            maxDelta = spectrumDeltas[spectrumIdxOfMainBeat];
            idxOfMaxDelta = spectrumIdxOfMainBeat;
        }
        else
        {
            while (maxDelta < spectrumSensitivity[idxOfMaxDelta])
            {
                maxDelta = spectrumDeltas.Max();
                if (maxDelta == 0)
                    break;
                idxOfMaxDelta = System.Array.IndexOf(spectrumDeltas, maxDelta);
                spectrumDeltas[idxOfMaxDelta] = 0;
            }
        }
        if (!beatCooldown || (idxOfMaxDelta == spectrumIdxOfMainBeat && !mainBeatCooldown)) // trueBeat overrides cooldown // wtf, why?
        {
            if (balancingDecay)
            {
                if (maxDelta > 0)
                {
                    ProcessBeat(true, idxOfMaxDelta);
                    HitBeat(idxOfMaxDelta);
                }
            }
            // Debug.Log(maxDelta);
            // Debug.Log(spectrumAvgs[System.Array.IndexOf(spectrumDeltas, maxDelta)]);
        }

        // if(linearDecay && progressMultiplier > minBeatSensitivity)
        //{
        //    progressMultiplier -= decayPerFrame;
        //}

        // Debug.Log(Mathf.Sign(0));

        //for (int i = 0; i < spectrumSensitivity.Length; i++)
        //{
        //    Debug.Log("i: " + i + "   sens: " + spectrumAvgVariance[i]);
        //}
        //Debug.Log(spectrumIdxOfMainBeat + " main");

        //totalSum += currFrameSum;
        //sumCount++;
        //averageSumSoFar = totalSum / sumCount;

        //if (currFrameSum > 2.5f * averageSumSoFar)
        //{
        //    HitBeat();
        //}
    }

    private IEnumerator MaxDeltaTime(int spectrumIdx)
    {
        float toWait = fakeBeatTimer * progressMultiplier;
        yield return new WaitForSeconds(toWait);
        if (Time.time - spectrumTimeLastBeat[spectrumIdx] >= toWait)
        {
            ProcessBeat(false, spectrumIdx);
        }
    }


    private float MapFreqVarianceToSensitivity(float freqVar)
    {
        return (freqVar - minFreqVar) / (maxFreqVar - minFreqVar) * (maxBeatSensitivity - minBeatSensitivity) + minBeatSensitivity;
    }

    private void ProcessBeat(bool trueBeat, int idx)
    {
        float theTime = Time.time;
        float deltaTimeSinceLastBeat = theTime - spectrumTimeLastBeat[idx];
        if (!trueBeat)
            deltaTimeSinceLastBeat *= 2; // penalty for fakeBeat
        spectrumBeatsSoFar[idx]++;
        spectrumSensitivity[idx] = Mathf.Clamp(MapFreqVarianceToSensitivity(spectrumFreqVarianceAvgs[idx] * progressMultiplier),
                                               minBeatSensitivity, maxBeatSensitivity);
        //spectrumSensitivity[idx] = mySens;
        //if (idx == spectrumIdxOfMainBeat)
        //    spectrumSensitivity[idx] = Mathf.Max(spectrumSensitivity[idx] / 2, minBeatSensitivity);

        // spectrumSensitivity[idx] = Mathf.Clamp(maxDeltaTime / 2 * Mathf.Max(1f, spectrumFreqVarianceAvgs[idx]), minBeatSensitivity, maxBeatSensitivity); // for variance < 1 => lower avg freq => fewer onsets (take sens as min sens)
        // spectrumSensitivity[idx] = Mathf.Clamp(spectrumFreqVarianceAvgs[idx] *2 , minBeatSensitivity, maxBeatSensitivity);
        if (trueBeat) // x seconds grace period for determining beat
        {
            float variance = Mathf.Abs(spectrumDeltaTimeLastBeat[idx] - deltaTimeSinceLastBeat);

            float dequed = (float)varianceHistories[idx - 1].Dequeue();
            spectrumTotalVariance[idx] -= dequed;

            varianceHistories[idx - 1].Enqueue(variance);
            spectrumTotalVariance[idx] += variance;

            spectrumAvgVariance[idx] = spectrumTotalVariance[idx] / varianceHistoryLength;

            if (spectrumBeatsSoFar[idx] > minBeatsForMainIdx)
            {
                if (spectrumAvgVariance[idx] < minAvgVariance && idxsAllowedAsMainBeat.Contains(idx)) // idx 1 -> special atk, idx 5 -> dash
                {
                    minAvgVariance = spectrumAvgVariance[idx];
                    spectrumIdxOfMainBeat = idx;
                }
                // spectrumIdxOfMainBeat = System.Array.IndexOf(spectrumAvgVariance, spectrumAvgVariance.Min());

            }
        }

        spectrumDeltaTimeLastBeat[idx] = deltaTimeSinceLastBeat;
        spectrumTimeLastBeat[idx] = theTime;

        StartCoroutine(MaxDeltaTime(idx));
    }

    public void CallStopLinearDecay()
    {
        StartCoroutine(StopLinearDecay());
    }

    private IEnumerator StopLinearDecay()
    {
        defenseOnBeatDecayGraceCounter++;
        linearDecay = false;
        yield return new WaitForSeconds(defenseOnBeatDecayGrace);
        defenseOnBeatDecayGraceCounter--;
        if (defenseOnBeatDecayGraceCounter == 0)
            linearDecay = true;
    }



}
