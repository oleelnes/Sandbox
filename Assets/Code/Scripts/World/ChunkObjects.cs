using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkObjects
{

    public List<GameObject> treeList;
    public List<GameObject> caveEntranceList;
	public List<GameObject> rockList;
	public List<GameObject> plantList;

	System.Random random;

	public bool visible;
	public WorldPopulated worldPopulated;

	MeshCollider meshCollider;
	GameObject proc = GameObject.FindGameObjectWithTag("MeshGenerator");

	int chunkSize;

	public ChunkObjects(MeshCollider collider, int chunkSize, bool visible)
    {
		this.meshCollider = collider;
		this.chunkSize = chunkSize;	
		this.visible = visible;
		worldPopulated = WorldPopulated.FALSE;
		random = new System.Random(123234512);


		treeList = new List<GameObject>();
		caveEntranceList = new List<GameObject>();
		rockList = new List<GameObject>();	
		plantList = new List<GameObject>();
		
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

    public void populateTerrainChunk(bool visible, GameObject meshObject, MeshData meshData, EndlessTerrain world, PopulateWithObjects objectPopulator)
    {
        if (visible)
        {
			populateWithCaveEntrances(meshObject, world, objectPopulator);
            populateWithTrees(20, 20, 50 * 50, meshObject, meshData, world, objectPopulator);
			populateWithRocks(meshObject, world, objectPopulator, meshData);
			populateWithPlants(meshObject, world, objectPopulator, meshData);
        }
    }

	private void populateWithRocks(GameObject meshObject, EndlessTerrain world, PopulateWithObjects objectPopulator, MeshData meshData)
    {
		//smallRock
		for (int i = 0; i < 100; i++)
        {
			float x = 0f + meshObject.transform.position.x + (float)random.Next(0, chunkSize);
			float z = 0f + meshObject.transform.position.z + (float)random.Next(0, chunkSize);

			float height = world.GetHeight(x, z);
			if(!IsWater(height - 0.2f, meshData))
            {
				rockList.Add(objectPopulator.createNewObject(new Vector3(x, height, z), "rock", 2.0f + (float)random.Next(0, 2), "rockOne"));
            }
        }

		for (int i = 0; i < random.Next(0, 3); i++)
        {
			float x = 0f + meshObject.transform.position.x + (float)random.Next(0, chunkSize);
			float z = 0f + meshObject.transform.position.z + (float)random.Next(0, chunkSize);

			float height = world.GetHeight(x, z);

			if (!IsWater(height - 0.2f, meshData))
			{
				rockList.Add(objectPopulator.createNewObject(new Vector3(x, height, z), "rock", 1.0f + (float)random.Next(0, 2), "rockBigOne"));
			}
		}
    }

	private void populateWithPlants(GameObject meshObject, EndlessTerrain world, PopulateWithObjects objectPopulator, MeshData meshData)
	{
		//grass
		addToList(plantList, meshObject, world, meshData, objectPopulator, "plant", "grassOne", 80, 120, 1);
		//flowers
		addToList(plantList, meshObject, world, meshData, objectPopulator, "plant", "flowerOne", 15, 30, 1);
		//mushroom
		addToList(plantList, meshObject, world, meshData, objectPopulator, "plant", "mushroomOne", 3, 10, 1);
	}

	private void addToList(List<GameObject> list, GameObject meshObject, EndlessTerrain world, MeshData meshData, PopulateWithObjects objectPopulator, 
		string type, string subType, int randomLow, int randomHigh, int scaleRandomHigh)
    {
		for (int i = 0; i < random.Next(randomLow, randomHigh); i++)
		{
			float x = 0f + meshObject.transform.position.x + (float)random.Next(0, chunkSize);
			float z = 0f + meshObject.transform.position.z + (float)random.Next(0, chunkSize);

			float height = world.GetHeight(x, z);

			if (!IsWater(height - 0.2f, meshData))
			{
				list.Add(objectPopulator.createNewObject(new Vector3(x, height, z), type, 1.0f + (float)random.Next(0, scaleRandomHigh), subType));
			}
		}
	}


	/// <summary>
	/// Function that changes the visibility of objects that are populated in the world.
	/// </summary>
	/// <param name="visible">The visibility of the objects.</param>
	void ObjectsVisible(Vector2 position, float distance)
	{
		VisibleObjectsInList(position, distance, treeList);
		VisibleObjectsInList(position, distance, caveEntranceList);
		VisibleObjectsInList(position, distance, rockList);
		VisibleObjectsInList(position, distance, plantList);
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
	void populateWithTrees(int xSize, int zSize, int amount, GameObject meshObject, MeshData meshData, EndlessTerrain world, PopulateWithObjects objectPopulator)
	{
		System.Random treePosRandom = new System.Random(121112);

		float density = 1.0f;

		if (!visible) return;

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
					GenerateNewTree(placementLocationX, height, placementLocationZ, objectPopulator);
				}

			}
		}
		if (treeList.Count > 4) objectPopulator.makeTreeVisible(treeList[2], false);

		//Setting the world populated enum to true.

	}

	void GenerateNewTree(float placementLocationX, float height, float placementLocationZ, PopulateWithObjects objectPopulator)
	{
		

		Vector3 vec = new Vector3(placementLocationX, height, placementLocationZ);

		int treeIndicator = random.Next(0, 100);
		string subType = "treeOne";
		float treeScale = 5f;

		//Change to switch-case?
		if (treeIndicator == 0)
		{
			subType = "treeOne";
			treeScale = 5f + random.Next(-2, 3);
		}
		else if (treeIndicator == 1)
		{
			subType = "treeTwo";
			treeScale = 2f + random.Next(-1, 4);
		}
		else if (treeIndicator > 1 && treeIndicator < 5)
		{
			subType = "treeThree";
			treeScale = 5f + random.Next(-1, 3);
		}
		else
		{
			subType = "treeFour";
			treeScale = 5f + random.Next(-1, 3);
		}
		GameObject newTree = objectPopulator.createNewObject(vec, "tree", treeScale, subType);
		newTree.SetActive(false);
		if (newTree != null) treeList.Add(newTree);
	}

	public void populateWithCaveEntrances(GameObject meshObject, EndlessTerrain world, PopulateWithObjects objectPopulator)
	{
		float height = world.GetHeight((float)(200) + meshObject.transform.position.x, (float)(200) + meshObject.transform.position.z);
		caveEntranceList.Add(objectPopulator.createNewObject(new Vector3((float)(200) + meshObject.transform.position.x, height,
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
