using System;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance;
    public Environment environment;
    public static event Action<Environment> OnEnvironmentChanged; // Event for environment change

    private float environmentDuration; // How long the environment lasts
    private float timer; // Tracks time

    [SerializeField] float initialMinimumDuration;
    [SerializeField] float initialMaximumDuration;

    [SerializeField] float minimumDuration;
    [SerializeField] float maximumDuration;

    [SerializeField] Environment[] environments; // Array of environments

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        environment = environments[0];

        environmentDuration = UnityEngine.Random.Range(initialMinimumDuration, initialMaximumDuration); // Example duration for the initial "Default" environment
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer <= environmentDuration)
            return;

        if (environment.environment == EnvironmentType.Default)
        {
            NewEnvironment();
        }
        else
        {
            DefaultEnvironment();
        }

        ResetTimer();
    }

    void DefaultEnvironment()
    {
        environment = environments[0];

        // Enable stars
        ParticleSystem starsParticleSystem = GameObject.Find("Stars").GetComponent<ParticleSystem>();
        var emission = starsParticleSystem.emission;
        emission.enabled = environment.showStars;
    }

    void NewEnvironment()
    {
        int randomIndex = UnityEngine.Random.Range(0, environments.Length - 1) + 1;
        environment = environments[randomIndex];

        if (environment.environment == EnvironmentType.Void)
        {
            // Disable stars
            ParticleSystem starsParticleSystem = GameObject.Find("Stars").GetComponent<ParticleSystem>();
            var emission = starsParticleSystem.emission;
            emission.enabled = false;
        }

        // Trigger the environment change event
        OnEnvironmentChanged?.Invoke(environment);

        // Set random durations for the next environment and cooldown
        environmentDuration = UnityEngine.Random.Range(minimumDuration, maximumDuration); // Duration the environment lasts
    }

    void ResetTimer()
    {
        timer = 0f;
    }
}
