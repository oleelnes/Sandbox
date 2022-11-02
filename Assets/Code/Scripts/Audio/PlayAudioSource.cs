using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioSource : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    /*    [SerializeField]
        private KeyCode keyCode;
        [SerializeField]
        private AudioClip audioClip;
        [SerializeField]
        private float volume = 1.0f;
    */

    private void Update()
    {
        PlayAudio();
    }
    public void PlayAudio()
    {
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }

    public void PlayAudioOnButtonDown(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            audioSource.Play();
        }
    }

    public void PlaySpecificAudioClipOnButtonDown(KeyCode key, AudioClip audioClip, float volume = 1.0f)
    {
        if (Input.GetKeyDown(key))
        {
            audioSource.PlayOneShot(audioClip, volume);
        }
    }

    public void PlaySpecificAudioClip(AudioClip audioClip, float volume = 1.0f)
    {
        audioSource.PlayOneShot(audioClip, volume);
    }
}
