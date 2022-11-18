using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{

    [SerializeField]
    public GameObject caveMan;
    [SerializeField]
    public GameObject houndMan;
    [SerializeField]
    public GameObject goblin;
    [SerializeField]
    public GameObject squidMan;

    private List<GameObject> enemyList;



    // Start is called before the first frame update
    void Start()
    {
        enemyList = new List<GameObject>();
        for(int i = 0; i < 5; i++)
        {
            float x = -38.6f + i;
            float z = -161.5f + i;
            Vector3 position = new Vector3(x, GetHeight(x, z), z);
            enemyList.Add(createNewObject(position, "CavemanObject", 1.0f));
        }
    }


    private float GetHeight (float x, float z)
    {
        RaycastHit hit;
        float hitHeight = 100.9f;
        Ray ray = new Ray(new Vector3(x, 10.0f, z), Vector3.down);

        if (!Physics.Raycast(ray, out hit, 50.0f)) return hit.point.y;
        if (Physics.Raycast(ray, out hit, 50.0f)) return hit.point.y;

        return hitHeight;
    }


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
        switch (objectName.ToLower())
        {
            case "cavemanobject":
                if(caveMan == null) return null;
                else  return Instantiate(caveMan);
            default:
                Debug.Log(objectName + " did not match any of the cases");
                return null;
        }
    }

  



}
