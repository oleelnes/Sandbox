using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackRadius : MonoBehaviour
{

    public float scaleX = 1.0f;
    public float scaleY = 1.0f;
    public int swipeAttackDistance = 5;
    public int smashAttackDistance = 1;
    public int stompAttackDistance = 2;

    private void Update()
    {
        transform.localScale += new Vector3(stompAttackDistance, stompAttackDistance, 0);
        transform.localScale += new Vector3(smashAttackDistance, smashAttackDistance, 0);
    }

    public void radiusStomp()
    {
        transform.localScale += new Vector3(scaleX*stompAttackDistance, scaleY * stompAttackDistance, 0);
    }

    public void radiusSmash()
    {
        transform.localScale += new Vector3(scaleX * smashAttackDistance, scaleY * smashAttackDistance, 0);
    }

    public void radiusSwipe()
    {
        transform.localScale += new Vector3(scaleX * swipeAttackDistance, scaleY * swipeAttackDistance, 0);
    }
}
