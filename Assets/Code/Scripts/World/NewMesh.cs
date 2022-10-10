using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NewMesh : MonoBehaviour
{
    Mesh mesh;

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

    //Queue<MapThreadInfo<OldMeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<OldMeshData>>();

    public void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    public MeshData CreateNewMesh(int seedXOffset, int seedZOffset)
    {
        return CreateMeshData(10*seedXOffset, 15*seedZOffset);
    }
  
    MeshData CreateMeshData(int seedXOffset, int seedZOffset)
    {
        NoiseMapGenerator noiseMapGenerator = new NoiseMapGenerator();
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        vertices = noiseMapGenerator.CreateNoiseMap(xSize, zSize, seedMain + seedXOffset + seedZOffset, scale);

        colors = new Color[vertices.Length];
        //Creating the color array AND adjusting the height according to both heightMapCurve and the heightScale
        colors = HeightScaleAndColor(vertices, 0.05f);


        //Ordering the vertices of the mesh into triangles
        triangles = CreateTriangles(xSize * zSize * 6);
        
        //Creating the instance of the MeshData that will be returned
        return new MeshData(vertices,colors, triangles);
    }

    private Color[] HeightScaleAndColor(Vector3[] noiseMap, float waterLevel)
    {
        Color[] colorMap = new Color[noiseMap.Length];
        
        for (int i = 0; i < noiseMap.Length; i++)
        {
            //Using the heightMapCurve (AnimationCurve) to evaluate the height value of the mesh!
            noiseMap[i].y = meshHeightCurve.Evaluate(vertices[i].y);// * heightScale;
            if (noiseMap[i].y < waterLevel)
            {
                noiseMap[i].y = waterLevel;
                colorMap[i] = Color.blue;
            }
            else if (noiseMap[i].y > waterLevel && noiseMap[i].y < 0.6f) colorMap[i] = Color.green;
            else if (noiseMap[i].y >= 0.6f && noiseMap[i].y < 0.85f) colorMap[i] = Color.grey;
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

    public MeshData(Vector3[] inVertices, Color[] inColors, int[] inTriangles)
    {
        vertices = inVertices;
        colors = inColors;
        triangles = inTriangles;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        return mesh;
    }
}
