using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : IState
{    
    private float attackTime;
    private bool attackIng;
    private float attackResetTime = 3;
    private int attackCount;    


    private void Awake()
    {
        canState.Add(PlayerState.IdleState);
        canState.Add(PlayerState.HitState);        
        canState.Add(PlayerState.AttackReturnState);
    }

    public override void OnStateEnter(PlayerController player)
    {        
        player.state = PlayerState.AttackState;
        player.rigid.velocity = Vector3.zero;
        player.ani.Play("Attack" + attackCount);      
    }

    public override void OnStateExcute(PlayerController player)
    {
        if (player.ani.GetCurrentAnimatorStateInfo(0).IsName("Attack"+ attackCount) && player.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (attackCount == 2) player.ChangeState(PlayerState.IdleState);
            else player.ChangeState(PlayerState.AttackReturnState);
        }        
    }

    public override void OnStateExit(PlayerController player)
    {
        if (attackCount == 0 && !attackIng) StartCoroutine(ResetRoutine(player));
        else if (attackIng) attackTime = 0;
        attackCount++;
        if (attackCount > 2) attackCount = 0;
        player.ani.SetInteger("AttackType", attackCount);
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
        attackCount = 0;
        player.ani.SetInteger("AttackType", attackCount);
    }
}
