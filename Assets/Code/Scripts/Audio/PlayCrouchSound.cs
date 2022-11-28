using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCrouchSound : MonoBehaviour
{
    private AudioSource crouchAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        crouchAudioSource = GetComponent<AudioSource>();    
    }

    // Update is called once per frame
    // void Update()
    // {
    //     PlayCrouchAudio();
    // }

    public void PlayCrouchAudio()
    {
        // if (Input.GetKeyDown(Player.instance.movement.crouchKey))
        // {
        crouchAudioSource.Play();
        // }
    }
}
