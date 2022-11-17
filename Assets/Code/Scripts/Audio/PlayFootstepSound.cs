using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayFootstepSound : MonoBehaviour
{
    private AudioSource audioSource;
    private bool IsMoving;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (Player.instance.movement.grounded)
        {
            playWalkAudio();
            playSprintAudio();
        }
        else
        {
            audioSource.Stop();
        }

    }

    private void playWalkAudio()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) IsMoving = true; // better use != 0 here for both directions
        else IsMoving = false;

        if (IsMoving && !audioSource.isPlaying) 
        {
            audioSource.Play();
        }// if player is moving and audiosource is not playing play it
        if (!IsMoving)
        {
            audioSource.Stop(); // if player is not moving and audiosource is playing stop
        }

    }

    private void playSprintAudio()
    {
        if (Player.instance.movement.isSprinting)
        {
            audioSource.pitch = 1.5f;
        }
        else if (Player.instance.movement.isSuperSprinting)
        {
            audioSource.pitch = 1.65f;
        }
        else
        {
            audioSource.pitch = 1f;
        }

    }
}
