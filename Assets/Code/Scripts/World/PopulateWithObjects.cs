using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateWithObjects : MonoBehaviour
{

    public GameObject treeObjectToClone;
    public GameObject caveEntranceObjectToClone;
    //public GameObject parent;

    EndlessTerrain world;

    List<GameObject> children;


    // Start is called before the first frame update
    void Start()
    {
        children = new List<GameObject>();
       /* for (int x = 0; x < 3; x++)
        {
            for(int y = 0; y < 3; y++)
            {
                //float height = world.GetHeight(10 + x * 5, 10 + y * 5).HasValue ? world.GetHeight(10 + x * 5, 10 + y * 5).Value : -1000f;
                float height = world.GetHeight(10 + x * 5, 10 + y * 5);
                Debug.Log("retreived height:" + height);
                if (height > -100f)
                {
                    createNewTree(new Vector3(10 + x * 5, height, 10 + y * 5));
                }
            }  
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        /*Destroy(children[2]);
        makeTreeVisible(3, false);*/
    }

    public GameObject createNewObject(Vector3 position, string objectName)
    {
        //GameObject objectToCreate = Instantiate(treeObjectToClone);
        GameObject objectToCreate = InstantiateClone(objectName);
        objectToCreate.transform.localScale *= 2.7f;
        objectToCreate.transform.position = position;
        objectToCreate.transform.parent = GameObject.Find(objectName).transform;
        //children.Add(newTree);
        return objectToCreate;
    }

    public GameObject InstantiateClone(string objectName)
    {
        switch(objectName.ToLower())
        {
            case "tree":
                return Instantiate(treeObjectToClone);
            case "caveentrance":
                return Instantiate(caveEntranceObjectToClone);
            default:
                Debug.Log(objectName + " did not match any of the cases");
                return null;
        }
    }

    public void makeTreeVisible(GameObject tree, bool visibility)
    {
        tree.SetActive(visibility);
    }
}
