using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float sizeMultiplier; // Multiplier for camera zoom based on largest mass
    [SerializeField] float minSize; // Minimum orthographic size to avoid excessive zooming
    [SerializeField] float zoomSpeed; // Speed of zoom transition

    float largestMass;
    float targetOrthographicSize;

    void OnEnable()
    {
        Particle.OnMassChanged += HandleMassChanged;
    }

    void OnDisable()
    {
        Particle.OnMassChanged -= HandleMassChanged;
    }

    void HandleMassChanged(float newMass)
    {
        // Update largest mass if necessary
        if (newMass > largestMass)
        {
            largestMass = newMass;

            // Calculate target orthographic size based on largest mass
            targetOrthographicSize = Mathf.Max(minSize, largestMass * sizeMultiplier);
        }
    }

    void Update()
    {
        SetOrthographicSize();
    }

    void SetOrthographicSize()
    {
        // Smoothly transition to the target orthographic size
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
    }
}
