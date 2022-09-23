using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : IState
{
    private void Awake()
    {
        canState.Add(PlayerState.IdleState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        player.invinBool = true;
        player.state = PlayerState.HitState;
        player.ani.Play("Hit");
        player.rigid.AddForce(player.lookVector * -8, ForceMode.Impulse);
        StartCoroutine(Routine(player));
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (player.ani.GetCurrentAnimatorStateInfo(0).IsName("Hit") && player.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (player.isGround) player.ChangeState(PlayerState.IdleState);
            else player.ChangeState(PlayerState.FallState);
        }
    }

    public override void OnStateExit(PlayerController player)
    {
        
    }

    private IEnumerator Routine(PlayerController player)
    {
        yield return YieldInstructionCache.waitForSeconds(2);
        player.invinBool = false;
    }
}
