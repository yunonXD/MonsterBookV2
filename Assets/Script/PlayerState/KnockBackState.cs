using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackState : IState
{
    float time;

    private void Awake()
    {
        canState.Add(PlayerState.IdleState);
    }

    public override void OnStateEnter(PlayerController player)
    {        
        player.state = PlayerState.KnockBackState;
        player.ani.Play("JumpLoop");
        player.invinBool = true;        
        player.input.SetInputAction(false);
        time = 0;
    }

    public override void OnStateExcute(PlayerController player)
    {
        time += Time.deltaTime;
        if (player.isGround && time > 0.5f)
        {
            player.ChangeState(PlayerState.IdleState);
        }
    }

    public override void OnStateExit(PlayerController player)
    {
        player.invinBool = false;
        player.input.SetInputAction(true);
    }
}
