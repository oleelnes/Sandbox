using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateWithObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject treeObjectOne;
    [SerializeField]
    private GameObject treeObjectTwo;
    [SerializeField]
    private GameObject treeObjectThree;
    [SerializeField]
    private GameObject treeObjectFour;

    [SerializeField]
    private GameObject treeRoundTwo;
    [SerializeField]
    private GameObject treeRoundThree;

    //dungeon
    [SerializeField]
    private GameObject dungeonEntranceToClone;

    [SerializeField]
    private GameObject rockOne;
    [SerializeField]
    private GameObject rockBigOne;

    [SerializeField]
    private GameObject grassOne;
    [SerializeField]
    private GameObject flowerOne;
    [SerializeField]
    private GameObject mushroomOne;

    [SerializeField]
    private GameObject bushOne;
    [SerializeField]
    private GameObject bushTwo;
    [SerializeField]
    private GameObject bushThree;

    [SerializeField]
    private GameObject goblinEnemy;

    [SerializeField]
    private GameObject houndManEnemy;

    [SerializeField]
    private GameObject squidManEnemy;


    /// <summary>
    /// Function that returns a game object 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="objectName"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public GameObject createNewObject(Vector3 position, string objectName, float scale, string subType = null)
    {
        GameObject objectToCreate = InstantiateClone(objectName, subType);

        if (objectToCreate == null) return null;
        //child nodes have to be set to inactive in the hierarchy, and then activated here. If not, the position won't  
        //get changed
        for (int i = 0; i < objectToCreate.transform.childCount; i++)
            objectToCreate.transform.GetChild(i).gameObject.SetActive(true);
        
        objectToCreate.transform.localScale *= scale;
        objectToCreate.transform.position = position;

        objectToCreate.transform.parent = GameObject.Find(objectName).transform;
        return objectToCreate;
    }

    public GameObject InstantiateClone(string objectName, string subType = null)
    {
        switch(objectName.ToLower())
        {
            case "tree":
                return GetTreeType(subType);
            case "dungeonentrance":
                return Instantiate(dungeonEntranceToClone);
            case "rock":
                return GetRockType(subType);
            case "plant":
                return GetPlantType(subType);
            case "bush":
                return GetBushType(subType);
            case "enemy":
                return GetEnemyType(subType);
            default:
                Debug.Log(objectName + " did not match any of the cases");
                return null;
        }
    }

    private GameObject GetEnemyType(string subType)
    {
        if (subType == null) return Instantiate(squidManEnemy);
        switch (subType.ToLower())
        {
            case "goblin":
                return Instantiate(goblinEnemy);
            case "houndman":
                return Instantiate(houndManEnemy);
            case "squidman":
                return Instantiate(squidManEnemy);
            default:
                return Instantiate(squidManEnemy);
        }
    }

    public GameObject GetBushType(string subType)
    {
        if (subType == null) return null;
        switch(subType.ToLower())
        {
            case "bushone":
                return Instantiate(bushOne);
            case "bushtwo":
                return Instantiate(bushTwo);
            case "bushthree": 
                return Instantiate(bushThree);
            default:
                Debug.Log(subType + " did not match any of the cases");
                return null;
        }
    }

    public GameObject GetPlantType(string subType)
    {
        if (subType == null) return null;
        switch (subType.ToLower())
        {
            case "grassone":
                return Instantiate(grassOne);
            case "flowerone":
                return Instantiate(flowerOne);
            case "mushroomone":
                return Instantiate(mushroomOne);
            default:
                Debug.Log(subType + " did not match any of the cases");
                return null;
        }
    }

    public GameObject GetRockType(string subType)
    {
        if (subType == null) return null;
        switch (subType.ToLower())
        {
            case "rockone":
                return Instantiate(rockOne);
            case "rockbigone":
                return Instantiate(rockBigOne);
            default:
                Debug.Log(subType + " did not match any of the cases");
                return null;
        }
    }




    public GameObject GetTreeType(string subType)
    {
        if (subType == null) return null;
        switch(subType.ToLower())
        {
            case "treeone":
                return Instantiate(treeObjectOne);
            case "treetwo":
                return Instantiate(treeObjectTwo);
            case "treethree":
                return Instantiate(treeObjectThree);
            case "treefour":
                return Instantiate(treeObjectFour);
            case "treeroundtwo":
                return Instantiate(treeRoundTwo);
            case "treeroundthree":
                return Instantiate(treeRoundThree);
            default:
                Debug.Log(subType + " did not match any of the cases");
                return null;
        }
    }

    public void makeTreeVisible(GameObject tree, bool visibility)
    {
        tree.SetActive(visibility);
    }

}
