using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    internal bool canSeePlayer;
    internal bool isInAttackRange;

    public abstract State RunCurrentState();
}
