using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpState : IState
{
    private float checkTime;
    private bool check;
    private bool fall;

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
        checkTime = 0;
        check = false;
        fall = false;
        player.state = PlayerState.JumpState;
        player.rigid.velocity = Vector3.zero;
        player.rigid.AddForce(Vector3.up * player.jumpForce, ForceMode.Impulse);
        player.ani.Play("Jump");
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (player.rigid.velocity.y < 0) player.ChangeState(PlayerState.FallState);

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
}
