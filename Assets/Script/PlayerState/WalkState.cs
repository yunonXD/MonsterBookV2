using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WalkState : IState
{

    private void Awake()
    {
        canState.Add(PlayerState.IdleState);
        canState.Add(PlayerState.DashState);
        canState.Add(PlayerState.AttackState);
        canState.Add(PlayerState.CuttingState);
        canState.Add(PlayerState.JumpState);
        canState.Add(PlayerState.WireSearchState);
        canState.Add(PlayerState.DeadState);
        canState.Add(PlayerState.HitState);
        canState.Add(PlayerState.FallState);
        canState.Add(PlayerState.KnockBackState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        //player.state = PlayerState.WalkState;
        if (player.mode) player.ani.Play("N_Run");
        else player.ani.Play("Run");
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (!player.isGround) player.ChangeState(PlayerState.FallState);
        if (player.walkVector == 0) player.ChangeState(PlayerState.IdleState);
        player.CheckRotation();

        player.Move();
    }

    public override void OnStateExit(PlayerController player)
    {
        player.rigid.velocity = Vector3.zero;        
    }


}
