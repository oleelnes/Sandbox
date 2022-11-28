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
    private int progress = 0;
    private DestructionBar dBar;
    private string currentTag = "";
    float interval = 0.5f;
    public InventoryItemData treeItemData;

    // Start is called before the first frame update
    void Start()
    {
        dBar = FindObjectOfType<DestructionBar>();
        //playerCam = FindObjectOfType<PlayerCam>();


    }

    // Update is called once per frame
    void Update()
    {
        HitManager();
    }

    private void HitManager()
    {
        PlayerCam playerCam = FindObjectOfType<PlayerCam>();



        // ray = playerCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        Vector3 centerScreen = new Vector3(Screen.width/2, Screen.height/2, 0);
        ray = playerCam.GetComponent<Camera>().ScreenPointToRay(centerScreen);


        if (CheckHit())
        {
            MoveCamera moveCamScript = FindObjectOfType<MoveCamera>();
            GameObject hitObject = hit.collider.gameObject;

            if (CheckDistance(moveCamScript, hitObject, 4.0f)) InitialHitAction();
            else QuitHitting();

            if (hitting) HitAction(hitObject);

        }
        else if (CheckQuitHitting()) QuitHitting();
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

            Player player = FindObjectOfType<Player>();
            var inventory = player.transform.GetComponent<InventoryHolder>();

            if (!inventory) return;

            if (inventory.InventorySystem.AddToInventory(treeItemData, 1))
            {

            }
            //TODO: add to inventory
        }
    }

    private void QuitHitting()
    {
        progress = 0;
        hitting = false;
        dBar.UpdateProgressBar(DestructionProgress.ProgressStatus.NotHitting);
    }


    private bool leftMouseDown = false;
    // Called by Actions_OnFoot
    private void DoAttack(){  leftMouseDown = true; }
    // Called by Actions_OnFoot
    private void ReleaseAttack(){ leftMouseDown = false; }
    /// <summary>
    /// Checks if player has quit hitting an object, either by letting go of the mouse or not 
    /// hitting the hitBox of the object anymore.
    /// </summary>
    /// <returns> Returns false if player is still hitting; true if player has quit hitting. </returns>
    private bool CheckQuitHitting()
    {
        return hitting && !leftMouseDown || hitting && hit.collider.tag != currentTag || !hitting;
    }

    private bool checkColliderTag(string tag)
    {
        switch (tag)
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
        return leftMouseDown && Physics.Raycast(ray, out hit);
    }

    private DestructionProgress.ProgressStatus GetProgress(int progress)
    {
        switch (progress)
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

    private enum Weapon
    {
        AXE,
        SWORD,
        PICKAXE,

    }

}
