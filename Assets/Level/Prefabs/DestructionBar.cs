using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestructionBar : MonoBehaviour
{

    public GameObject destructionBarPrefab;

    GameObject destructionBar;
    DestructionProgress desProg;

    // Start is called before the first frame update
    void Start()
    {
        
        DrawBar();
    }

    public void UpdateProgressBar(DestructionProgress.ProgressStatus progress)
    {
        desProg.SetProgressImage(progress);
    }
   
    private void DrawBar()
    {
        destructionBar = Instantiate(destructionBarPrefab);
        destructionBar.transform.SetParent(transform);
        //destructionBar.transform.SetPositionAndRotation(new Vector3(0, 75, 0), new Quaternion(0, 0, 0, 0));
        destructionBar.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //destructionBar.GetComponent<RectTransform>().localPosition = new Vector3(0, 75, 0);


        desProg = destructionBar.GetComponent<DestructionProgress>();
    }

}
