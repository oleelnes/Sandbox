using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player instance { get; private set; }
    public PlayerStats stats;
    public PlayerMovement movement;

    public int woodInInventory;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            movement = gameObject.GetComponent<PlayerMovement>();
            stats = gameObject.GetComponent<PlayerStats>();
            instance = this;
            woodInInventory = 0;
        }
    }

    public Player GetInstance()
    {
        return instance;
    }
}