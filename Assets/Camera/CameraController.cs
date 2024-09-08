using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    [SerializeField] CameraSettings settings;
    float targetOrthographicSize;
    Color targetBackgroundColor;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        if (ParticleManager.Instance == null || ParticleManager.Instance.LargestParticle == null)
            return;

        GameObject largestParticle = ParticleManager.Instance.LargestParticle;

        targetOrthographicSize = largestParticle.GetComponent<Particle>().mass * settings.sizeMultiplier + settings.minSize;

        // Smoothly transition to the target orthographic size
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetOrthographicSize, Time.deltaTime * settings.zoomSpeed);
    }

    void SetBackgroundColor()
    {
        // Smoothly transition to the target background color
        Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, targetBackgroundColor, Time.deltaTime * settings.backgroundSpeed);
    }

    public int GetZoomStepCount()
    {
        // Calculate how many times the camera has zoomed out
        return Mathf.FloorToInt((Camera.main.orthographicSize - settings.minSize) / settings.sizeMultiplier);
    }
}
