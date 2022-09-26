using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IState : MonoBehaviour
{
    protected List<PlayerState> canState = new List<PlayerState>();

    public abstract void OnStateEnter(PlayerController player);
    public abstract void OnStateExcute(PlayerController player);
    public abstract void OnStateExit(PlayerController player);
    
    public bool CheckState(PlayerState state)
    {
        return canState.Contains(state);
    }
}
