using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkObjects
{

    public List<GameObject> treeList;
    public List<GameObject> caveEntranceList;
	public List<GameObject> rockList;
	public List<GameObject> plantList;

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

		populateWithCaveEntrances(meshObject, world, objectPopulator, meshData);
		populateWithTrees(meshObject, meshData, world, objectPopulator);
		populateWithRocks(meshObject, world, objectPopulator, meshData);
		populateWithPlants(meshObject, world, objectPopulator, meshData);

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
		addToList(plantList, meshObject, world, meshData, objectPopulator, "plant", "grassOne", 400, 600, 1, 0.2f);
		//flowers
		addToList(plantList, meshObject, world, meshData, objectPopulator, "plant", "flowerOne", 50, 100, 1);
		//mushroom
		addToList(plantList, meshObject, world, meshData, objectPopulator, "plant", "mushroomOne", 20, 60, 2);

		addToList(plantList, meshObject, world, meshData, objectPopulator, "bush", "bushOne", 50, 100, 5, 4, 0.3f, 0.6f);

		addToList(plantList, meshObject, world, meshData, objectPopulator, "bush", "bushTwo", 20, 50, 5, 4, 0.3f, 0.6f);

		addToList(plantList, meshObject, world, meshData, objectPopulator, "bush", "bushThree", 20, 50, 5, 4, 0.3f, 0.5f);
	}

	private void addToList(List<GameObject> list, GameObject meshObject, EndlessTerrain world, MeshData meshData, PopulateWithObjects objectPopulator,
		string type, string subType, int randomLow, int randomHigh, int scaleRandomHigh, float scaleStart = 1.0f, float forestHeightStart = -1f
		, float forestHeightEnd = -1f)
    {
		for (int i = 0; i < random.Next(randomLow, randomHigh); i++)
		{
			float randX = (float)random.Next(0, chunkSize);
			float randZ = (float)random.Next(0, chunkSize);

			float x = 0f + meshObject.transform.position.x + randX;
			float z = 0f + meshObject.transform.position.z + randZ;

			float height = world.GetHeight(x, z);

			if (!IsWater(height - 0.5f, meshData))
			{
				if(forestHeightStart < 0) list
						.Add(objectPopulator.createNewObject(new Vector3(x, height, z), 
						type, scaleStart + (float)random.Next(0, scaleRandomHigh), subType));
				else if(forest[(int)((randZ / 5) * (chunkSize / 5) + (randX / 5))].y > forestHeightStart 
					&& forest[(int)((randZ / 5) * (chunkSize / 5) + (randX / 5))].y < forestHeightEnd)
                {
					list.Add(objectPopulator.createNewObject(new Vector3(x, height, z),
						type, scaleStart + (float)random.Next(0, scaleRandomHigh), subType));
				}
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

				int treeInterval = Mathf.RoundToInt(forest[i].y * 12);
				int randomNumber = random.Next(0, 10);

				//The higher the noise value in the forest noisemap, the greater the probability that a tree will appear
				bool placeTree = randomNumber + Mathf.RoundToInt(forest[i].y * 10) < 14;

				float internalX = (float)x + (float)random.Next(-treeInterval, treeInterval);
				float internalY = (float)y + (float)random.Next(-treeInterval, treeInterval);
				float placementLocationX = internalX + meshObject.transform.position.x;
				float placementLocationZ = internalY + meshObject.transform.position.z;
				float height = world.GetHeight(placementLocationX, placementLocationZ);
				//Addition of the random makes the edges of the forests have wider spacing
				if (forest[i].y > 0.6f + ((float)random.NextDouble() % 0.15))
                {
					if (placeTree && !IsWater(height - 0.5f, meshData) && height > 0.5f && height < 30 && internalX < (chunkSize ) && internalY < (chunkSize ) )
					{
						if (forest[i].y > 0.75f && forest[i].y < 0.85f)
                        {
							string treeTypeRandom = (random.Next(0, 2) == 1) ? "treeRoundTwo" : "treeRoundThree";
							GenerateNewTree(placementLocationX, height - 0.1f, placementLocationZ, objectPopulator, treeTypeRandom);
                        }
						else GenerateNewTree(placementLocationX, height - 0.1f, placementLocationZ, objectPopulator);
					}
				}

				i++;
				
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
	void GenerateNewTree(float placementLocationX, float height, float placementLocationZ, PopulateWithObjects objectPopulator, string defaultTreeType = "")
	{
		Vector3 vec = new Vector3(placementLocationX, height, placementLocationZ);


		int treeIndicator = random.Next(0, 100);
		string subType = decideTree(treeIndicator);
		float treeScale = (defaultTreeType == "") ? getScale(subType) : getScale(defaultTreeType);
		
		GameObject newTree = objectPopulator.createNewObject(vec, "tree", treeScale, (defaultTreeType == "") ? subType : defaultTreeType);
		newTree.SetActive(false);
		if (newTree != null) treeList.Add(newTree);
	}


	public string  decideTree(int treeIndicator)
    {
		if (treeIndicator == 0) return "treeOne";
		else if (treeIndicator == 1) return "treeTwo";
		else if (treeIndicator > 1 && treeIndicator < 5) return "treeThree";
		else return "treeFour";
	}


	private float getScale(string tree)
    {
		switch(tree)
        {
			case "treeOne":
				return 5f + random.Next(-2, 3);
			case "treeTwo":
				return 5f + random.Next(-1, 4);
			case "treeThree":
				return 5f + random.Next(-1, 3);
			case "treeFour":
				return  5f + random.Next(-1, 3);
			case "treeRoundTwo":
				return (random.Next(0, 30) == 1) ? 8.5f : 4f + (float)random.NextDouble() % 2.0f;
			case "treeRoundThree":
				return (random.Next(0, 30) == 1) ? 9f : 4f + (float)random.NextDouble() % 2.0f;
			default: return 3f;
		}
    }

	/// <summary>
	///  This function populates the world with cave entrances.
	/// </summary>
	/// <param name="meshObject"></param>
	/// <param name="world"></param>
	/// <param name="objectPopulator"></param>
	/// <param name="meshData"></param>
	public void populateWithCaveEntrances(GameObject meshObject, EndlessTerrain world, PopulateWithObjects objectPopulator, MeshData meshData)
	{
		float x = 0f;
		float z = 0f;
		bool placeDungeonEntrance = false;

		//Tries 10 random locations in the chunk, if none meet the required parameters, no dungeon entrance will be placed.
		for (int i = 0; i < 10; i++)
        {
			x = (float)random.Next(0, chunkSize) ;
			z = (float)random.Next(0, chunkSize) ;
			if (forest[(int)getPosition(x, z)].y < 0.5f && 
				!IsWater(world.GetHeight(x + meshObject.transform.position.x, z + meshObject.transform.position.z), meshData)) 
				placeDungeonEntrance = true;
			if (placeDungeonEntrance) break;
		}

		if(placeDungeonEntrance)
        {
			x += meshObject.transform.position.x;
			z += meshObject.transform.position.z;
			float height = world.GetHeight(x, z);
			caveEntranceList.Add(objectPopulator
				.createNewObject(new Vector3(x, height, z), "dungeonEntrance", 1.5f));
        }
	}

	private float getPosition(float x, float z)
    {
		return ((x / 5) * (chunkSize / 5) + (x / 5));
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

	/// <summary>
	/// 
	/// </summary>
	/// <param name="height"></param>
	/// <param name="meshData"></param>
	/// <returns></returns>
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
