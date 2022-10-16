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

	

	public bool flatshading = false;

	public int polyScale = 1;

	///public Material mapMaterial;

	public static Vector2 viewerPosition;
	static NewMesh mapGenerator;
	static PopulateWithTrees treePopulator;

	int chunkSize;
	int chunksVisibleInViewDst;

	Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
	List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

	void Start()
	{
		mapGenerator = FindObjectOfType<NewMesh>();
		treePopulator = FindObjectOfType<PopulateWithTrees>();

		chunkSize = ((mapChunkSize) * polyScale);// + eller - 1, originalt -;
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / (chunkSize ));
		
	}

	void Update()
	{
		Debug.DrawRay(new Vector3(10f + chunkSize, 20f, 10f), new Vector3(0f, -50f, 0f), Color.blue);
		viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
		UpdateVisibleChunks();
		//Debug.Log("height: " + GetHeight(1000.0f, 1000.0f) + "\n");
		Debug.Log("Height player: " + GetHeight(viewerPosition.x, viewerPosition.y) + "\n");
	}

	/// <summary> 
	/// Function that can be used to get the height of a given location. If the queried location exists, the height will be returned,
	/// otherwise, null will be returned. 
	/// </summary>
	/// <param name="x">  x position </param>
	/// <param name="z" > z position </param>
	/// <remarks> returns null if the queried location does not exist. </remarks>
	public float GetHeight(float x, float z)
    {
		//Debug.Log("incomding x and z: " + x + ", " + z);
		float retFloat = -999.0f;

		int currentChunkCoordX = Mathf.RoundToInt(x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt(z / chunkSize);

		float internalXPos = Mathf.Abs((x) % (chunkSize));
		float internalZPos = Mathf.Abs((z) % (chunkSize));
		///Debug.Log("Internal x and z: " + internalXPos + ", " + internalZPos);


		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
		{
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
			{
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
				{
					retFloat = terrainChunkDictionary[viewedChunkCoord]
						.GetLocalHeight(x, z);
				}
			}
		}
		//Debug.Log("... yielded the height: " + retFloat);
		return retFloat;
	}

	//from tutorial
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
					terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
					if (terrainChunkDictionary[viewedChunkCoord].IsVisible())
					{
						terrainChunksVisibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCoord]);
					}
				}
				else
				{
					terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, transform, materialTest, polyScale, texture, mapChunkSize , flatshading));
					Debug.Log("chunkcoord: " + viewedChunkCoord + ", chunksVisibleInDist: " + chunksVisibleInViewDst);
				}

			}
		}
	}

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
		WorldPopulated worldPopulated = WorldPopulated.FALSE;



		//Positions
		Vector2 position;
		Bounds bounds;


		public TerrainChunk(Vector2 coord, int size, Transform parent, Material material, int polyScale, Texture texture, int mapChunkSize, bool flatShading)
		{
			world = FindObjectOfType<EndlessTerrain>();
			treeList = new List<GameObject>();
			
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
			meshData = mapGenerator.CreateNewMesh(position, polyScale, mapChunkSize);

			//Using the mesh data to create a new mesh. 
			meshFilter.mesh = meshData.CreateMesh(flatShading);

			//Setting the material of the mesh renderer. This will be done elsewhere in the future.
			meshRenderer.material = material;




			//Adding collision detection to the mesh -- this will be done elsewhere in the future.
			meshCollider = meshObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = meshFilter.mesh;

			
		}


		public void UpdateTerrainChunk()
		{
			float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
			bool visible = viewerDstFromNearestEdge <= maxViewDst;
			SetVisible(visible);
			if (worldPopulated == WorldPopulated.FALSE) populateTerrainChunk(true);
		}

		public void SetVisible(bool visible)
		{
			meshObject.SetActive(visible);
			if(worldPopulated == WorldPopulated.HIDDEN && visible == true) ObjectsVisible(true);
			else if (worldPopulated == WorldPopulated.TRUE && visible == false) ObjectsVisible(false);
		}

		public bool IsVisible()
		{
			return meshObject.activeSelf;
		}

		public void populateTerrainChunk(bool trees)
        {
			if (trees) populateWithTrees();
        }

		public float GetLocalHeight(float x, float z)
		{
			RaycastHit hit;
			float hitHeight = -999.9f;
			Ray ray = new Ray(new Vector3(x, 100.0f, z), Vector3.down);

			if (!meshCollider.Raycast(ray, out hit, 150.0f)) hitHeight = hit.point.y;
			
			return hitHeight;
		}

		//TODO: move out of this file!
		void populateWithTrees()
        {
			for (int x = 0; x < 10; x++)
			{
				for (int y = 0; y < 10; y++)
				{
					float height = world.GetHeight((float)(10 + x * 5) + meshObject.transform.position.x, (float)(10 + y * 5) + meshObject.transform.position.z);
					if (height > 0.0f)
					{
						Vector3 vec = new Vector3(10 + x * 5 + meshObject.transform.position.x, height, 10 + y * 5 + meshObject.transform.position.z);
						treeList.Add(treePopulator.createNewTree(vec));
					}
				}
			}
			if (treeList.Count > 4) treePopulator.makeTreeVisible(treeList[2], false);

			//Setting the world populated enum to true.
			worldPopulated = WorldPopulated.TRUE;
		}

		void ObjectsVisible(bool visible)
        {
			treeList.ForEach(gameObject => gameObject.SetActive(visible));
			if (visible) worldPopulated = WorldPopulated.TRUE;
			else worldPopulated = WorldPopulated.HIDDEN;
        }

		enum WorldPopulated
        {
			TRUE,
			FALSE, 
			HIDDEN
        }


	}


}
