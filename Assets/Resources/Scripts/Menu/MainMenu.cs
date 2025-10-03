using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] Slider MasterVolumeSlider;
    [SerializeField] Slider MusicVolumeSlider;
    [SerializeField] Slider AmbienceVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenOptionsMenu()
    {
        OptionsMenu.SetActive(true);
        MasterVolumeSlider.value = AudioManager.instance.masterVolume;
        MusicVolumeSlider.value = AudioManager.instance.musicVolume;
        AmbienceVolumeSlider.value = AudioManager.instance.ambienceVolume;
        SFXVolumeSlider.value = AudioManager.instance.SFXVolume;
    }

    public void CloseOptionsMenu()
    {
        OptionsMenu.SetActive(false);
    }

    public void SetMasterVolume(float volume)
    {
        AudioManager.instance.masterVolume = volume;
    }
    public void SetMusicVolume(float volume)
    {
        AudioManager.instance.musicVolume = volume;
    }
    public void SetAmbienceVolume(float volume)
    {
        AudioManager.instance.ambienceVolume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        AudioManager.instance.SFXVolume = volume;
    }
}
