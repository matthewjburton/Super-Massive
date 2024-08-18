using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AntiParticle : MonoBehaviour
{
    public AntiParticleStats stats;
    public float mass;
    public float speed;
    private float targetSpeed;
    private Coroutine speedLerpCoroutine;
    private Rigidbody2D rb;

    public static event Action<float> OnMassChanged; // Event for mass change
    public static event Action OnAntiParticleCreated; // Event for mass change

    void Start()
    {
        OnAntiParticleCreated?.Invoke();

        rb = GetComponent<Rigidbody2D>();

        mass = stats.defaultMass;
        transform.localScale = new(mass, mass);

        speed = stats.defaultSpeed * EnvironmentManager.Instance.environment.speedMultiplier; ;
        Vector2 direction = GetRandomDirectionTowardsCamera(transform.position);
        rb.velocity = direction * speed;

        EnvironmentManager.OnEnvironmentChanged += OnEnvironmentChanged;
    }

    void OnDestroy()
    {
        // Unsubscribe from environment change event to avoid memory leaks
        EnvironmentManager.OnEnvironmentChanged -= OnEnvironmentChanged;
    }

    Vector2 GetRandomDirectionTowardsCamera(Vector3 spawnPosition)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector2 directionToCamera = (cameraPosition - spawnPosition).normalized;

        // Generate a random angle within the specified range
        float angle = UnityEngine.Random.Range(-stats.maxAngleDeviation, stats.maxAngleDeviation);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Rotate the direction vector by the random angle
        Vector2 randomDirection = rotation * directionToCamera;

        return randomDirection.normalized;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (rb == null)
            return;

        rb.velocity = rb.velocity.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Particle particle))
        {
            if (particle.mass <= mass)
            {
                DestroyParticles(other.gameObject);
            }
            else
            {
                ReduceParticle(particle);
            }
        }

        if (other.gameObject.TryGetComponent(out AntiParticle antiParticle))
        {
            if (antiParticle.mass.Equals(mass))
            {
                CombineParticles(other.gameObject);
            }
        }
    }

    void CombineParticles(GameObject other)
    {
        Destroy(other);

        SoundManager.Instance.PlayRandomSound(stats.fusionSounds, transform, UnityEngine.Random.Range(Math.Abs((1 - mass) / 1), 1));

        transform.localScale *= stats.growthMultiplier;
        mass = transform.localScale.x;
        speed *= stats.speedMultiplier;

        rb.mass = mass;

        // Notify listeners about the mass change
        OnMassChanged?.Invoke(mass);
    }

    void DestroyParticles(GameObject other)
    {
        if (other.GetComponent<Particle>().invincible)
            return;

        Destroy(other);

        SoundManager.Instance.PlayRandomSound(stats.destroySounds, transform, UnityEngine.Random.Range(Math.Abs((1 - other.GetComponent<Particle>().mass) / 1), 1));
        ScreenShake.Instance.Shake(.1f, 0.1f);

        // Notify listeners about the mass change
        OnMassChanged?.Invoke(mass);

        Instantiate(stats.destroyParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void ReduceParticle(Particle particle)
    {
        if (particle.invincible)
            return;

        particle.Reduce(CalculateCombinations());

        SoundManager.Instance.PlayRandomSound(stats.reduceSounds, transform, UnityEngine.Random.Range(Math.Abs((1 - particle.mass) / 1), 1));
        ScreenShake.Instance.Shake(.1f, 0.1f);

        // Notify listeners about the mass change
        OnMassChanged?.Invoke(mass);

        Instantiate(stats.destroyParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    float CalculateCombinations()
    {
        if (Mathf.Log(stats.growthMultiplier) == 0)
        {
            Debug.Log("Cant Divide by zero");
            return 1;
        }

        float numberOfCombinations = Mathf.Log(mass / stats.defaultMass) / Mathf.Log(stats.growthMultiplier) + 1;

        if (numberOfCombinations == 0)
        {
            Debug.Log("Cant return zero");
            return 1;
        }
        return numberOfCombinations;
    }


    void OnEnvironmentChanged(Environment environment)
    {
        switch (environment.environment)
        {
            case EnvironmentType.Warm:
                targetSpeed = speed * environment.speedMultiplier; // Example: Increase speed in Warm environment
                break;
            case EnvironmentType.Void:
                targetSpeed = speed * environment.speedMultiplier; // Example: Decrease speed in Void environment
                break;
            case EnvironmentType.Default:
                targetSpeed = speed * environment.speedMultiplier; // Reset to a default range if needed
                break;
            default:
                Debug.Log("Environment not found!");
                break;
        }

        // Start lerping to the new speed
        if (speedLerpCoroutine != null)
        {
            StopCoroutine(speedLerpCoroutine);
        }
        speedLerpCoroutine = StartCoroutine(LerpSpeed(targetSpeed));
    }

    IEnumerator LerpSpeed(float newSpeed)
    {
        float elapsedTime = 0f;
        float initialSpeed = speed;

        while (elapsedTime < stats.lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            speed = Mathf.Lerp(initialSpeed, newSpeed, elapsedTime / stats.lerpDuration);
            yield return null;
        }

        speed = newSpeed; // Ensure speed is exactly the target speed at the end
    }
}
