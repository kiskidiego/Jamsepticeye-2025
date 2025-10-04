using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] Slider MasterVolumeSlider;
    [SerializeField] Slider MusicVolumeSlider;
    [SerializeField] Slider AmbienceVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;
    [SerializeField] EventReference _pressSound;
    public void OpenOptionsMenu()
    {
        MasterVolumeSlider.value = AudioManager.instance.masterVolume;
        MusicVolumeSlider.value = AudioManager.instance.musicVolume;
        AmbienceVolumeSlider.value = AudioManager.instance.ambienceVolume;
        SFXVolumeSlider.value = AudioManager.instance.SFXVolume;
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlayOneShot(_pressSound, transform.position);
    }

    public void MuteMasterVolume()
    {
        bool isMuted = AudioManager.instance.masterMuted;
        AudioManager.instance.masterMuted = !isMuted;
    }

    public void MuteMusicVolume()
    {
        bool isMuted = AudioManager.instance.musicMuted;
        AudioManager.instance.musicMuted = !isMuted;
    }

    public void MuteAmbienceVolume()
    {
        bool isMuted = AudioManager.instance.ambienceMuted;
        AudioManager.instance.ambienceMuted = !isMuted;
    }

    public void MuteSFXVolume()
    {
        bool isMuted = AudioManager.instance.SFXMuted;
        AudioManager.instance.SFXMuted = !isMuted;
    }

    public void SetMasterVolume()
    {
        AudioManager.instance.masterVolume = MasterVolumeSlider.value;
        AudioManager.instance.masterMuted = false;
    }
    public void SetMusicVolume()
    {
        AudioManager.instance.musicVolume = MusicVolumeSlider.value;
        AudioManager.instance.musicMuted = false;
    }
    public void SetAmbienceVolume()
    {
        AudioManager.instance.ambienceVolume = AmbienceVolumeSlider.value;
        AudioManager.instance.ambienceMuted = false;
    }
    public void SetSFXVolume()
    {
        AudioManager.instance.SFXVolume = SFXVolumeSlider.value;
        AudioManager.instance.SFXMuted = false;
    }
    
    public void CloseOptionsMenu()
    {
        gameObject.SetActive(false);
    }

}
