using UnityEngine;

public class SoundFXManager : MonoBehaviour
{

    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

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
