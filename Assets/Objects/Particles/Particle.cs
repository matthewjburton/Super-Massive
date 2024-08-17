using System;
using System.Collections;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float mass;
    public float speed;
    [SerializeField] float growthModifer;
    [SerializeField] float lerpDuration = 1f; // Duration to lerp to the new speed
    [SerializeField] AudioClip[] fusionSounds;
    [SerializeField] float maxAngleDeviation = 45f; // Angle in degrees


    private float targetSpeed;
    private Coroutine speedLerpCoroutine;
    private Rigidbody2D rb;

    public static event Action<float> OnMassChanged; // Event for mass change

    void Start()
    {
        mass = transform.localScale.x;
        OnMassChanged?.Invoke(mass);

        rb = GetComponent<Rigidbody2D>();

        rb.mass = mass;

        Vector2 direction = GetRandomDirectionTowardsCamera(transform.position);
        rb.velocity = direction * speed;

        // Subscribe to environment change event
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
        float angle = UnityEngine.Random.Range(-maxAngleDeviation, maxAngleDeviation);
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
        if (rb != null)
        {
            // Calculate the new velocity based on the current speed and direction
            Vector2 currentVelocity = rb.velocity;
            float currentSpeed = currentVelocity.magnitude;
            if (currentSpeed > 0)
            {
                // Maintain direction and apply new speed
                Vector2 newVelocity = currentVelocity.normalized * speed;
                rb.velocity = newVelocity;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Particle particle))
        {
            if (particle.mass.Equals(mass))
            {
                CombineParticles(other.gameObject);
            }
        }
    }

    void CombineParticles(GameObject other)
    {
        Destroy(other);

        SoundManager.Instance.PlayRandomSound(fusionSounds, transform, UnityEngine.Random.Range(Math.Abs((1 - mass) / 1), 1));

        transform.localScale *= growthModifer;
        mass = transform.localScale.x;
        speed /= mass;

        rb.mass = mass;

        // Notify listeners about the mass change
        OnMassChanged?.Invoke(mass);
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
            default:
                targetSpeed = Mathf.Clamp(speed, 1f, 10f); // Reset to a default range if needed
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

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            speed = Mathf.Lerp(initialSpeed, newSpeed, elapsedTime / lerpDuration);
            yield return null;
        }

        speed = newSpeed; // Ensure speed is exactly the target speed at the end
    }
}
