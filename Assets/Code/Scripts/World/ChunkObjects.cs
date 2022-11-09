using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkObjects
{

    public List<GameObject> treeList;
    public List<GameObject> caveEntranceList;
	public List<GameObject> rockList;
	public List<GameObject> plantList;
	public List<GameObject> chestList; 

	public List<Vector3> objectPositions; //not in use yet. TODO!

	System.Random random;

	public bool visible;
	public WorldPopulated worldPopulated;

	private Vector3[] forest;

	MeshCollider meshCollider;
	GameObject proc = GameObject.FindGameObjectWithTag("MeshGenerator");
	EndlessTerrain world;

	int chunkSize;

	public ChunkObjects(MeshCollider collider, int chunkSize, bool visible, int randomOffset, Vector3[] forest, EndlessTerrain world)
    {
		this.meshCollider = collider;
		this.chunkSize = chunkSize;	
		this.visible = visible;
		this.forest = forest;
		this.world = world;
		worldPopulated = WorldPopulated.FALSE;
		random = new System.Random(1002 + randomOffset);

		treeList = new List<GameObject>();
		caveEntranceList = new List<GameObject>();
		rockList = new List<GameObject>();	
		plantList = new List<GameObject>();
		chestList = new List<GameObject>(); 
		
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

		populateWithCaveEntrances(meshObject, world, objectPopulator);
		populateWithTrees(meshObject, meshData, world, objectPopulator);
		populateWithRocks(meshObject, world, objectPopulator, meshData);
		populateWithPlants(meshObject, world, objectPopulator, meshData);
		populateWithChest(meshObject, world, objectPopulator, meshData); 

	}

	private void populateWithChest(GameObject meshObject, EndlessTerrain world, PopulateWithObjects objectPopulator, MeshData meshData) {
		float x = 0f + meshObject.transform.position.x + (float)random.Next(0, chunkSize);
		float z = 0f + meshObject.transform.position.z + (float)random.Next(0, chunkSize);
		float height = world.GetHeight(x, z); 

		if(!IsWater(height, meshData)) chestList.Add(objectPopulator.createNewObject(new Vector3(x, height, z), "chest", 1.0f));
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
	/// Calls a function that sets the visibility of each object in a given list depending on the distance between
	/// the player and the object.
	/// </summary>
	/// <param name="position">The position of the player.</param>
	/// <param name="distance">The render distance.</param>
	void ObjectsVisible(Vector2 position, float distance)
	{
		VisibleObjectsInList(position, distance, treeList);
		VisibleObjectsInList(position, distance, caveEntranceList);
		VisibleObjectsInList(position, distance, rockList);
		VisibleObjectsInList(position, distance, plantList);
		VisibleObjectsInList(position, distance, chestList);
	}

	/// <summary>
	/// Iterates through all the objects in a list and sets the visibility of objects within the render distance to
	/// true.
	/// </summary>
	/// <param name="position">The position of the player.</param>
	/// <param name="distance">The render distance.</param>
	/// <param name="list">The list through which the function will iterate.</param>
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
			if (list[i].tag == "deleteTree" || list[i] == null)
			{
				list[i].SetActive(false);
				world.toDelete.Add(list[i]);
				list.RemoveAt(i);

			}

		}

	}

	/// <summary>
	/// Function that fills the treeList with trees depending on the noise map generated for the forest, along
	/// with some element of random. It will not place a tree in water.
	/// </summary>
	/// <param name="meshObject"></param>
	/// <param name="meshData"></param>
	/// <param name="world"></param>
	/// <param name="objectPopulator"></param>
	void populateWithTrees(GameObject meshObject, MeshData meshData, EndlessTerrain world, PopulateWithObjects objectPopulator)
	{
		
		for (int x = 0, i = 0; x < chunkSize; x += 10)
		{
			for (int y = 0; y < chunkSize; y += 10)
			{
				//Addition of the random makes the edges of the forests have wider spacing
				if(forest[i].y > 0.6f + ((float)random.NextDouble() % 0.15))
                {
					int treeInterval = Mathf.RoundToInt(forest[i].y * 12);
					int randomNumber = random.Next(0, 10);

					//The higher the noise value in the forest noisemap, the greater the probability that a tree will appear
					bool placeTree = randomNumber + Mathf.RoundToInt(forest[i].y * 10) < 14;

					float internalX = (float)x + (float)random.Next(-treeInterval, treeInterval);
					float internalY = (float)y + (float)random.Next(-treeInterval, treeInterval);
					float placementLocationX = internalX + meshObject.transform.position.x;
					float placementLocationZ = internalY + meshObject.transform.position.z;
					float height = world.GetHeight(placementLocationX, placementLocationZ);

					if (/*placeTree &&*/ !IsWater(height - 0.6f, meshData) && internalX < (chunkSize ) && internalY < (chunkSize ) )
					{
						GenerateNewTree(placementLocationX, height - 0.1f, placementLocationZ, objectPopulator);
					}
				}
				i ++;
			}	
		}
	}

	/// <summary>
	/// Generates a single tree of a random type, and adds it to the treeList.
	/// </summary>
	/// <param name="placementLocationX"></param>
	/// <param name="height"></param>
	/// <param name="placementLocationZ"></param>
	/// <param name="objectPopulator"></param>
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

	/// <summary>
	/// TODO: update function.
	/// This function populates the world with cave entrances.
	/// </summary>
	/// <param name="meshObject"></param>
	/// <param name="world"></param>
	/// <param name="objectPopulator"></param>
	public void populateWithCaveEntrances(GameObject meshObject, EndlessTerrain world, PopulateWithObjects objectPopulator)
	{
		float height = world.GetHeight((float)(200) + meshObject.transform.position.x, (float)(200) + meshObject.transform.position.z);
		caveEntranceList.Add(objectPopulator.createNewObject(new Vector3((float)(200) + meshObject.transform.position.x, height,
			(float)(200) + meshObject.transform.position.z), "dungeonEntrance", 1.5f));
	}



	/// <summary>
	/// Handles the state of the world population. Hidden state is depracated but I have not dared to delete it yet.
	/// </summary>
	public enum WorldPopulated
	{
		TRUE,
		FALSE,
		HIDDEN
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
