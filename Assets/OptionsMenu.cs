using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;

    void Start()
    {
        InitializeSliders();
    }

    void InitializeSliders()
    {
        if (masterSlider != null && audioMixer.GetFloat("Master Volume", out float masterVolume))
        {
            masterSlider.value = DecibelToLinear(masterVolume);
        }

        if (musicSlider != null && audioMixer.GetFloat("Music Volume", out float musicVolume))
        {
            musicSlider.value = DecibelToLinear(musicVolume);
        }

        if (soundSlider != null && audioMixer.GetFloat("Sound Volume", out float soundVolume))
        {
            soundSlider.value = DecibelToLinear(soundVolume);
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master Volume", LinearToDecibel(volume));
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music Volume", LinearToDecibel(volume));
    }

    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("Sound Volume", LinearToDecibel(volume));
    }

    private float DecibelToLinear(float decibels)
    {
        return Mathf.Pow(10, decibels / 20);
    }

    private float LinearToDecibel(float linear)
    {
        return Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20;
    }
}