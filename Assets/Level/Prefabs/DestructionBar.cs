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
        if (progress == DestructionProgress.ProgressStatus.NotHitting)
            destructionBar.SetActive(false);
        else
        {
            destructionBar.SetActive(true);
            desProg.SetProgressImage(progress);
        }
    }
   
    private void DrawBar()
    {
        destructionBar = Instantiate(destructionBarPrefab);
        destructionBar.transform.SetParent(transform);

        desProg = destructionBar.GetComponent<DestructionProgress>();
        UpdateProgressBar(DestructionProgress.ProgressStatus.NotHitting);
    }

}
