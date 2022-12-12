using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public PlayerHealth playerHealth;
    List<HealthHeart> hearts = new List<HealthHeart>();

    private void Start()
    {
        DrawHearts();
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDamaged += DrawHearts;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDamaged -= DrawHearts;
    }

    /*
    * Method for drawing the right amounts of hearts, and the correct image 
    * corresponding to the players current health value. 
    */
    public void DrawHearts()
    {
        ClearHearts();

        // Determine how many hearts to make total by using a modulo to
        // see if the max health is odd or even. 
        // Add the remainder to see if you need an additional half heart.
        float maxHealthRemainder = playerHealth.maxHealth % 2;
        int heartsToMake = (int)(playerHealth.maxHealth / 2 + maxHealthRemainder);
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }


        // Use the current health value to get the correct image from the heartStatus enum.
        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(playerHealth.health - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }

    }

    /**
    * Utility function for creating empty hearts.
    * Instantiate the heart prefab and set the health bar as the parent.
    * Set the heart image as an empty heart and adding it to our list. 
    */
    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }



    /*
    * Utility function for removing all the hearts in the health bar.
    * Destroy all the heart game objects and clear the list.
    */
    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        hearts = new List<HealthHeart>();
    }


}
