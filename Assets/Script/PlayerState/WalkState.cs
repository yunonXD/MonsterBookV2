using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public override void OnStateEnter(PlayerController player)
    {
        player.state = PlayerState.WalkState;
        player.ani.Play("Run");
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (!player.isGround) player.ChangeState(PlayerState.FallState);
        if (player.walkVector == 0) player.ChangeState(PlayerState.IdleState);
        CheckRotation(player);

        if (CheckWall(player)) player.rigid.velocity = new Vector3(player.walkVector * player.walkSpeed, player.rigid.velocity.y, 0);        
    }

    public override void OnStateExit(PlayerController player)
    {
        player.rigid.velocity = Vector3.zero;
        //player.ani.SetBool("Walk", false);
    }

    private void CheckRotation(PlayerController player)
    {
        if (player.walkVector == 0) return;
        var look = player.walkVector > 0 ? 1 : -1;
        player.transform.rotation = Quaternion.Euler(0, 90 * look, 0);
        player.lookVector = new Vector2(look, 0);
    }

    private bool CheckWall(PlayerController player)
    {
        return !Physics.BoxCast(player.collid.bounds.center, new Vector3(0.025f, 1.2f, 0.5f), new Vector3(player.lookVector.x, 0), Quaternion.identity, player.collid.radius * 2, player.wallLayer);
    }

}
