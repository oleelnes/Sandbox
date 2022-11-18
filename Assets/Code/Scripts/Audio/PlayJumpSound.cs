using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayJumpSound : MonoBehaviour
{

    private AudioSource jumpAudioSource;
    public AudioClip[] jump_start;
    public AudioClip[] jump_end;
/*    public AudioClip jump_start;
    public AudioClip jump_end;*/
    public int jumpVolume = 5;

    private bool onPlatformLastFrame = false;

    // Start is called before the first frame update
    void Start()
    {
        jumpAudioSource = GetComponent<AudioSource>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.instance.movement.grounded == true && onPlatformLastFrame == false)
        {
            jumpAudioSource.PlayOneShot(jump_end[Random.Range(0, jump_end.Length)], jumpVolume);
        }
        onPlatformLastFrame = Player.instance.movement.grounded;
        
    }

    public void playJumpAudio()
    {
        // onPlatform = Physics2D.OverlapCircle(platformChecker.position, platformCheckRadius, whatIsPlatform);*/

        jumpAudioSource.PlayOneShot(jump_start[Random.Range(0, jump_start.Length)], jumpVolume);   
    }
}
