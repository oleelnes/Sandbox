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
					terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk(new Vector2(viewerPosition.x, viewerPosition.y), trees);
					if (terrainChunkDictionary[viewedChunkCoord].IsVisible())
					{
						terrainChunksVisibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCoord]);
					}
				}
				else
				{
					terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, transform, materialTest, polyScale, texture, mapChunkSize , flatshading, trees));
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
		ChunkObjects chunkObjects;
		Corruption corruption;

		BoxCollider caveEntranceCollider;
		bool trees;

		//Positions
		Vector2 position;
		Bounds bounds;
		int chunkSize;

		int count = 0;
		int corrTest = 0;

		public TerrainChunk(Vector2 coord, int size, Transform parent, Material material, int polyScale, Texture texture, int mapChunkSize, bool flatShading, bool trees)
		{
			this.chunkSize = mapChunkSize * polyScale;
			
			
			corruption = FindObjectOfType<Corruption>();
			world = FindObjectOfType<EndlessTerrain>();

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
			chunkObjects = new ChunkObjects(meshCollider, chunkSize, trees);
		}


		public void UpdateTerrainChunk(Vector2 position, bool visibility)
		{
			count++;
			float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
			bool visible = viewerDstFromNearestEdge <= maxViewDst;
			SetVisible(visible);
			
			if (chunkObjects.worldPopulated == ChunkObjects.WorldPopulated.FALSE)
			{
				chunkObjects.worldPopulated = ChunkObjects.WorldPopulated.TRUE;
				chunkObjects.populateTerrainChunk(true, meshObject, meshData, world, treePopulator);
			}
			if (count >= 45)
			{
				corrTest++;
				chunkObjects.SetObjectsVisible(position, visibility);
				count = 0;
				EnableCorruption(corrTest);
			}
			
		}

		public void EnableCorruption(int extra)
        {
			Debug.Log("colors size: " + meshData.colors.Length + ", triangles size: " + meshData.triangles.Length);
			RaycastHit hit;
			float hitHeight = -999.9f;
			Ray ray = new Ray(new Vector3(1.0f, 100.0f, 1.0f), Vector3.down);
			Color[] newColors = meshData.colors;

			meshCollider.Raycast(ray, out hit, 150.0f);
			int triangle = hit.triangleIndex;
			if (hit.collider != null)
            {
				for (int i = 0; i < 100 + extra; i++)
				{

					newColors[triangle + i] = new Color(0.1f, 0.1f, 0.1f);
				}
				meshFilter.mesh.colors = newColors;
			}
		}

		public void SetVisible(bool visible)
		{
			meshObject.SetActive(visible);
		}


		public bool IsVisible()
		{
			return meshObject.activeSelf;
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
	}
}