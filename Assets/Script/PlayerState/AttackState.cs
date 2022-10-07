using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : IState
{    
    private float attackTime;
    private bool attackIng;
    private float attackResetTime = 1.5f;
    private int attackCount = 0;


    private void Awake()
    {
        canState.Add(PlayerState.IdleState);
        canState.Add(PlayerState.HitState);
        canState.Add(PlayerState.KnockBackState);
        canState.Add(PlayerState.DeadState);
        canState.Add(PlayerState.AttackReturnState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        attackTime = 0;
        //player.state = PlayerState.AttackState;
        player.rigid.velocity = Vector3.zero;
        if (player.mode) player.ani.Play("N_Attack" + player.attackCount);
        else player.ani.Play("Attack" + player.attackCount);
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (player.mode)
        {
            if (player.ani.GetCurrentAnimatorStateInfo(0).IsName("N_Attack" + player.attackCount) && player.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                if (player.prevInput)
                {
                    player.AgainState();
                }
                else player.ChangeState(PlayerState.AttackReturnState);
            }
        }
        else
        {
            if (player.ani.GetCurrentAnimatorStateInfo(0).IsName("Attack" + player.attackCount) && player.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                if (player.prevInput)
                {
                    player.AgainState();
                }
                else player.ChangeState(PlayerState.AttackReturnState);
            }
        }
    }

    public override void OnStateExit(PlayerController player)
    {
        //if (!attackIng) StartCoroutine(ResetRoutine(player));
        //else if (attackIng) attackTime = 0;
        player.attackCount++;        
        if (player.attackCount > 2) player.attackCount = 0;
        //player.ani.SetInteger("AttackCount", player.attackCount);
        player.AttackBoxOff(0);
    }

    private IEnumerator ResetRoutine(PlayerController player)
    {
        attackTime = 0;
        attackIng = true;
        while (attackIng)
        {
            attackTime += Time.deltaTime;
            if (attackTime > attackResetTime) attackIng = false;
            yield return new WaitForFixedUpdate();
        }
        attackIng = false;
        player.attackCount = 0;
        player.ani.SetInteger("AttackCount", player.attackCount);        
    }
}
