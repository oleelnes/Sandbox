using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    
    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public int seed = 10000;
    public int xSize = 255;
    public int zSize = 255;

    public AnimationCurve meshHeightCurve;
    public int heightScale = 30;

    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateMesh();
        UpdateMesh();
    }

   void CreateMesh() 
   {

        //----------------------------------------IMRPROVEMENT_TODO----------------------------------------
        //TODO: A LOT of performance improvements can be done with the code below (between the lines)

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                Vector2 offset = new Vector2(0, 0);
                float y = generateNoiseValue(x, z, 1.0f, 7, 1.8f, 0.4f, seed, offset);
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
            float sampleX = x / scale * frequency + octaveOffsets[octave].x;
            float sampleZ = z / scale * frequency + octaveOffsets[octave].y;

            float perlinValue = Mathf.PerlinNoise(sampleX * 0.08f, sampleZ * 0.08f) * 2;
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

    /*private void OnDrawGizmos()
    {
        if (vertices == null) 
            return;
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }*/

}
