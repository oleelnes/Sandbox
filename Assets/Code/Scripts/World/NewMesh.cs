using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NewMesh : MonoBehaviour
{
    Mesh mesh;
    MeshData meshData;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public const int mapChunkSize = 241;

    public int seedMain = 10000;
    public int xSize = 241;
    public int zSize = 241;
    public float scale = 2.0f;

    public AnimationCurve meshHeightCurve;
    public int heightScale = 30;

    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    public void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMeshData(0,0);
        UpdateMesh();
    }
    public oleMeshData CreateNewMesh(int seedXOffset, int seedZOffset)
    {

        return CreateMeshData(10*seedXOffset, 15*seedZOffset);
        //UpdateMesh();

       // return mesh;
    }
    void Update()
    {
        if (meshDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    oleMeshData CreateMeshData(int seedXOffset, int seedZOffset)
    {

        //----------------------------------------IMRPROVEMENT_TODO----------------------------------------
        //TODO: A LOT of performance improvements can be done with the code below (between the lines)

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                Vector2 offset = new Vector2(0, 0);
                float y = generateNoiseValue(x, z, scale, 7, 2.8f, 0.5f, seedMain + seedXOffset + seedZOffset, offset);
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        //Updating max and min noise value
        float maxHeight = 0, minHeight = float.MaxValue;
        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].y > maxHeight) maxHeight = vertices[i].y;
            if (vertices[i].y < minHeight) minHeight = vertices[i].y;
        }

        float heightDeltaValue = Mathf.Abs(maxHeight - minHeight);
        //max value will now be 1, min will be 0
        for (int i = 0; i < vertices.Length; i++)
            vertices[i].y = (vertices[i].y - minHeight) / heightDeltaValue;


        //This FL and the one above could be the same but I'm separating them for the sake of experimentation for now
        colors = new Color[vertices.Length];
        float waterLevel = 0.05f;
        for (int i = 0; i < vertices.Length; i++)
        {
            //Using the heightMapCurve (AnimationCurve) to evaluate the height value of the mesh!
            vertices[i].y = meshHeightCurve.Evaluate(vertices[i].y);// * heightScale;
            if (vertices[i].y < waterLevel)
            {
                vertices[i].y = waterLevel;
                colors[i] = Color.blue;
            }
            else if (vertices[i].y > waterLevel && vertices[i].y < 0.6f) colors[i] = Color.green;
            else if (vertices[i].y >= 0.6f && vertices[i].y < 0.85f) colors[i] = Color.grey;
            else colors[i] = Color.white;

            vertices[i].y *= heightScale;
        }


        //----------------------------------------IMRPROVEMENT_TODO----------------------------------------


        //Ordering the vertices of the mesh into triangles
        triangles = new int[xSize * zSize * 6];
        int vert = 0, tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;

                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
        oleMeshData meshData = new oleMeshData(vertices,colors, triangles);
        return meshData;
    }

    public Mesh createMesh(MeshFilter meshFilter, Vector3[] vertices, int[] triangles, Color[] colors)
    {
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.triangles = triangles;
        meshFilter.mesh.colors = colors;
        return meshFilter.mesh;
    }


    /**
     * x: The world's x position.
     * z: The world's z position.
     * scale: The scale.
     * octaves: The amount of times that the perlin noise is recalculated with
     * altered samples.
     * persistance: changes amplitude. 
     *      Should not be greater than 3.0f or lower than 0.0f.
     * lucanarity: affects frequency -- the higher, the spikier the terrain.
     *      Should not be greater than 1.0f or lower than 0.0f.
     */
    float generateNoiseValue(int x, int z, float scale, int octaves, float persistance, float lucanarity, int seed, Vector2 offset)
    {
        //Offsetting the noise value and adding different layers of random numbers to the noise 
        System.Random r = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = r.Next(-100000, 100000) + offset.x;
            float offsetZ = r.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetZ);
        }

        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;
        for (int octave = 0; octave < octaves; octave++)
        {
            float sampleX = (x + octaveOffsets[octave].x) / scale * frequency;
            float sampleZ = (z + octaveOffsets[octave].y) / scale * frequency;

            float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ) * 2;
            noiseHeight += perlinValue * amplitude;

            amplitude *= persistance;
            frequency *= lucanarity;

        }
        return noiseHeight;
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;



        //Lighting(?)
        mesh.RecalculateNormals();

        //Collision detection
        MeshCollider meshCollider = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = GetComponent<MeshFilter>().mesh;
    }

    public void RequestMeshData(Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate {
            MeshDataThread(callback);
        };

        new Thread(threadStart).Start();
    }

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }

    }

    void MeshDataThread(Action<MeshData> callback)
    {
       // MeshData meshD = CreateNewMesh();
      
        lock (meshDataThreadInfoQueue)
        {
          //  meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshD));
        }
    }

}

public class oleMeshData
{
    public Color[] colors;
    //public Mesh mesh;
    public int[] triangles;
    public Vector3[] vertices;

    public oleMeshData(Vector3[] inVertices, Color[] inColors, int[] inTriangles)
    {
        vertices = inVertices;
        colors = inColors;
        triangles = inTriangles;
        //mesh = inMesh;
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Color[] colors;


    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        //uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        colors = new Color[meshWidth * meshHeight]; 
    }



    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
        return mesh;
    }
}
