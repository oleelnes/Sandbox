using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateWithTrees : MonoBehaviour
{

    public GameObject objectToClone;
    public GameObject parent;

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

    public GameObject createNewTree(Vector3 position)
    {
        GameObject newTree = Instantiate(objectToClone);
        newTree.transform.localScale *= 2.7f;
        newTree.transform.position = position;
        newTree.transform.parent = GameObject.Find("Trees").transform;
        //children.Add(newTree);
        return newTree;
    }

    public void makeTreeVisible(GameObject tree, bool visibility)
    {
        tree.SetActive(visibility);
    }
}
