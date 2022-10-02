using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : IState
{    
    private void Awake()
    {
        canState.Add(PlayerState.LandState);
        canState.Add(PlayerState.DashState);
        canState.Add(PlayerState.WireSearchState);
        canState.Add(PlayerState.KnockBackState);
        canState.Add(PlayerState.HitState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        player.state = PlayerState.FallState;     
        player.ani.Play("JumpLoop");
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (player.isGround)
        {            
            player.ChangeState(PlayerState.LandState);
        }

        CheckRotation(player);

        if (player.CheckMonster())
        {

        }
        else if (player.CheckWall()) player.rigid.velocity = new Vector3(player.walkVector * player.walkSpeed, player.rigid.velocity.y, 0);
    }

    public override void OnStateExit(PlayerController player)
    {
        
    }

    private void CheckRotation(PlayerController player)
    {
        if (player.walkVector == 0) return;
        var look = player.walkVector > 0 ? 1 : -1;
        transform.localScale = new Vector3(2, 2, 2 * look);
        player.lookVector = new Vector2(look, 0);
    }

    //private bool CheckFall(PlayerController player)
    //{
    //    return Physics.BoxCast(player.collid.bounds.center, new Vector3(0.15f, 0.015f, 0.5f), -transform.up, Quaternion.identity, player.collid.height * 2.5f, player.groundLayer);
    //}
}
