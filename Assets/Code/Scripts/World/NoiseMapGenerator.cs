using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGenerator 
{



    public NoiseData CreateNoiseMap(int xSize, int zSize, int seed, float scale, Vector2 offset, int octaves, float persistance, float lucanarity, int biomeIndicator, int polyScale = 1)
    {

       if (polyScale < 1) polyScale = 1;  
       
        Debug.Log("biomeIndicator: " + biomeIndicator);

        //Offsetting the noise value and adding different layers of random numbers to the noise 
        System.Random r = new System.Random(seed);

        Vector3[] vertices = new Vector3[(xSize + 1) * (zSize + 1)];



        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;
        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = r.Next(-100000, 100000) + offset.x;
            float offsetZ = r.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetZ);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }


        for (int i = 0, z = 0; z <= zSize * polyScale; z += polyScale)
        {
            for (int x = 0; x <= xSize * polyScale; x += polyScale)
            {
                float y = generateNoiseValue(x, z, scale, octaves, persistance, lucanarity, frequency, octaveOffsets, amplitude);



                if (x != 0 && x != xSize * polyScale && z != 0 && z != zSize * polyScale)
                {
                    // Debug.Log("inside");
                           
                }




                vertices[i] = new Vector3(x, y, z);
               
                i++;
            }
        }


        for (int z = 0, i = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float normalizedHeight = vertices[i].y / (2f * maxPossibleHeight / 1.25f);
                vertices[i].y = normalizedHeight;
                i++;
            }
        }

        //test

        //Updating max and min noise value
        /*float maxHeight = 0, minHeight = float.MaxValue;
        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].y > maxHeight) maxHeight = vertices[i].y;
            if (vertices[i].y < minHeight) minHeight = vertices[i].y;
        }

        float heightDeltaValue = Mathf.Abs(maxHeight - minHeight);
        //max value will now be 1, min will be 0
        for (int i = 0; i < vertices.Length; i++)
            vertices[i].y = (vertices[i].y - minHeight) / heightDeltaValue;*/

        NoiseData noiseData = new NoiseData(vertices);

        return noiseData;

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
    public float generateNoiseValue(int x, int z, float scale, int octaves, float persistance, float lucanarity, float frequency, Vector2[] octaveOffsets, float amplitude)
    {

        float noiseHeight = 0;
        amplitude = 1;
        frequency = 1;
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
}

public class NoiseData
{
    public Vector3[] noiseMap;


    public NoiseData(Vector3[] inNoiseMap)
    {
        noiseMap = inNoiseMap;
    }
}
