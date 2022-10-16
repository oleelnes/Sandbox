using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : AnimationStateManager
{

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
