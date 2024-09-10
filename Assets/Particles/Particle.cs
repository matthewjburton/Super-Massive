using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Particle : MonoBehaviour
{
    public ParticleStats stats;

    public int fusions;

    public float speed;

    public bool invincible;

    protected Rigidbody2D rb;

    public static event Action<int> OnFusionsChanged;
    public static event Action OnFusion;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        UpdateSize();
        SetInitialRotation(transform.position);
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (rb == null)
            return;

        rb.velocity = transform.right.normalized * GetSpeed();
    }

    protected abstract void OnTriggerEnter2D(Collider2D other);

    protected void Fuse(GameObject other)
    {
        SoundManager.Instance.PlayRandomSound(stats.fusionSounds, transform, UnityEngine.Random.Range(1 / (fusions + 1), 1));
        Handheld.Vibrate();

        SetAverageRotation(other);
        Destroy(other);
        Grow();

        OnFusion?.Invoke();
    }

    public void Fission(int fissions)
    {
        StartCoroutine(nameof(HandleInvicibilty));
        Shrink(fissions);
    }

    public void Grow()
    {
        fusions++;
        UpdateSize();
    }

    void Shrink(int fissions)
    {
        if (fusions < fissions)
        {
            Destroy(gameObject);
            return;
        }

        fusions -= fissions;
        UpdateSize();
    }

    IEnumerator HandleInvicibilty()
    {
        invincible = true;
        yield return new WaitForSeconds(stats.invincibilityDuration);
        invincible = false;
    }

    void UpdateSize()
    {
        float size = Mathf.Pow(stats.sizeMultiplier, fusions);
        Vector2 newSize = new(size, size);
        transform.localScale = newSize;
    }

    // Sets the initial rotation based on a direction to the camera
    void SetInitialRotation(Vector3 initialPosition)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector2 directionToCamera = (cameraPosition - initialPosition).normalized;

        // Generate a random angle within the specified range
        float angle = UnityEngine.Random.Range(-stats.maxAngleDeviation, stats.maxAngleDeviation);

        // Rotate the particle to face the direction
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector2 randomDirection = rotation * directionToCamera;

        // Set the rotation of the particle to face the new direction
        float angleToRotate = Mathf.Atan2(randomDirection.y, randomDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleToRotate);
    }

    void SetAverageRotation(GameObject other)
    {
        // Get the rotation of the current particle and the other particle
        Quaternion currentRotation = transform.rotation;
        Quaternion otherRotation = other.transform.rotation;

        // Calculate the average rotation
        Quaternion averageRotation = Quaternion.Slerp(currentRotation, otherRotation, 0.5f);

        // Set the remaining particle's rotation to the average rotation
        transform.rotation = averageRotation;
    }

    protected virtual float GetSpeed()
    {
        return stats.defaultSpeed / (fusions + 1) * EnvironmentManager.Instance.environment.speedMultiplier;
    }
}
