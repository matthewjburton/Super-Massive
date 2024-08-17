using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] AudioSource soundObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioClip audioClip, Transform spawnTransform, float pitch = 1f, float volume = 1f)
    {
        if (!IsInView(spawnTransform.position))
            return;

        if (!audioClip)
        {
            Debug.LogWarning("No audio clip assigned!");
            return;
        }

        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSound(AudioClip[] audioClips, Transform spawnTransform, float pitch = 1f, float volume = 1f)
    {
        if (!IsInView(spawnTransform.position))
            return;

        if (audioClips.Length == 0)
        {
            Debug.LogWarning("No audio clips in array!");
            return;
        }

        AudioClip audioClip = audioClips[Random.Range(0, audioClips.Length)];
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    bool IsInView(Vector3 position)
    {
        // Get the main camera
        Camera cam = Camera.main;

        // Convert the particle's position to viewport coordinates
        Vector3 viewportPoint = cam.WorldToViewportPoint(position);

        // Check if the particle is within the camera's view
        bool isInView = viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                        viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                        viewportPoint.z > 0; // Ensure the particle is in front of the camera

        return isInView;
    }
}
