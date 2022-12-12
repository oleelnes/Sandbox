using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkObjects
{

    public List<GameObject> treeList;
    public List<GameObject> caveEntranceList;
	public List<GameObject> rockList;
	public List<GameObject> plantList;

	public List<GameObject> enemyList;

	public List<Vector3> objectPositions; //not in use yet. TODO!

	System.Random random;

	public bool visible;
	public WorldPopulated worldPopulated;

	private Vector3[] forest;

	MeshCollider meshCollider;
	GameObject proc = GameObject.FindGameObjectWithTag("MeshGenerator");
	GameObject meshObject; MeshData meshData; EndlessTerrain world; PopulateWithObjects objectPopulator;

	int chunkSize;

	public ChunkObjects(MeshCollider collider, int chunkSize, bool visible, 
		int randomOffset, Vector3[] forest, GameObject meshObject, MeshData meshData, EndlessTerrain world, PopulateWithObjects objectPopulator)
    {
		this.meshCollider = collider;
		this.chunkSize = chunkSize;	
		this.visible = visible;
		this.forest = forest;
		this.world = world;
		this.meshData = meshData;
		this.objectPopulator = objectPopulator;
		this.meshObject = meshObject;
		worldPopulated = WorldPopulated.FALSE;
		random = new System.Random(1002 + randomOffset);

		treeList = new List<GameObject>();
		caveEntranceList = new List<GameObject>();
		rockList = new List<GameObject>();	
		plantList = new List<GameObject>();
		enemyList = new List<GameObject>();
		
	}

    public void SetObjectsVisible(Vector2 position, bool objectsVisible)
    {
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

    public void populateTerrainChunk(bool visible)
    {
		populateWithCaveEntrances();
		populateWithTrees();
		populateWithRocks();
		populateWithPlants();
	}

	private void populateWithRocks()
    {
		//smallRock
		for (int i = 0; i < 100; i++)
        {
			float x = 0f + meshObject.transform.position.x + (float)random.Next(0, chunkSize);
			float z = 0f + meshObject.transform.position.z + (float)random.Next(0, chunkSize);

			float height = world.GetHeight(x, z);
			if(!IsWater(height - 0.2f))
            {
				rockList.Add(objectPopulator.createNewObject(new Vector3(x, height, z), "rock", 2.0f + (float)random.Next(0, 2), "rockOne"));
            }
        }

		for (int i = 0; i < random.Next(0, 3); i++)
        {
			float x = 0f + meshObject.transform.position.x + (float)random.Next(0, chunkSize);
			float z = 0f + meshObject.transform.position.z + (float)random.Next(0, chunkSize);

			float height = world.GetHeight(x, z);

			if (!IsWater(height - 0.2f))
			{
				rockList.Add(objectPopulator.createNewObject(new Vector3(x, height, z), "rock", 1.0f + (float)random.Next(0, 2), "rockBigOne"));
			}
		}
    }

	private void populateWithPlants()
	{
		//grass
		addToList(plantList,  "plant", "grassOne", 400, 600, 1, 0.2f);
		//flowers
		addToList(plantList,  "plant", "flowerOne", 50, 100, 1);
		//mushroom
		addToList(plantList,  "plant", "mushroomOne", 20, 60, 2);

		addToList(plantList,  "bush", "bushOne", 50, 100, 5, 4, 0.3f, 0.6f);

		addToList(plantList,  "bush", "bushTwo", 20, 50, 5, 4, 0.3f, 0.6f);

		addToList(plantList,  "bush", "bushThree", 20, 50, 5, 4, 0.3f, 0.5f);
	}

	private void addToList(List<GameObject> list,
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

			if (!IsWater(height - 0.5f))
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
	private void populateWithTrees()
	{
		for (int x = 0, i = 0; x < chunkSize; x += 10)
		{
			for (int y = 0; y < chunkSize; y += 10, i++)
			{
				createTreeInForest(i, x, y);
			}	
		}
	}

	/// <summary>
	/// Function that places one tree, given that all necessary criterea are met.
	/// </summary>
	/// <param name="i">x and y's corresponding position in the forest array</param>
	/// <param name="x">x position</param>
	/// <param name="y">y position</param>
	/// <returns></returns>
	private void createTreeInForest(int i, int x, int y)
    {
		int treeInterval = Mathf.RoundToInt(forest[i].y * 12);
		int randomNumber = random.Next(0, 10);

		//The higher the noise value in the forest noisemap, the greater the probability that a tree will appear
		//Noise value measured in values between 0 and 1.
		bool placeTree = randomNumber + Mathf.RoundToInt(forest[i].y * 10) < 14;

		float internalX = (float)x + (float)random.Next(-treeInterval, treeInterval);
		float internalY = (float)y + (float)random.Next(-treeInterval, treeInterval);
		float placementLocationX = internalX + meshObject.transform.position.x;
		float placementLocationZ = internalY + meshObject.transform.position.z;
		float height = world.GetHeight(placementLocationX, placementLocationZ);

		//Addition of the random makes the edges of the forests have wider spacing
		if (forest[i].y > 0.6f + ((float)random.NextDouble() % 0.15))
		{
			if (placeTree && !IsWater(height - 0.5f) && height > 0.5f && height < 30 && internalX < (chunkSize) && internalY < (chunkSize))
			{
				if (forest[i].y > 0.75f && forest[i].y < 0.85f)
				{
					string treeTypeRandom = (random.Next(0, 2) == 1) ? "treeRoundTwo" : "treeRoundThree";
					GenerateNewTree(placementLocationX, height - 0.1f, placementLocationZ, treeTypeRandom);
				}
				else GenerateNewTree(placementLocationX, height - 0.1f, placementLocationZ);
			}
		}
	}

    internal void enemiesHandler(bool spawnMonsters)
    {
		if (spawnMonsters && enemyList.Count < 35)
        {
			spawnNewEnemies(20 - enemyList.Count);

		}
		else if (!spawnMonsters && enemyList.Count > 0)
        {
			for (int i = 0; i < enemyList.Count; i++)
            {
				world.toDelete.Add(enemyList[i]);
            }
			enemyList.Clear();
		}
    }

	private void spawnNewEnemies(int numEnemies)
    {
		for(int i = 0; i < numEnemies; i++)
        {
			bool found = false;
			bool exhausted = false;
			float xPosition, zPosition, height;
			int j = 0;
			do
			{
				if (j > 20) exhausted = true;
				float x = random.Next(0, chunkSize);
				float z = random.Next(0, chunkSize);

				xPosition = x + meshObject.transform.position.x;
				zPosition = z + meshObject.transform.position.z;

				height = world.GetHeight(xPosition, zPosition);

				if ( !IsWater(height - 0.5f) && height > 1.0f) found = true;
    
				j++;
			} while (!found || !exhausted);

			if(found)
            {
				int monsterSelector = random.Next(2);
				string enemyType = "";
				switch(monsterSelector)
                {
					case 0: enemyType = "goblin"; break;
					case 1: enemyType = "houndman"; break;
					case 2: enemyType = "squidman"; break;
					default: enemyType = "squidman"; break;
                }
				GameObject enemy = objectPopulator.createNewObject(new Vector3(xPosition, height, zPosition), "enemy", 1.0f, enemyType);
				if (enemy != null) enemyList.Add(enemy);
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
    void GenerateNewTree(float placementLocationX, float height, float placementLocationZ, string defaultTreeType = "")
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
	public void populateWithCaveEntrances()
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
				!IsWater(world.GetHeight(x + meshObject.transform.position.x, z + meshObject.transform.position.z))) 
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
	public bool IsWater(float height)
	{
		if (height > meshData.getWaterLevel()) return false;
		return true;
	}

	public bool IsMountainTop(float x, float z)
	{
		return true;
	}

}
