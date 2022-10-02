using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireThrowState : IState
{
    private float time;

    private void Awake()
    {
        canState.Add(PlayerState.WireState);
        canState.Add(PlayerState.DeadState);
        canState.Add(PlayerState.HitState);
        canState.Add(PlayerState.KnockBackState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        time = 0;
        player.state = PlayerState.WireThrowState;
        player.ani.Play("WireThrow");

        player.ParticlePlay("ThrowWire");                                      
    }

    public override void OnStateExcute(PlayerController player)
    {
        player.line.SetPosition(1, player.wireObject.transform.position);
        player.line.SetPosition(0, player.wireStart.position);
        time += Time.deltaTime;
        if (time > 0.2f)
        {
            player.ChangeState(PlayerState.WireState);
        }
    }

    public override void OnStateExit(PlayerController player)
    {

    }
}
