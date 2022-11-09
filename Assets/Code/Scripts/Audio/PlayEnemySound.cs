using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEnemySound : MonoBehaviour
{

    [Header("EnemyState audio")]
    public AudioClip idle_AudioClip;
    public AudioClip chase_AudioClip;
    public AudioClip Attack_AudioClip;

    PlayAudioSource playAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        playAudioSource = gameObject.GetComponent<PlayAudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
