using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandState : IState
{    
    private void Awake()
    {
        canState.Add(PlayerState.IdleState);
        canState.Add(PlayerState.DashState);
        canState.Add(PlayerState.WalkState);
        canState.Add(PlayerState.WireSearchState);
        canState.Add(PlayerState.CuttingState);
        canState.Add(PlayerState.AttackState);
        canState.Add(PlayerState.KnockBackState);
        canState.Add(PlayerState.DeadState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        player.state = PlayerState.FallState;        
        player.ani.Play("Landing");
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (player.ani.GetCurrentAnimatorStateInfo(0).IsName("Landing") && player.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            player.ChangeState(PlayerState.IdleState);
            
        }
    }

    public override void OnStateExit(PlayerController player)
    {

    }
}
