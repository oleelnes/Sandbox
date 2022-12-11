using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AnimationAudioTrigger : MonoBehaviour
{
    public AudioClip idle_audioClip;
    public AudioClip scream_audioClip;
    public AudioClip chase_audioClip;
    public AudioClip attack_audioClip;
    public AudioClip pound_audioClip;
    public AudioClip death_audioClip;
    public AudioClip jump_audioClip;

    public float idleVolume = 0.5f;

    [SerializeField]
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        /*audioSource = GetComponent<AudioSource>();  */
    }

    public void IdleEvent()
    {
        audioSource.clip = idle_audioClip;
        audioSource.PlayOneShot(audioSource.clip, idleVolume);
    }

    public void ScreamEvent()
    {
        audioSource.clip = scream_audioClip;
        audioSource.PlayOneShot(audioSource.clip, 1.2f);
    }
    public void ChaseEvent()
    {
        audioSource.clip = chase_audioClip;
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void AttackEvent()
    {
        audioSource.clip = attack_audioClip;
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void PoundEvent()
    {
        audioSource.clip = pound_audioClip;
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void DeathEvent()
    {
        audioSource.clip = death_audioClip;
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void JumpEvent()
    {
        audioSource.clip = jump_audioClip;
        audioSource.pitch = 0.2f;
        audioSource.PlayOneShot(audioSource.clip, 1.5f);
        audioSource.pitch = 1.0f;
    }
}
