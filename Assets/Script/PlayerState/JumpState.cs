using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpState : IState
{
    private void Awake()
    {
        canState.Add(PlayerState.IdleState);
        canState.Add(PlayerState.DashState);
        canState.Add(PlayerState.HitState);
        canState.Add(PlayerState.FallState);
        canState.Add(PlayerState.WireSearchState);
        canState.Add(PlayerState.KnockBackState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        //player.state = PlayerState.JumpState;
        player.rigid.velocity = Vector3.zero;
        player.rigid.AddForce(Vector3.up * player.jumpForce, ForceMode.Impulse);
        player.ani.SetTrigger("Jump");
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (player.rigid.velocity.y < 0) player.ChangeState(PlayerState.FallState);

        player.CheckRotation();
        if (player.CheckMonster())
        {

        }
        else if (player.CheckWall()) player.rigid.velocity = new Vector3(player.walkVector * player.walkSpeed, player.rigid.velocity.y, 0);
    }

    public override void OnStateExit(PlayerController player)
    {

    }

}
