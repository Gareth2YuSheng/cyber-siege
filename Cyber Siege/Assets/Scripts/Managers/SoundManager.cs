using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager main;
    [Header("References")]
    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        main = this;
    }

    // Mixer Functions
    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", level);
    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", level);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", level);
    }

    // FX Functions
    public void PlaySoundFXClip(AudioClip audioClip, float volume)
    {
        // Spawn in gameObject
        // AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        AudioSource audioSource = Instantiate(soundFXObject);

        // Assign Clip
        audioSource.clip = audioClip;

        // Assign volume
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        // Get length of sound FX clip
        float clipLength = audioSource.clip.length;

        // Destroy clip
        Destroy(audioSource.gameObject, clipLength);
    }
}
