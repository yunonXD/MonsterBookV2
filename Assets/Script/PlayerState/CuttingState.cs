using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingState : IState
{
    private void Awake()
    {         
        canState.Add(PlayerState.HitState);
        canState.Add(PlayerState.DeadState);
        canState.Add(PlayerState.CuttingReturnState);
        canState.Add(PlayerState.KnockBackState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        //player.state = PlayerState.CuttingState;
        if (player.mode) player.ani.Play("N_CutAttack");
        else player.ani.Play("CutAttack");
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (player.ani.GetCurrentAnimatorStateInfo(0).IsName("CutAttack") && player.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            player.ChangeState(PlayerState.CuttingReturnState);
        }
    }

    public override void OnStateExit(PlayerController player)
    {
        
    }
}
