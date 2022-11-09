using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        SpawnCharacter();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnCharacter()
    {
        player.transform.position = spawnPoint.transform.position;
    }

}
