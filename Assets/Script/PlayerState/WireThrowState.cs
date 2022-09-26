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
    }

    public override void OnStateEnter(PlayerController player)
    {
        time = 0;
        player.state = PlayerState.WireThrowState;
        player.ani.Play("Idle");

        player.ParticlePlay("ThrowWire");
        
        player.wireObject.Shot(player.wireStart.position, new Vector3(player.wireTarget.x, player.wireTarget.y + 0.5f, player.wireTarget.z));
        player.line.enabled = true;                
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
