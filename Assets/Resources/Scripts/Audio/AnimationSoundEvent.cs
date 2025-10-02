using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AnimationSoundEvent : MonoBehaviour
{
    public EventReference soundSFX;
    public void Play()
    {
        AudioManager.instance.PlayOneShot(soundSFX, transform.position);
    }
}
