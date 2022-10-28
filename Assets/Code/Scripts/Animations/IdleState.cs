using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public NoticedState noticedState;
    public bool canSeeThePlayer;
    public override State RunCurrentState()
    {
        if (canSeePlayer)
        {
            return noticedState;
        }
        else
        {
            return this;
        }
    }

}
