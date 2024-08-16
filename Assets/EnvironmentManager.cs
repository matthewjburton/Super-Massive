using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance;
    public Environment currentEnvironment;
    private float environmentDuration; // How long the environment lasts
    private float cooldownDuration; // Time before changing to the next environment
    private float timer; // Tracks time
    private Camera mainCamera;
    [SerializeField] Color defaultColor;
    [SerializeField] Color voidColor;
    [SerializeField] Color warmColor;

    public enum Environment
    {
        Default,
        Warm,
        Void
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        currentEnvironment = Environment.Default;

        environmentDuration = Random.Range(5f, 10f); // Example duration for the initial "None" environment
        cooldownDuration = 0f; // Start with no cooldown
        timer = 0f;

        mainCamera = Camera.main; // Get reference to the main camera
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (currentEnvironment == Environment.Default && timer >= environmentDuration)
        {
            // After the initial "None" environment duration, switch to a random environment
            NewEnvironment();
            ResetTimer();
        }
        else if (timer >= environmentDuration + cooldownDuration)
        {
            // When the environment duration and cooldown have passed, switch environments
            currentEnvironment = Environment.Default; // Reset to None before choosing a new environment

            // Enter Default environment
            ParticleSystem starsParticleSystem = GameObject.Find("Stars").GetComponent<ParticleSystem>();
            var emission = starsParticleSystem.emission;
            emission.enabled = true; // Enable stars emission

            ResetTimer();
        }

        // Lerp camera background color based on current environment
        LerpBackgroundColor();
    }

    void NewEnvironment()
    {
        if (Random.Range(0, 2) == 0)
        {
            // Enter Void environment
            ParticleSystem starsParticleSystem = GameObject.Find("Stars").GetComponent<ParticleSystem>();
            var emission = starsParticleSystem.emission;
            emission.enabled = false; // Disable stars emission
            currentEnvironment = Environment.Void;
        }
        else
        {
            // Enter Warm environment
            currentEnvironment = Environment.Warm;
            // Example: Speed up particles or other effects for Warm environment
        }

        // Set random durations for the next environment and cooldown
        environmentDuration = Random.Range(10f, 20f); // Duration the environment lasts
        cooldownDuration = Random.Range(5f, 10f); // Cooldown before changing again
    }

    void LerpBackgroundColor()
    {
        if (currentEnvironment == Environment.Default)
        {
            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, defaultColor, Time.deltaTime);
        }
        if (currentEnvironment == Environment.Warm)
        {
            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, warmColor, Time.deltaTime);
        }
        if (currentEnvironment == Environment.Void)
        {
            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, voidColor, Time.deltaTime);
        }
    }

    void ResetTimer()
    {
        timer = 0f;
    }

}
