using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingState : IState
{
    private void Awake()
    {
        canState.Add(PlayerState.IdleState);        
        canState.Add(PlayerState.HitState);
        canState.Add(PlayerState.DeadState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        player.state = PlayerState.CuttingState;
        player.ani.Play("CutAttack");
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (player.ani.GetCurrentAnimatorStateInfo(0).IsName("CutAttack") && player.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            player.ChangeState(PlayerState.IdleState);
        }
    }

    public override void OnStateExit(PlayerController player)
    {
        
    }
}
