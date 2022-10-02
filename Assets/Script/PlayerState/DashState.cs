using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DashState : IState
{
    private void Awake()
    {
        canState.Add(PlayerState.IdleState);
        canState.Add(PlayerState.FallState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        player.state = PlayerState.DashState;
        player.rigid.velocity = Vector3.zero;
        player.invinBool = true;
        player.ani.Play("Dash");
        gameObject.layer = LayerMask.NameToLayer("PlayerDash");
        player.rigid.AddForce(new Vector3(player.lookVector.x * player.dashForce, 3), ForceMode.Impulse);
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (player.ani.GetCurrentAnimatorStateInfo(0).IsName("Dash") && player.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (player.isGround) player.ChangeState(PlayerState.IdleState);
            else player.ChangeState(PlayerState.FallState);
        }        
    }

    public override void OnStateExit(PlayerController player)
    {
        player.invinBool = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
