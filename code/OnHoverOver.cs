using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnHoverOver : MonoBehaviour
{
    [SerializeField] GameObject[] toPull;
    [SerializeField] GameObject[] dontDestroyOnLoad;
    [SerializeField] GameObject distort;
    [SerializeField] GameObject hole;
    [SerializeField] GameObject swirl;
    [SerializeField] GameObject menuAudio;
    [SerializeField] SettingsController settingsController;
    [SerializeField] GameObject DeleteFrom;

    Material distortMat;
    SpriteRenderer holeRenderer;
    ParticleSystem particles;

    Color originalColor;
    [SerializeField] Color overColor;

    ParticleSystem.MinMaxGradient originalGrad;
    ParticleSystem.MinMaxGradient overGrad;
    ParticleSystem.RotationOverLifetimeModule rotation;

    [Header("onClick")]
    private bool clicked = false;
    [SerializeField] float diffuseSpeed;
    [SerializeField] float pullSpeed;

    void Start()
    {
        distortMat = distort.GetComponent<MeshRenderer>().material;
        particles = swirl.GetComponent<ParticleSystem>();
        holeRenderer = hole.GetComponent<SpriteRenderer>();
        originalColor = holeRenderer.color;

        Color color1 = overColor;
        color1.r -= (float)55 / 255;
        Color color2 = overColor;
        color2.g -= (float)55 / 255;

        overGrad = new ParticleSystem.MinMaxGradient(color1, color2);
        overGrad.mode = ParticleSystemGradientMode.RandomColor;

        color1 = originalColor;
        color1.b -= (float)55 / 255;
        color2 = originalColor;
        color2.g -= (float)55 / 255;
        originalGrad = new ParticleSystem.MinMaxGradient(color1, color2);
        originalGrad.mode = ParticleSystemGradientMode.RandomColor;

        rotation = particles.rotationOverLifetime;
    }

    void FixedUpdate()
    {
        if(clicked)
        {
            hole.transform.localScale /= diffuseSpeed;
            distortMat.SetFloat("_DistortionAmount", distortMat.GetFloat("_DistortionAmount") / diffuseSpeed);
            distortMat.SetFloat("_Alpha", distortMat.GetFloat("_Alpha") / diffuseSpeed);
            ChangeParticleSystemSize();

            foreach(GameObject pullThis in toPull)
            {
                pullThis.transform.localScale = new Vector3(pullThis.transform.localScale.x / diffuseSpeed, pullThis.transform.localScale.y / diffuseSpeed, pullThis.transform.localScale.z);
                float step = pullSpeed * Time.deltaTime;
                pullThis.transform.position = Vector3.MoveTowards(pullThis.transform.position, transform.position, step);
            }

            if(hole.transform.localScale.x < 0.01f)
            {
                Destroy(gameObject);
                foreach(GameObject dontDestroy in dontDestroyOnLoad)
                {
                    Destroy(dontDestroy);
                }
            }
        }
    }

    void OnMouseEnter()
    {
        if (!clicked)
        {
            holeRenderer.color = overColor;
            //   distortMat.SetFloat("_RotationAmount", distortMat.GetFloat("_RotationAmount") * 2f);
            ChangeParticleSystem(overGrad, true);
        }
    }

    void OnMouseExit()
    {
        if(!clicked)
        {
            holeRenderer.color = originalColor;
            //  distortMat.SetFloat("_RotationAmount", distortMat.GetFloat("_RotationAmount") / 2f);
            ChangeParticleSystem(originalGrad, false);
        }
    }

    void OnMouseDown()
    {
        if(!clicked)
        {
            clicked = true;
            var emission = particles.emission;
            emission.rateOverTime = 0;
            if(DeleteFrom != null)
                DeleteFrom.SetActive(false);
            foreach (GameObject pullThis in toPull)
            {
                textAlphaFade text = pullThis.GetComponent<textAlphaFade>();
                if (text)
                {
                    text.FadeObject(false, 1f);
                }
            }

            //if (settingsController)
            //    settingsController.Save(); // handled in slider OnChange event

            DontDestroyOnLoad(gameObject);
            foreach (GameObject dontDestroy in dontDestroyOnLoad)
            {
                DontDestroyOnLoad(dontDestroy);
            }
            DontDestroyOnLoad(menuAudio);
            menuAudio.GetComponent<MenuAudioHandler>().Fade();

            FindObjectOfType<GameState>().LastSceneIdx = SceneManager.GetActiveScene().buildIndex;
            FindObjectOfType<SceneLoader>().LoadPlaySceneAsync();
        }
    }

    private void ChangeParticleSystem(ParticleSystem.MinMaxGradient gradient, bool speedUp)
    {
        var main = particles.main;
        main.startColor = new ParticleSystem.MinMaxGradient(gradient.colorMin, gradient.colorMax);

        if (speedUp)
        {
            rotation.z = new ParticleSystem.MinMaxCurve(1f, 3f);
        }
        else
        {
            rotation.z = new ParticleSystem.MinMaxCurve(0.5f, 1.5f);
        }

        int particlesCount = particles.particleCount;
        ParticleSystem.Particle[] particlesArr = new ParticleSystem.Particle[particlesCount];
        particles.GetParticles(particlesArr);
        for (int i = 0; i < particlesArr.Length; i++)
        {
            var randColor = Color.Lerp(gradient.colorMin, gradient.colorMax, Random.value);
            particlesArr[i].startColor = randColor;

            if(speedUp) {
                particlesArr[i].angularVelocity *= 2;
            }
            else
            {
                particlesArr[i].angularVelocity /= 2;
            }
        }
        particles.SetParticles(particlesArr, particlesCount);
    }

    private void ChangeParticleSystemSize() // slow, but don't need speed in it's context
    {
        int particlesCount = particles.particleCount;
        ParticleSystem.Particle[] particlesArr = new ParticleSystem.Particle[particlesCount];
        particles.GetParticles(particlesArr);
        for (int i = 0; i < particlesArr.Length; i++)
        {
            particlesArr[i].startSize /= diffuseSpeed;
        }
        particles.SetParticles(particlesArr, particlesCount);
    }
}
