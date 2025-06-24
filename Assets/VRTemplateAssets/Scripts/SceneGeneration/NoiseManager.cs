using System.Collections;
using UnityEngine;
using VRPresentationTrainer.Core.Data;

public class NoiseManager : MonoBehaviour
{
    public VRConfigSettings config;

    [Header("Nivel Bajo")]
    public AudioSource backgroundNoise;
    public AudioSource coughNoise;
    public float coughRepeatInterval = 40f;

    [Header("Nivel Medio")]
    public AudioSource environmentNoise;
    public AudioSource phoneNoise;
    public float phoneRepeatInterval = 60f;

    [Header("Nivel Alto")]
    public AudioSource peopleTalkingNoise;

    private Coroutine coughCoroutine;
    private Coroutine phoneCoroutine;

    void Start()
    {
        var level = config.environment.noiseLevel;

        // Desactiva todos por defecto
        if (backgroundNoise) backgroundNoise.gameObject.SetActive(false);
        if (coughNoise) coughNoise.gameObject.SetActive(false);
        if (environmentNoise) environmentNoise.gameObject.SetActive(false);
        if (phoneNoise) phoneNoise.gameObject.SetActive(false);
        if (peopleTalkingNoise) peopleTalkingNoise.gameObject.SetActive(false);

        switch (level)
        {
            case VRConfigSettings.EnvironmentSettings.NoiseLevel.Ninguno:
                // Nada
                break;
            case VRConfigSettings.EnvironmentSettings.NoiseLevel.Bajo:
                if (backgroundNoise)
                {
                    backgroundNoise.gameObject.SetActive(true);
                    backgroundNoise.Play();
                }
                if (coughNoise)
                {
                    coughNoise.gameObject.SetActive(true);
                    coughNoise.loop = false;
                    coughCoroutine = StartCoroutine(PlayCoughPeriodically());
                }
                break;

            case VRConfigSettings.EnvironmentSettings.NoiseLevel.Medio:
                if (backgroundNoise)
                {
                    backgroundNoise.gameObject.SetActive(true);
                    backgroundNoise.Play();
                }
                if (coughNoise)
                {
                    coughNoise.gameObject.SetActive(true);
                    coughNoise.loop = false;
                    coughCoroutine = StartCoroutine(PlayCoughPeriodically());
                }
                if (environmentNoise)
                {
                    environmentNoise.gameObject.SetActive(true);
                    environmentNoise.loop = true;
                    environmentNoise.Play();
                }
                if (phoneNoise)
                {
                    phoneNoise.gameObject.SetActive(true);
                    phoneNoise.loop = false;
                    phoneCoroutine = StartCoroutine(PlayPhonePeriodically());
                }
                break;

            case VRConfigSettings.EnvironmentSettings.NoiseLevel.Alto:
                if (backgroundNoise)
                {
                    backgroundNoise.gameObject.SetActive(true);
                    backgroundNoise.Play();
                }
                if (coughNoise)
                {
                    coughNoise.gameObject.SetActive(true);
                    coughNoise.loop = false;
                    coughCoroutine = StartCoroutine(PlayCoughPeriodically());
                }
                if (environmentNoise)
                {
                    environmentNoise.gameObject.SetActive(true);
                    environmentNoise.loop = true;
                    environmentNoise.Play();
                }
                if (phoneNoise)
                {
                    phoneNoise.gameObject.SetActive(true);
                    phoneNoise.loop = false;
                    phoneCoroutine = StartCoroutine(PlayPhonePeriodically());
                }
                if (peopleTalkingNoise)
                {
                    peopleTalkingNoise.gameObject.SetActive(true);
                    peopleTalkingNoise.loop = true;
                    peopleTalkingNoise.Play();
                }
                break;
        }
    }

    IEnumerator PlayPhonePeriodically()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {
            phoneNoise.Play();
            yield return new WaitForSeconds(phoneRepeatInterval);
        }
    }

    IEnumerator PlayCoughPeriodically()
    {
        yield return new WaitForSeconds(10f);

        while (true)
        {
            coughNoise.Play();
            yield return new WaitForSeconds(coughRepeatInterval);
        }
    }
}