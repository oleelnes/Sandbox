using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{


    private Ray ray;
    private RaycastHit hit;
    private bool hitting = false;
    private float startTime = 0.0f;
    private float hitDuration = 0.0f;
    private int trees = 0;
    private float progress = 0;
    private DestructionBar dBar;

    // Start is called before the first frame update
    void Start()
    {
        dBar = FindObjectOfType<DestructionBar>();
        
       
        
    }

    // Update is called once per frame
    void Update()
    {
        checkHit();
    }

    private void checkHit()
    {
        PlayerCam pC = FindObjectOfType<PlayerCam>();
        pC.GetComponent<Camera>();
        ray = pC.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0) && Physics.Raycast(ray, out hit))
        {
            MoveCamera moveCamScript = FindObjectOfType<MoveCamera>();
            GameObject hitObject = hit.collider.gameObject;
            if (Vector3.Distance(moveCamScript.getCameraPosition(), hitObject.transform.position) < 4.0f)
            {
                if (hit.collider.tag == "tree" && !hitting)
                {
                    //Shrinking the object. Will be changed later.
                    //hitObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    startTime = Time.time;
                    hitting = true;
                    Debug.Log("hit an object");
                }
            }
            if (hitting)
            {
                if (((Time.time - startTime) % 10) + 1 >= progress + 1)
                {
                    progress += 1;
                    Debug.Log("hitting an object, progress: " + progress * 10 + "%");
                    dBar.UpdateProgressBar(DestructionProgress.ProgressStatus.Ten);
                }
                if (Time.time - startTime > 10)
                {
                    trees++;
                    Debug.Log("you have " + trees + " trees.");
                    hitObject.tag = "deleteTree";
                    progress = 0;
                    hitting = false;
                }
            }
        }
        else if (hitting && !Input.GetMouseButtonDown(0) && !Physics.Raycast(ray, out hit))
        {
            Debug.Log("stopped hitting");
            progress = 0;
            hitting = false;
        }
    }
}
