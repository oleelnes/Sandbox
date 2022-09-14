using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    
    Vector3[] vertices;
    int[] triangles;

    public int xSize = 150;
    public int zSize = 150;

    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

   void CreateShape() 
   {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                Vector2 offset = new Vector2(0, 0);
                float y = generateNoiseValue(x, z, 2.0f, 5, 1.8f, 0.6f, 1222, offset);
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }
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
            float sampleX = x / scale * frequency + octaveOffsets[octave].x;
            float sampleZ = z / scale * frequency + octaveOffsets[octave].y;

            float perlinValue = Mathf.PerlinNoise(sampleX * .2f, sampleZ * .2f) * 2 - 1;
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

        mesh.RecalculateNormals();

        //Collision detection
        MeshCollider meshCollider = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = GetComponent<MeshFilter>().mesh;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null) 
            return;
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

}
