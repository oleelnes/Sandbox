using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestructionProgress : MonoBehaviour
{
    public Sprite ProgressTen, ProgressTwenty, ProgressThirty, ProgressFourty,
        ProgressFifty, ProgressSixty, ProgressSeventy, ProgressEighty, ProgressNinety,
        ProgressHundred;

    Image ProgressImage;

    // Start is called before the first frame update
    void Start()
    {
        ProgressImage = GetComponent<Image>();
        GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 75);
        /*ProgressImage.SetNativeSize();
        ProgressImage.transform.position = new Vector3(0, 75, 0);
        ProgressImage.sprite = ProgressTen;*/
    }

   

    public void SetProgressImage(ProgressStatus progress)
    {
        switch(progress)
        {
            case ProgressStatus.NotHitting:
                ProgressImage.sprite = ProgressTen;
                break;
            case ProgressStatus.Ten:
                ProgressImage.sprite = ProgressTen;
                break;
            case ProgressStatus.Twenty:
                ProgressImage.sprite = ProgressTwenty;
                break;
            case ProgressStatus.Thirty:
                ProgressImage.sprite = ProgressThirty;
                break;
            case ProgressStatus.Fourty:
                ProgressImage.sprite = ProgressFourty;
                break;
            case ProgressStatus.Fifty:
                ProgressImage.sprite = ProgressFifty;
                break;
            case ProgressStatus.Sixty:
                ProgressImage.sprite = ProgressSixty;
                break;
            case ProgressStatus.Seventy:
                ProgressImage.sprite = ProgressSeventy;
                break;
            case ProgressStatus.Eighty:
                ProgressImage.sprite = ProgressEighty;
                break;
            case ProgressStatus.Ninety:
                ProgressImage.sprite = ProgressNinety;
                break;
            case ProgressStatus.Hundred:
                ProgressImage.sprite = ProgressHundred;
                break;
        }   
    }
    
    public enum ProgressStatus
    {
        NotHitting = 0,
        Ten = 10,
        Twenty = 20,
        Thirty = 30,
        Fourty = 40,
        Fifty = 50,
        Sixty = 60,
        Seventy = 70,
        Eighty = 80,
        Ninety = 90,
        Hundred = 100,
    }

}
