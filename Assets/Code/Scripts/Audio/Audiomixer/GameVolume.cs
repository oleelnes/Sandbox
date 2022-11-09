using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SocialPlatforms;

public class GameVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer gameAudioMixer;

    [Range(-50, 20)][SerializeField] private float masterVolume = 0;
    [Range(-50, 20)][SerializeField] private float hostilesVolume = 0;
    [Range(-50, 20)][SerializeField] private float effectsVolume = 0;
    [Range(-50, 20)][SerializeField] private float musicVolume = 0;
    [Range(-50, 20)][SerializeField] private float ambientVolume = 0;

    private void Update()
    {

        gameAudioMixer.SetFloat("MasterVolume", masterVolume);
        gameAudioMixer.SetFloat("HostilesVolume", hostilesVolume);
        gameAudioMixer.SetFloat("EffectsVolume", effectsVolume);
        gameAudioMixer.SetFloat("MusicVolume", musicVolume);
        gameAudioMixer.SetFloat("AmbientVolume", ambientVolume);

    }
}
