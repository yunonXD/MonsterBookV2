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
        //player.state = PlayerState.HitState;
        if (player.mode) player.ani.SetTrigger("Hit");
        else player.ani.SetTrigger("Hit");

        CameraController.CameraShaking(0.5f, 0.2f);

        player.rigid.AddForce(player.lookVector * -8, ForceMode.Impulse);
        StartCoroutine(Routine(player));
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (player.ani.GetCurrentAnimatorStateInfo(0).IsName("Hit") && player.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            player.ChangeState(PlayerState.IdleState);
        }
    }

    public override void OnStateExit(PlayerController player)
    {
        
    }

    private IEnumerator Routine(PlayerController player)
    {
        yield return YieldInstructionCache.waitForSeconds(1.2f);
        player.invinBool = false;
    }
}
