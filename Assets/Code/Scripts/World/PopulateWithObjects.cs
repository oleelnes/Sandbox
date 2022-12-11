using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateWithObjects : MonoBehaviour
{

    public GameObject treeObjectOne;
    public GameObject treeObjectTwo;
    public GameObject treeObjectThree;
    public GameObject treeObjectFour;

    public GameObject treeRoundTwo;
    public GameObject treeRoundThree;

    //dungeon
    public GameObject dungeonEntranceToClone;

    public GameObject rockOne;
    public GameObject rockBigOne;


    public GameObject grassOne;
    public GameObject flowerOne;
    public GameObject mushroomOne;

    public GameObject bushOne;
    public GameObject bushTwo;
    public GameObject bushThree;


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
            default:
                Debug.Log(objectName + " did not match any of the cases");
                return null;
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
