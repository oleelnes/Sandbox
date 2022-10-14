using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NewMesh : MonoBehaviour
{
    Mesh mesh;

    //Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public const int mapChunkSize = 241;

    public int seedMain = 10000;
    public int xSize = 241;
    public int zSize = 241;
    public float scale = 2.0f;
    public Vector2 offset;

    public AnimationCurve meshHeightCurve;
    public int heightScale = 30;

    //Queue<MapThreadInfo<OldMeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<OldMeshData>>();

    public void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    public MeshData CreateNewMesh(Vector2 position, int polyScale)
    {
        return CreateMeshData(position, polyScale);
    }
  
    MeshData CreateMeshData(Vector2 position, int polyScale)
    {
        NoiseMapGenerator noiseMapGenerator = new NoiseMapGenerator();
        Vector3[] vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        NoiseData noiseData = noiseMapGenerator.CreateNoiseMap(xSize, zSize, seedMain, scale, position + offset, 7, 2.8f, 0.5f, (int) position.x / 240, polyScale);


        vertices = noiseData.noiseMap;


        colors = new Color[vertices.Length];
        //Creating the color array AND adjusting the height according to both heightMapCurve and the heightScale
        colors = HeightScaleAndColor(vertices, 0.05f, position);


        //Ordering the vertices of the mesh into triangles
        triangles = CreateTriangles(xSize * zSize * 6);
        
        //Creating the instance of the MeshData that will be returned
        return new MeshData(vertices,colors, triangles, zSize);
    }

    private Color[] HeightScaleAndColor(Vector3[] noiseMap, float waterLevel, Vector2 position)
    {
        Color[] colorMap = new Color[noiseMap.Length];

        System.Random colorRandomizer = new System.Random(123123);

        float groundGrassColor = 0.0f;
        float waterIterator = 0.0f;
        float mountainSideColor = 0.0f;
        
        for (int i = 0; i < noiseMap.Length; i++)
        {
            //Using the heightMapCurve (AnimationCurve) to evaluate the height value of the mesh!
            noiseMap[i].y = meshHeightCurve.Evaluate(noiseMap[i].y);// * heightScale;
            if (noiseMap[i].y < waterLevel)
            {
                noiseMap[i].y = waterLevel;
                colorMap[i] = new Color(0.1f + (waterIterator / 2.5f), 0.1f + (waterIterator / 2.5f), 0.3f + waterIterator);
                waterIterator = (float)colorRandomizer.NextDouble() % 0.25f;
            }
            else if (noiseMap[i].y > waterLevel && noiseMap[i].y < 0.6f)
            {

                groundGrassColor = Mathf.PerlinNoise((offset.x + ((float)noiseMap[i].x + position.x)) / 254.66f,
                    ((float)offset.y + ((float)noiseMap[i].z + position.y)) / 291.3f);

                colorMap[i] = new Color(0.1f + (groundGrassColor / 1.5f), 0.2f + (groundGrassColor / 1.2f), 0.1f + (groundGrassColor / 2.5f));

            }
            else if (noiseMap[i].y >= 0.6f && noiseMap[i].y < 0.85f)
            {
                groundGrassColor = Mathf.PerlinNoise((offset.x + ((float)noiseMap[i].x + position.x)) / 24.2f,
                    ((float)offset.y + ((float)noiseMap[i].z + position.y)) / 20.3f);

                colorMap[i] = new Color(0.2f + (groundGrassColor / 1.2f), 0.2f + (groundGrassColor / 1.2f), 0.2f + (groundGrassColor / 1.2f));
            }
            else colorMap[i] = Color.white;

            noiseMap[i].y *= heightScale;
        }

        

        return colorMap;
    }
    private int[] CreateTriangles(int size)
    {
        int[] triangleArray = new int[size];
        int vert = 0, tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangleArray[tris + 0] = vert + 0;
                triangleArray[tris + 1] = vert + xSize + 1;
                triangleArray[tris + 2] = vert + 1;

                triangleArray[tris + 3] = vert + 1;
                triangleArray[tris + 4] = vert + xSize + 1;
                triangleArray[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
        return triangleArray;
    }
}



public class MeshData
{
    public Color[] colors;
    //public Mesh mesh;
    public int[] triangles;
    public Vector3[] vertices;
    public int zSize;

    public MeshData(Vector3[] inVertices, Color[] inColors, int[] inTriangles, int inZSize)
    {
        vertices = inVertices;
        colors = inColors;
        triangles = inTriangles;
        zSize = inZSize;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        
        return mesh;
    }

    /*
     https://answers.unity.com/questions/607226/get-height-of-a-mesh-by-x-and-z-coordinates.html
     */
    public float GetLocalHeight(int x, int z, MeshCollider collider)
    {
        RaycastHit hit;
        float hitHeight = -999.9f;
        Ray ray = new Ray(new Vector3(x, 100.0f, z), Vector3.down);
        if (!collider.Raycast(ray, out hit, 200.0f))
        {
            hitHeight = hit.point.y;
        }
        return hitHeight;
    }
}
