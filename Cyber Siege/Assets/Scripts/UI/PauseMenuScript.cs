using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    // OnClick Functions
    public void ContinueButtonOnClick()
    {
        UIManager.main.PauseMenuContinueButtonOnClick();
    }

    public void ExitButtonOnClick()
    {
        UIManager.main.PauseMenuExitLevelButtonOnClick();
    }

    // OnValueChanged Functions
    public void MasterVolumeOnValueChanged(float level)
    {
        SoundManager.main.SetMasterVolume(level);
    }

    public void SoundFXVolumeOnValueChanged(float level)
    {
        SoundManager.main.SetSoundFXVolume(level);
    }

    public void MusicVolumeOnValueChanged(float level)
    {
        SoundManager.main.SetMusicVolume(level);
    }
}
