
using System.Collections;
using TMPro;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI centerText;

    [Header("Intro")]
    [SerializeField] string[] introDialogue;
    [SerializeField] GameObject[] introPrefabs;
    [SerializeField] GameObject[] activateAfterIntro;

    [Header("Fusion")]
    bool fusionCustscenePlayed;
    [SerializeField] string[] fusionDialogue;
    [SerializeField] GameObject massSlider;

    [Header("Antimatter")]
    bool antiParticleCutscenePlayed;
    [SerializeField] string[] antimatterDialogue;

    [Tooltip("Time for text to fade in or out")]
    [SerializeField] float fadeDuration;
    [Tooltip("Time for text to stay at max alpha")]
    [SerializeField] float readDuration;
    [Tooltip("Time for superposition prefab to exist")]
    [SerializeField] float superPositionDuration;
    [Tooltip("Time for Big Bang prefab to exist")]
    [SerializeField] float bigBangDuration;

    [SerializeField] TextMeshProUGUI skipText;
    [SerializeField] bool skipIntroCutscene;

    void Start()
    {
        StartCoroutine(nameof(IntroCutscene));
    }

    void Update()
    {
        if (InputManager.Instance.SkipCutsceneInput)
        {
            skipIntroCutscene = true;
        }

        if (skipIntroCutscene)
        {
            StopCoroutine(nameof(IntroCutscene));

            centerText.gameObject.SetActive(false);
            skipText.gameObject.SetActive(false);

            GameObject superPosition = GameObject.Find("Super Position(Clone)");
            if (superPosition)
                Destroy(superPosition);

            GameObject bigBang = GameObject.Find("Big Bang(Clone)");
            if (bigBang)
                Destroy(bigBang);

            StartGame();
        }
    }

    IEnumerator IntroCutscene()
    {
        centerText.gameObject.SetActive(true);
        centerText.text = introDialogue[0];
        yield return StartCoroutine(FadeText(centerText, fadeDuration, 0, 1));

        skipText.gameObject.SetActive(true);
        yield return StartCoroutine(FadeText(skipText, fadeDuration, 0, .6f));

        yield return new WaitForSeconds(readDuration);
        yield return StartCoroutine(FadeText(centerText, fadeDuration, 1, 0));

        GameObject superPosition = Instantiate(introPrefabs[0], transform);
        yield return new WaitForSeconds(superPositionDuration);
        Destroy(superPosition);

        GameObject bigBang = Instantiate(introPrefabs[1], transform);
        yield return new WaitForSeconds(bigBangDuration);
        Destroy(bigBang);

        centerText.text = introDialogue[1];
        yield return StartCoroutine(FadeText(centerText, fadeDuration, 0, 1));
        yield return new WaitForSeconds(readDuration);
        yield return StartCoroutine(FadeText(centerText, fadeDuration, 1, 0));

        centerText.text = introDialogue[2];
        yield return StartCoroutine(FadeText(centerText, fadeDuration, 0, 1));
        yield return new WaitForSeconds(readDuration);
        yield return StartCoroutine(FadeText(centerText, fadeDuration, 1, 0));
        centerText.gameObject.SetActive(false);

        yield return StartCoroutine(FadeText(skipText, fadeDuration, .6f, 0));
        skipText.gameObject.SetActive(false);

        StartGame();
    }

    void StartGame()
    {
        centerText.text = "";
        centerText.gameObject.SetActive(true);

        foreach (GameObject objectToActivate in activateAfterIntro)
        {
            objectToActivate.SetActive(true);
        }

        Camera.main.GetComponent<CameraController>().enabled = true;

        skipIntroCutscene = false;
    }

    void HandleFusion()
    {
        if (fusionCustscenePlayed)
            return;

        StartCoroutine(nameof(FusionCutscene));
    }

    IEnumerator FusionCutscene()
    {
        fusionCustscenePlayed = true;

        centerText.gameObject.SetActive(true);
        centerText.text = fusionDialogue[0];
        yield return StartCoroutine(FadeText(centerText, fadeDuration, 0, 1));

        massSlider.SetActive(true);

        yield return new WaitForSeconds(readDuration);
        yield return StartCoroutine(FadeText(centerText, fadeDuration, 1, 0));
        centerText.gameObject.SetActive(false);

    }

    void HandleAntiParticleCreated()
    {
        if (antiParticleCutscenePlayed)
            return;

        StartCoroutine(nameof(AntiParticleCutscene));
    }

    IEnumerator AntiParticleCutscene()
    {
        antiParticleCutscenePlayed = true;

        centerText.gameObject.SetActive(true);
        centerText.text = antimatterDialogue[0];
        yield return StartCoroutine(FadeText(centerText, fadeDuration, 0, 1));
        yield return new WaitForSeconds(readDuration);
        yield return StartCoroutine(FadeText(centerText, fadeDuration, 1, 0));
        centerText.gameObject.SetActive(false);
    }

    IEnumerator FadeText(TextMeshProUGUI textMeshPro, float duration, float start, float end)
    {
        Color color = textMeshPro.color;
        color.a = 0f;
        textMeshPro.color = color;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(start, end, elapsedTime / duration);
            textMeshPro.color = color;
            yield return null;
        }

        // Ensure the final alpha is fully opaque
        color.a = end;
        textMeshPro.color = color;
    }

    void OnEnable()
    {
        Particle.OnFusion += HandleFusion;
        AntiParticle.OnAntiParticleCreated += HandleAntiParticleCreated;
    }

    void OnDisable()
    {
        Particle.OnFusion -= HandleFusion;
        AntiParticle.OnAntiParticleCreated -= HandleAntiParticleCreated;
    }
}
