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
}
