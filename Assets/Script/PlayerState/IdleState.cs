using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private float restTime;
    private bool restBool;

    private void Awake()
    {
        canState.Add(PlayerState.WalkState);
        canState.Add(PlayerState.DashState);
        canState.Add(PlayerState.AttackState);
        canState.Add(PlayerState.JumpState);
        canState.Add(PlayerState.WireSearchState);
        canState.Add(PlayerState.DeadState);
        canState.Add(PlayerState.HitState);
        canState.Add(PlayerState.FallState);
        canState.Add(PlayerState.CuttingState);
        canState.Add(PlayerState.KnockBackState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        
        restTime = 0;
        restBool = false;
        player.state = PlayerState.IdleState;
        player.ani.Play("Idle");        
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (player.walkVector != 0) player.ChangeState(PlayerState.WalkState);
        if (!player.isGround) player.ChangeState(PlayerState.FallState);
        if (!restBool)
        {
            restTime += Time.deltaTime;
            if (restTime > 5)
            {
                player.ani.Play("Rest");
                restBool = true;
            }
        }
    }

    public override void OnStateExit(PlayerController player)
    {

    }
}
