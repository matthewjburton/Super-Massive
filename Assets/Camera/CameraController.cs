using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CameraSettings settings;
    float largestMass;
    float targetOrthographicSize;
    Color targetBackgroundColor;

    void OnEnable()
    {
        Particle.OnMassChanged += HandleMassChanged;
        EnvironmentManager.OnEnvironmentChanged += HandleEnvironmentChanged;
    }

    void OnDisable()
    {
        Particle.OnMassChanged -= HandleMassChanged;
        EnvironmentManager.OnEnvironmentChanged -= HandleEnvironmentChanged;
    }

    void HandleMassChanged(float newMass)
    {
        // Update largest mass if necessary
        if (newMass > largestMass)
        {
            largestMass = newMass;

            // Calculate target orthographic size based on largest mass
            targetOrthographicSize = largestMass * settings.sizeMultiplier + settings.minSize;
        }
    }

    private void HandleEnvironmentChanged(Environment environment)
    {
        targetBackgroundColor = environment.backgroundColor;
    }

    void Update()
    {
        SetOrthographicSize();
        SetBackgroundColor();
    }

    void SetOrthographicSize()
    {
        // Smoothly transition to the target orthographic size
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetOrthographicSize, Time.deltaTime * settings.zoomSpeed);
    }

    void SetBackgroundColor()
    {
        // Smoothly transition to the target background color
        Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, targetBackgroundColor, Time.deltaTime * settings.backgroundSpeed);
    }
}
