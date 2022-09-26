using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : IState
{
    private bool fall;

    private void Awake()
    {
        canState.Add(PlayerState.IdleState);
        canState.Add(PlayerState.DashState);
        canState.Add(PlayerState.WireSearchState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        player.state = PlayerState.FallState;
        fall = false;
        player.ani.Play("Jump_Loop");
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (CheckFall(player) && !fall)
        {
            player.ani.Play("Landing");
            fall = true;
        }
        if (player.isGround) player.ChangeState(PlayerState.IdleState);

        CheckRotation(player);

        if (CheckWall(player)) player.rigid.velocity = new Vector3(player.walkVector * player.walkSpeed, player.rigid.velocity.y, 0);
    }

    public override void OnStateExit(PlayerController player)
    {
        
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

    private bool CheckFall(PlayerController player)
    {
        return Physics.BoxCast(player.collid.bounds.center, new Vector3(0.15f, 0.015f, 0.5f), -transform.up, Quaternion.identity, player.collid.height * 2.5f, player.groundLayer);
    }
}
