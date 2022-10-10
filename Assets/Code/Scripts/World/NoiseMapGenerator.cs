using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGenerator 
{



    public Vector3[] CreateNoiseMap(int xSize, int zSize, int seed, float scale)
    {
        //----------------------------------------IMRPROVEMENT_TODO----------------------------------------
        //TODO: A LOT of performance improvements can be done with the code below (between the lines)

        Vector3[] vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                Vector2 offset = new Vector2(0, 0);
                float y = generateNoiseValue(x, z, scale, 7, 2.8f, 0.5f, seed, offset);
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

        return vertices;

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
}
