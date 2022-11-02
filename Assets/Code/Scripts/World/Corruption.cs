using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corruption : MonoBehaviour
{

    Vector2[] corruptionVectors;
    Bounds corruptionBounds;
    int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        corruptionVectors = new Vector2[4];
        corruptionVectors[0] = new Vector2(0f, 0f);
        corruptionVectors[1] = new Vector2(0f, 50f);
        corruptionVectors[2] = new Vector2(50f, 50f);
        corruptionVectors[3] = new Vector2(50f, 0f);
        corruptionBounds = new Bounds(new Vector3(100f, 0f, 100f), new Vector3(50f, 100f, 50f));
    }

    // Update is called once per frame
    void Update()
    {
        if (counter >= 500)
        {
            //corruptionBounds.size = new Vector3(corruptionBounds.size.x + 10.0f, 100f, corruptionBounds.size.z + 10.0f);
            corruptionBounds.Expand(10.0f);
            counter = 0;
        }
        counter++;
    }

    public bool IsCorrupted(float x, float z)
    {
        if (corruptionBounds.Contains(new Vector3(x, 10f, z))) return true;
        return false;
    }

    public Bounds GetCorruptionBounds()
    {
        return corruptionBounds;
    }
}
