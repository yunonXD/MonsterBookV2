using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackReturnState : IState
{

    private void Awake()
    {
        canState.Add(PlayerState.IdleState);
        canState.Add(PlayerState.WalkState);
        canState.Add(PlayerState.DashState);
        canState.Add(PlayerState.AttackState);
        canState.Add(PlayerState.JumpState);
        canState.Add(PlayerState.WireSearchState);
        canState.Add(PlayerState.DeadState);
        canState.Add(PlayerState.HitState);
        canState.Add(PlayerState.FallState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        player.state = PlayerState.IdleState;
        player.ani.Play("AttackReturn" + player.ani.GetInteger("AttackType"));
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (player.ani.GetCurrentAnimatorStateInfo(0).IsName("AttackReturn" + player.ani.GetInteger("AttackType")) && player.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            player.ChangeState(PlayerState.IdleState);
        }
    }

    public override void OnStateExit(PlayerController player)
    {

    }
}
