using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
	public const float maxViewDst = 450;
	public Transform viewer;

	public Material materialTest;
	public Texture texture;

	public int mapChunkSize = 241;

	public bool trees = true;
	

	public bool flatshading = false;

	public int polyScale = 1;

	///public Material mapMaterial;

	public static Vector2 viewerPosition;
	static NewMesh mapGenerator;
	static PopulateWithObjects treePopulator;

	int chunkSize;
	int chunksVisibleInViewDst;

	Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
	List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

	void Start()
	{
		mapGenerator = FindObjectOfType<NewMesh>();
		treePopulator = FindObjectOfType<PopulateWithObjects>();

		chunkSize = ((mapChunkSize) * polyScale);// + eller - 1, originalt -;
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / (chunkSize ));
		
	}

	void Update()
	{
		Debug.DrawRay(new Vector3(10f + chunkSize, 20f, 10f), new Vector3(0f, -50f, 0f), Color.blue);
		viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
		UpdateVisibleChunks();
		//Debug.Log("height: " + GetHeight(1000.0f, 1000.0f) + "\n");
	}

	/// <summary> 
	/// Function that can be used to get the height of a given location. If the queried location exists, the height will be returned,
	/// otherwise, null will be returned.  https://answers.unity.com/questions/607226/get-height-of-a-mesh-by-x-and-z-coordinates.html
	/// </summary>
	/// <param name="x">  x position </param>
	/// <param name="z" > z position </param>
	/// <remarks> returns null if the queried location does not exist. </remarks>
	public float GetHeight(float x, float z)
    {
		float retFloat = -999.0f;

		int currentChunkCoordX = Mathf.RoundToInt(x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt(z / chunkSize);

		// Iterates through the visible chunks to find which one the player recides in.
		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
		{
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
			{
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				// Then checks if the viewed chunks exists in the terrainChunkDictionary
				if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
				{
					// And if it does, then it calls the method GetLocalHeight(x,z) which lies in the TerrainChunk Class.
					retFloat = terrainChunkDictionary[viewedChunkCoord]
						.GetLocalHeight(x, z);
				}
			}
		}
		return retFloat;
	}

	//from tutorial
	/// <summary>
	/// Function that updates the visible chunks. If a brand new, unloaded chunk is visible, it instantiates it, 
	/// and if a loaded chunk is too far away, it hides it.
	/// </summary>
	void UpdateVisibleChunks()
	{
		
		for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
		{
			terrainChunksVisibleLastUpdate[i].SetVisible(false);
		}
		terrainChunksVisibleLastUpdate.Clear();

		int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
		{
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
			{
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
				{
					terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk(new Vector2(viewerPosition.x, viewerPosition.y));
					if (terrainChunkDictionary[viewedChunkCoord].IsVisible())
					{
						terrainChunksVisibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCoord]);
					}
				}
				else
				{
					terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, transform, materialTest, polyScale, texture, mapChunkSize , flatshading, trees));
					Debug.Log("chunkcoord: " + viewedChunkCoord + ", chunksVisibleInDist: " + chunksVisibleInViewDst);
				}

			}
		}
	}


	/// <summary>
	/// Class that manages the individual terrain chunks.
	/// </summary>
	public class TerrainChunk
	{

		//Reference to the class within which TerrainChunk recides (better way to do this?)
		EndlessTerrain world;

		//Mesh
		public MeshData meshData;
		public MeshCollider meshCollider;
		GameObject meshObject;
		MeshRenderer meshRenderer;
		MeshFilter meshFilter;

		//Ground objects
		List<GameObject> treeList;
		List<GameObject> caveEntranceList;
		BoxCollider caveEntranceCollider;
		WorldPopulated worldPopulated = WorldPopulated.FALSE;
		bool trees;

		//Positions
		Vector2 position;
		Bounds bounds;
		int chunkSize;

		int count = 0;


		public TerrainChunk(Vector2 coord, int size, Transform parent, Material material, int polyScale, Texture texture, int mapChunkSize, bool flatShading, bool trees)
		{
			this.chunkSize = mapChunkSize * polyScale;
			
			world = FindObjectOfType<EndlessTerrain>();
			treeList = new List<GameObject>();
			caveEntranceList = new List<GameObject>();
			caveEntranceCollider = FindObjectOfType<BoxCollider>();
			
			
			position = coord * size;
			bounds = new Bounds(position , Vector2.one * size);
			Vector3 positionV3 = new Vector3(position.x, 0, position.y);

			//Storing the mesh object as a game object.
			meshObject = new GameObject("Terrain Chunk");
			meshRenderer = meshObject.AddComponent<MeshRenderer>();
			meshFilter = meshObject.AddComponent<MeshFilter>();

			meshObject.transform.position = positionV3;
			meshObject.transform.parent = parent;
			SetVisible(false);


			//Requesting mesh data for new terrain chunk
			meshData = mapGenerator.CreateNewMesh(position, polyScale, mapChunkSize, material);

			//Using the mesh data to create a new mesh. 
			meshFilter.mesh = meshData.CreateMesh(flatShading);

			//Setting the material of the mesh renderer. This will be done elsewhere in the future.
			meshRenderer.material = material;


			//Adding collision detection to the mesh -- this will be done elsewhere in the future.
			meshCollider = meshObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = meshFilter.mesh;

			this.trees = trees;
		}


		public void UpdateTerrainChunk(Vector2 position)
		{
			count++;
			float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
			bool visible = viewerDstFromNearestEdge <= maxViewDst;
			SetVisible(visible);
			if (count >= 45)
            {
				SetObjectsVisible(position);
				count = 0;
            }
			if (worldPopulated == WorldPopulated.FALSE)
			{
				worldPopulated = WorldPopulated.TRUE;
				populateTerrainChunk(trees);
			}
			if (worldPopulated == WorldPopulated.TRUE)
            {
				for (int i = 0; i < caveEntranceList.Count; i++)
                {
					BoxCollider collider = caveEntranceList[i].GetComponent<BoxCollider>();
					
                }
            }
		}

		

		public void SetVisible(bool visible)
		{
			meshObject.SetActive(visible);
			//if(worldPopulated == WorldPopulated.HIDDEN && visible == true) ObjectsVisible(true);
			//else if (worldPopulated == WorldPopulated.TRUE && visible == false) ObjectsVisible(false);
		}

		public void SetObjectsVisible(Vector2 position)
        {
			if (worldPopulated != WorldPopulated.FALSE)
            {
				ObjectsVisible(position, 300f);
            }
        }

		public bool IsVisible()
		{
			return meshObject.activeSelf;
		}

		public void populateTerrainChunk(bool trees)
        {
			if (trees)
			{
				populateWithCaveEntrances();
				populateWithTrees(20, 20, 50*50);
			}
				//if (rocks) populateWithRocks();
        }

		/// <summary>
		/// Function that changes the visibility of objects that are populated in the world.
		/// </summary>
		/// <param name="visible">The visibility of the objects.</param>
		void ObjectsVisible(Vector2 position, float distance)
		{
			for (int i = 0; i < treeList.Count; i++)
            {
				Vector3 vec3 = new Vector3(position.x, 0.0f, position.y);
				if (treeList[i].activeSelf && Vector3.Distance(vec3, treeList[i].transform.position) > distance)
                {
					treeList[i].SetActive(false);
                }
				else if (Vector3.Distance(vec3, treeList[i].transform.position) <= distance)
                {
					treeList[i].SetActive(true);

				}
			}
			
			/*caveEntranceList.ForEach(gameObject => gameObject.SetActive(visible));
			treeList.ForEach(gameObject => gameObject.SetActive(visible));
			if (visible) worldPopulated = WorldPopulated.TRUE;
			else worldPopulated = WorldPopulated.HIDDEN;*/
		}



		//TODO: move out of this file!
		void populateWithTrees(int xSize, int zSize, int amount)
        {
			System.Random treePosRandom = new System.Random(121112);
			for (int x = 0; x < 300; x+=10)
			{
				for (int y = 0; y < 300; y+=10)
				{
					float internalX = x + treePosRandom.Next(-4, 4);
					float internalY = y + treePosRandom.Next(-4, 4);
					float placementLocationX = internalX + meshObject.transform.position.x;
					float placementLocationZ = internalY + meshObject.transform.position.z;
					float height = world.GetHeight(placementLocationX, placementLocationZ);
						/*Debug.Log("height: " + height + "; x and z after: " + 
							placementLocationX + " and " + placementLocationZ 
							+ "x and z before: " + x + " and " + y);*/
					if (!IsWater(height) && internalX < chunkSize && internalY < chunkSize)
					{
						Vector3 vec = new Vector3(placementLocationX, height, placementLocationZ);
						treeList.Add(treePopulator.createNewObject(vec, "Tree", 5f + treePosRandom.Next(-1, 3)));
					}
					
				}
			}
			if (treeList.Count > 4) treePopulator.makeTreeVisible(treeList[2], false);

			//Setting the world populated enum to true.
			
		}

		void populateWithCaveEntrances()
        {
			float height = world.GetHeight((float)(200) + meshObject.transform.position.x, (float)(200) + meshObject.transform.position.z);
			caveEntranceList.Add(treePopulator.createNewObject(new Vector3((float)(200) + meshObject.transform.position.x, height, (float)(200) + meshObject.transform.position.z), "dungeonEntrance", 1.5f));
        }

		

		/// <summary>
		/// Handles the state of the world population.
		/// </summary>
		enum WorldPopulated
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
			float hitHeight = -999.9f;
			Ray ray = new Ray(new Vector3(x, 100.0f, z), Vector3.down);

			if (!meshCollider.Raycast(ray, out hit, 150.0f)) return hit.point.y;

			return hitHeight;
		}

		/*public bool IsWater(float x, float z)
        {
			if (GetLocalHeight(x, z) >= meshData.getWaterLevel()) return false;
			return true;
        }*/

		public bool IsWater(float height)
		{
			//Debug.Log("mesh water: " + meshData.getWaterLevel() + ", current height: " + height);
			if (height > meshData.getWaterLevel())
			{
				
				return false;
			}
			return true;
		}

		public bool IsMountainTop(float x, float z)
        {
			return true;
        }
	}
}
