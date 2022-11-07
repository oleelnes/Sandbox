using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{

    private PlayerCam playerCam;
    private Ray ray;
    private RaycastHit hit;
    private bool hitting = false;
    private float startTime = 0.0f;
    private float hitDuration = 0.0f;
    private int trees = 0;
    private int progress = 0;
    private DestructionBar dBar;
    private string currentTag = "";
    float interval = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        dBar = FindObjectOfType<DestructionBar>();
        PlayerCam pC = FindObjectOfType<PlayerCam>();


    }

    // Update is called once per frame
    void Update()
    {
        HitManager();
    }

    private void HitManager()
    {
        
        ray = playerCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (CheckHit())
        {
            MoveCamera moveCamScript = FindObjectOfType<MoveCamera>();
            GameObject hitObject = hit.collider.gameObject;

            if (CheckDistance(moveCamScript, hitObject, 4.0f)) InitialHitAction();
            
            if (hitting) HitAction(hitObject);
            
        }
        else if (CheckQuitHitting())
        {
            progress = 0;
            hitting = false;
            dBar.UpdateProgressBar(DestructionProgress.ProgressStatus.NotHitting);
        }
    }

    private void InitialHitAction()
    {
        if (checkColliderTag(hit.collider.tag) && !hitting)
        {
            startTime = Time.time;
            hitting = true;
        }
    }

    private void HitAction(GameObject hitObject)
    {
        if (((Time.time - startTime)) >= progress * interval)
            dBar.UpdateProgressBar(GetProgress(++progress * 10));

        if (progress > 10)
        {
            trees++;
            Debug.Log("you have " + trees + " " + currentTag + ".");
            hitObject.tag = "deleteTree";
            progress = 0;
            hitting = false;
            dBar.UpdateProgressBar(DestructionProgress.ProgressStatus.NotHitting);
            //TODO: add to inventory
        }
    }

    /// <summary>
    /// Checks if player has quit hitting an object, either by letting go of the mouse or not 
    /// hitting the hitBox of the object anymore.
    /// </summary>
    /// <returns> Returns false if player is still hitting; true if player has quit hitting. </returns>
    private bool CheckQuitHitting()
    {
        return hitting && !Input.GetMouseButtonDown(0) || hitting && hit.collider.tag != currentTag;
    }

    private bool checkColliderTag(string tag)
    {
        switch(tag)
        {
            case "tree":
                currentTag = "tree";
                return true;
            default: return false;
        }
    }


    private bool CheckDistance(MoveCamera moveCamScript, GameObject hitObject, float distance)
    {
        return Vector3.Distance(moveCamScript.getCameraPosition(), hitObject.transform.position) < distance;
    }

    

    private void SetInterval(string weapon, string hitObjectType)
    {

    }

    private bool CheckHit()
    {
        return Input.GetMouseButton(0) && Physics.Raycast(ray, out hit);
    }

    private DestructionProgress.ProgressStatus GetProgress(int progress)
    {
        switch(progress)
        {
            case 10: return DestructionProgress.ProgressStatus.Ten;
            case 20: return DestructionProgress.ProgressStatus.Twenty;
            case 30: return DestructionProgress.ProgressStatus.Thirty;
            case 40: return DestructionProgress.ProgressStatus.Fourty;
            case 50: return DestructionProgress.ProgressStatus.Fifty;
            case 60: return DestructionProgress.ProgressStatus.Sixty;
            case 70: return DestructionProgress.ProgressStatus.Seventy;
            case 80: return DestructionProgress.ProgressStatus.Eighty;
            case 90: return DestructionProgress.ProgressStatus.Ninety;
            case 100: return DestructionProgress.ProgressStatus.Hundred;
            default: return DestructionProgress.ProgressStatus.NotHitting;
        }
    }

    private enum Weapon {
        AXE,
        SWORD,
        PICKAXE, 

    }

}
