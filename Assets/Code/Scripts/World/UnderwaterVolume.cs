using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class UnderwaterVolume : MonoBehaviour
{

    PostProcessVolume underwaterVolume;
    GameObject underwaterVolumeGameObject;

    // Start is called before the first frame update
    void Start()
    {
        //underwaterVolume = GameObject.Find("UnderwaterVolume");
        underwaterVolumeGameObject = GameObject.Find("UnderwaterVolume");
        underwaterVolume = underwaterVolumeGameObject.GetComponent<PostProcessVolume>();
        underwaterVolume.weight = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(FindObjectOfType<Player>().transform.position.y > 0)
        {
            EnableComponents(false);
        }
        else
        {
            EnableComponents(true);
            Debug.Log("enabled? " + underwaterVolume.profile.GetSetting<ColorGrading>().enabled.value);
        }
    }

    void EnableComponents(bool enabled)
    {
        underwaterVolume.profile.GetSetting<ColorGrading>().enabled.value = enabled;
        underwaterVolume.profile.GetSetting<Bloom>().enabled.value = enabled;
        underwaterVolume.profile.GetSetting<ScreenSpaceReflections>().enabled.value = enabled;
    }
}
