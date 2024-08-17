using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CameraSettings settings;
    float largestMass;
    float targetOrthographicSize;
    Color targetBackgroundColor;

    void OnEnable()
    {
        EnvironmentManager.OnEnvironmentChanged += HandleEnvironmentChanged;
    }

    void OnDisable()
    {
        EnvironmentManager.OnEnvironmentChanged -= HandleEnvironmentChanged;
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
        GameObject largestParticle = ParticleManager.Instance.LargestParticle;
        if (!largestParticle)
            return;

        targetOrthographicSize = largestParticle.GetComponent<Particle>().mass * settings.sizeMultiplier + settings.minSize;

        // Smoothly transition to the target orthographic size
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetOrthographicSize, Time.deltaTime * settings.zoomSpeed);
    }

    void SetBackgroundColor()
    {
        // Smoothly transition to the target background color
        Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, targetBackgroundColor, Time.deltaTime * settings.backgroundSpeed);
    }
}
