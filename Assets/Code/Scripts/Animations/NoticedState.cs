using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticedState : State
{
    public ChaseState chaseState;
    public override State RunCurrentState()
    {
        if (isInAttackRange)
        {
            return chaseState;
        }
        else
        {
            return this;
        }
    }
}
