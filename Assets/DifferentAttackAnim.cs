using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentAttackAnim : MonoBehaviour
{
    private Animator anim;

    IEnumerator Start()
    {
        anim = GetComponent<Animator>();

        while (true)
        {
            yield return new WaitForSeconds(3);
            anim.SetInteger("AttackIndex", Random.Range(0, 3));
            anim.SetTrigger("Attack");
        }
    }

}
