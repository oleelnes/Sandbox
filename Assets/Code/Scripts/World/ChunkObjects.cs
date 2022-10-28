using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkObjects
{

    public List<GameObject> treeList;
    public List<GameObject> caveEntranceList;
	public bool trees;
	public WorldPopulated worldPopulated;

	MeshCollider meshCollider;
	GameObject proc = GameObject.FindGameObjectWithTag("MeshGenerator");

	int chunkSize;

	public ChunkObjects(MeshCollider collider, int chunkSize, bool trees)
    {
		this.meshCollider = collider;
		this.chunkSize = chunkSize;	
		this.trees = trees;
		worldPopulated = WorldPopulated.FALSE;


		treeList = new List<GameObject>();
		caveEntranceList = new List<GameObject>();
	}

    public void SetObjectsVisible(Vector2 position, bool objectsVisible)
    {
		//Debug.Log(treeList.Count);
        if (worldPopulated != WorldPopulated.FALSE)
        {
			if (!objectsVisible)
			{
				ObjectsVisible(position, 0f);
			}
			else
            {
				ObjectsVisible(position, 300f);

            }
        }
		
    }

    public void populateTerrainChunk(bool trees, GameObject meshObject, MeshData meshData, EndlessTerrain world, PopulateWithObjects treePopulator)
    {
        if (trees)
        {
			Debug.Log("here");

			populateWithCaveEntrances(meshObject, world, treePopulator);
            populateWithTrees(20, 20, 50 * 50, meshObject, meshData, world, treePopulator);

        }
       
        //if (rocks) populateWithRocks();
    }

	/// <summary>
	/// Function that changes the visibility of objects that are populated in the world.
	/// </summary>
	/// <param name="visible">The visibility of the objects.</param>
	void ObjectsVisible(Vector2 position, float distance)
	{
		VisibleObjectsInList(position, distance, treeList);
		VisibleObjectsInList(position, distance, caveEntranceList);
	}

	void VisibleObjectsInList(Vector2 position, float distance, List<GameObject> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			Vector3 vec3 = new Vector3(position.x, 0.0f, position.y);
			if (list[i].activeSelf && Vector3.Distance(vec3, list[i].transform.position) > distance)
			{
				list[i].SetActive(false);
			}
			else if (Vector3.Distance(vec3, list[i].transform.position) <= distance)
			{
				list[i].SetActive(true);
			}
		}
	}



	//TODO: move out of this file!
	void populateWithTrees(int xSize, int zSize, int amount, GameObject meshObject, MeshData meshData, EndlessTerrain world, PopulateWithObjects treePopulator)
	{
		System.Random treePosRandom = new System.Random(121112);

		float density = 1.0f;

		if (!trees) return;

		for (int x = 0; x < 300; x += Mathf.RoundToInt(10 * density))
		{
			for (int y = 0; y < 300; y += Mathf.RoundToInt(10 * density))
			{
				float internalX = x + treePosRandom.Next(-4, 4);
				float internalY = y + treePosRandom.Next(-4, 4);
				float placementLocationX = internalX + meshObject.transform.position.x;
				float placementLocationZ = internalY + meshObject.transform.position.z;
				float height = world.GetHeight(placementLocationX, placementLocationZ);

				if (!IsWater(height - 0.2f, meshData) && internalX < chunkSize && internalY < chunkSize)
				{
					GenerateNewTree(placementLocationX, height, placementLocationZ, treePopulator, treePosRandom);
				}

			}
		}
		if (treeList.Count > 4) treePopulator.makeTreeVisible(treeList[2], false);

		//Setting the world populated enum to true.

	}

	void GenerateNewTree(float placementLocationX, float height, float placementLocationZ, PopulateWithObjects treePopulator, System.Random treeRandom)
	{
		

		Vector3 vec = new Vector3(placementLocationX, height, placementLocationZ);

		int treeIndicator = treeRandom.Next(0, 100);
		string subType = "treeOne";
		float treeScale = 5f;

		//Change to switch-case?
		if (treeIndicator == 0)
		{
			subType = "treeOne";
			treeScale = 5f + treeRandom.Next(-2, 3);
		}
		else if (treeIndicator == 1)
		{
			subType = "treeTwo";
			treeScale = 2f + treeRandom.Next(-1, 4);
		}
		else if (treeIndicator > 1 && treeIndicator < 5)
		{
			subType = "treeThree";
			treeScale = 5f + treeRandom.Next(-1, 3);
		}
		else
		{
			subType = "treeFour";
			treeScale = 5f + treeRandom.Next(-1, 3);
		}
		Debug.Log("Subtype: " + subType);
		GameObject newTree = treePopulator.createNewObject(vec, "tree", treeScale, subType);
		newTree.SetActive(false);
		if (newTree != null) treeList.Add(newTree);
	}

	public void populateWithCaveEntrances(GameObject meshObject, EndlessTerrain world, PopulateWithObjects treePopulator)
	{
		float height = world.GetHeight((float)(200) + meshObject.transform.position.x, (float)(200) + meshObject.transform.position.z);
		caveEntranceList.Add(treePopulator.createNewObject(new Vector3((float)(200) + meshObject.transform.position.x, height,
			(float)(200) + meshObject.transform.position.z), "dungeonEntrance", 1.5f));
	}



	/// <summary>
	/// Handles the state of the world population.
	/// </summary>
	public enum WorldPopulated
	{
		TRUE,
		FALSE,
		HIDDEN
	}

	/// <summary>
	/// Function that gets the height (y coordinate) of a mesh given coordinates x and z.
	/// </summary>
	/// <param name="x"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	public float GetLocalHeight(float x, float z)
	{
		RaycastHit hit;
		Ray ray = new Ray(new Vector3(x, 100.0f, z), Vector3.down);

		if (!meshCollider.Raycast(ray, out hit, 150.0f)) return hit.point.y;

		return -999f;
	}

	/*public bool IsWater(float x, float z)
	{
		if (GetLocalHeight(x, z) >= meshData.getWaterLevel()) return false;
		return true;
	}*/

	public bool IsWater(float height, MeshData meshData)
	{
		if (height > meshData.getWaterLevel()) return false;
		return true;
	}

	public bool IsMountainTop(float x, float z)
	{
		return true;
	}

}
