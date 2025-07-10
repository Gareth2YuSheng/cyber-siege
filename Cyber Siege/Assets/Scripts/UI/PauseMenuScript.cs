using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] protected Slider masterVolumeSlider;
    [SerializeField] protected Slider soundFXVolumeSlider;
    [SerializeField] protected Slider musicVolumeSlider;


    public void Start()
    {
        MasterVolumeOnValueChanged(DoNotDestroy.main.masterVolume);
        SoundFXVolumeOnValueChanged(DoNotDestroy.main.soundFXVolume);
        MusicVolumeOnValueChanged(DoNotDestroy.main.musicVolume);
    }
    public void ContinueButtonOnClick()
    {
        UIManager.main.PauseMenuContinueButtonOnClick();
    }

    public void RetryButtonOnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitButtonOnClick()
    {
        UIManager.main.PauseMenuExitLevelButtonOnClick();
    }

    // OnValueChanged Functions
    public void MasterVolumeOnValueChanged(float level)
    {
        masterVolumeSlider.value = level;
        SoundManager.main.SetMasterVolume(level);
        DoNotDestroy.main.masterVolume = level;
    }

    public void SoundFXVolumeOnValueChanged(float level)
    {
        soundFXVolumeSlider.value = level;
        SoundManager.main.SetSoundFXVolume(level);
        DoNotDestroy.main.soundFXVolume = level;

    }

    public void MusicVolumeOnValueChanged(float level)
    {
        musicVolumeSlider.value = level;
        SoundManager.main.SetMusicVolume(level);
        DoNotDestroy.main.musicVolume = level;
    }
}
