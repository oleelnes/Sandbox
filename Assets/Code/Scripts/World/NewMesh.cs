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

    

    public int seedMain = 10000;
    
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
    public MeshData CreateNewMesh(Vector2 position, int polyScale, int chunkSize)
    {
        return CreateMeshData(position, polyScale, chunkSize);
    }
  
    MeshData CreateMeshData(Vector2 position, int polyScale, int chunkSize)
    {
        NoiseMapGenerator noiseMapGenerator = new NoiseMapGenerator();
        Vector3[] vertices = new Vector3[(chunkSize + 1) * (chunkSize + 1)];

        NoiseData noiseData = noiseMapGenerator.CreateNoiseMap(chunkSize, chunkSize, seedMain, scale, position + offset, 7, 2.8f, 0.5f, (int) position.x / (chunkSize - 1), polyScale);


        vertices = noiseData.noiseMap;


        colors = new Color[vertices.Length];
        //Creating the color array AND adjusting the height according to both heightMapCurve and the heightScale
        colors = HeightScaleAndColor(vertices, 0.05f, position);


        //Ordering the vertices of the mesh into triangles
        triangles = CreateTriangles(chunkSize * chunkSize * 6, chunkSize);
        
        //Creating the instance of the MeshData that will be returned
        return new MeshData(vertices, colors, triangles);
    }

    private Color[] HeightScaleAndColor(Vector3[] noiseMap, float waterLevel, Vector2 position)
    {
        Color[] colorMap = new Color[noiseMap.Length];

        System.Random colorRandomizer = new System.Random(123123);

        float groundGrassColor = 0.0f;
        float waterIterator = 0.0f;
        float mountainSideColor = 0.0f;
        
        Vector2[] UVs = new Vector2[noiseMap.Length];

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

                //Groundgrass color is procedurally set(without octaves), thus varying the shade of green of the ground. 
                //The divisors (254.66 and 291.3) are arbitrarily set and adjust the rate of change of color.
                //
                //TODO: Implement the coloring of the ground in a custom shader.
                groundGrassColor = Mathf.PerlinNoise((offset.x + ((float)noiseMap[i].x + position.x)) / 254.66f,
                    ((float)offset.y + ((float)noiseMap[i].z + position.y)) / 291.3f);
                groundGrassColor += (Mathf.PerlinNoise((offset.x + ((float)noiseMap[i].x + position.x)) / 2.26f,
                    ((float)offset.y + ((float)noiseMap[i].z + position.y)) / 2.3f)) / 9.5f ;
                groundGrassColor += (Mathf.PerlinNoise((offset.x + ((float)noiseMap[i].x + position.x)) / 0.012f,
                    ((float)offset.y + ((float)noiseMap[i].z + position.y)) / 0.014f)) / 10.5f;




                colorMap[i] = new Color(0.1f + (groundGrassColor / 2.5f), 0.2f + (groundGrassColor / 1.2f), 0.1f + (groundGrassColor / 2.5f));

            }
            else if (noiseMap[i].y >= 0.6f && noiseMap[i].y < 0.85f)
            {
                groundGrassColor = Mathf.PerlinNoise((offset.x + ((float)noiseMap[i].x + position.x)) / 24.2f,
                    ((float)offset.y + ((float)noiseMap[i].z + position.y)) / 20.3f);

                colorMap[i] = new Color(0.2f + (groundGrassColor / 1.2f), 0.2f + (groundGrassColor / 1.2f), 0.2f + (groundGrassColor / 1.2f));
            }
            else colorMap[i] = Color.white;

           /* if (i % 4 == 0) UVs[i] = new Vector2(0.0f, 0.0f);
            else if (i % 4 == 1) UVs[i] = new Vector2(1.0f, 0.0f);
            else if (i % 4 == 2) UVs[i] = new Vector2(0.0f, 1.0f);
            else if (i % 4 == 3) UVs[i] = new Vector2(1.0f, 1.0f);*/

            noiseMap[i].y *= heightScale;
        }

        

        return colorMap;
    }

    /*
     * Function that returns an integer array consisting of the ordering of the triangles within the noisemap.
     * int size: the size of the noisemap.
     */
    private int[] CreateTriangles(int size, int chunkSize)
    {
        int[] triangleArray = new int[size];
        int vert = 0, tris = 0;
        for (int z = 0; z < chunkSize; z++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                triangleArray[tris + 0] = vert + 0;
                triangleArray[tris + 1] = vert + chunkSize + 1;
                triangleArray[tris + 2] = vert + 1;

                triangleArray[tris + 3] = vert + 1;
                triangleArray[tris + 4] = vert + chunkSize + 1;
                triangleArray[tris + 5] = vert + chunkSize + 2;

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
    //public Vector2[] UVs;
    public int[] triangles;
    public Vector3[] vertices;
    //public int zSize;

    public MeshData(Vector3[] vertices, Color[] colors, int[] triangles)
    {
        this.vertices = vertices;
        this.colors = colors;
        this.triangles = triangles;
        //zSize = inZSize;
    }

    public Mesh CreateMesh(bool flatShading)
    {
        Mesh mesh = new Mesh();
        if(flatShading) FlatShading();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
        //mesh.uv = UVs;

        return mesh;
    }

    /*
     https://answers.unity.com/questions/607226/get-height-of-a-mesh-by-x-and-z-coordinates.html
     */
    public float GetLocalHeight(float x, float z, MeshCollider collider)
    {
        RaycastHit hit;
        float hitHeight = -999.9f;
        Ray ray = new Ray(new Vector3(x, 100.0f, z), Vector3.down);
        //Debug.Log("x and z: " + x + " and " + z);
        //return FetchHeightFromLocation(Mathf.RoundToInt(x), Mathf.RoundToInt(z));
        if (!collider.Raycast(ray, out hit, 150.0f))
        {
            hitHeight = hit.point.y;
        }
        
        return hitHeight;
    }

    private float FetchHeightFromLocation(int x, int z)
    {
        float height = -20.0f;
        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].x == x && vertices[i].z == z)
            {
                height = vertices[i].y;
                break;
            }
        }
        return height;
    }

    void FlatShading()
    {
        Vector3[] flatShadedVertices = new Vector3[triangles.Length];
        Color[] flatShadedColors = new Color[triangles.Length];

        for (int i = 0; i < triangles.Length; i++)
        {
            flatShadedVertices[i] = vertices[triangles[i]];
            flatShadedColors[i] = colors[triangles[i]];
            triangles[i] = i;
        }
        vertices = flatShadedVertices;
        colors = flatShadedColors;
    }
}
