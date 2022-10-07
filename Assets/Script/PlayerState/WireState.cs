using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireState : IState
{
    private void Awake()
    {
        canState.Add(PlayerState.IdleState);
        canState.Add(PlayerState.WireAttackState);
        canState.Add(PlayerState.HitState);
        canState.Add(PlayerState.KnockBackState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        //player.state = PlayerState.WireState;
        player.ani.SetTrigger("WireMove");
        player.rigid.velocity = Vector3.zero;
        player.rigid.useGravity = false;
    }

    public override void OnStateExcute(PlayerController player)
    {
        player.line.SetPosition(0, player.wireStart.position);
        var movePosition = Vector3.Lerp(transform.position, player.wirePos, player.wireForce * Time.fixedDeltaTime);

        player.rigid.MovePosition(movePosition);
        if (Vector3.Distance(transform.position, player.wirePos) <= 1.2f)
        {            
            player.ChangeState(PlayerState.IdleState);            
        }
    }

    public override void OnStateExit(PlayerController player)
    {
        player.rigid.useGravity = true;
        player.line.enabled = false;
    }
}
