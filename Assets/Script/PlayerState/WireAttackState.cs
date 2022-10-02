using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WireAttackState : IState
{
    private bool change;

    private void Awake()
    {
        canState.Add(PlayerState.IdleState);        
    }

    public override void OnStateEnter(PlayerController player)
    {
        change = false;
        player.invinBool = true;
        player.state = PlayerState.WireAttackState;
        player.ani.Play("WireAttack");
        player.line.enabled = true;

        //player.rigid.AddForce(player.lookVector * 15, ForceMode.Impulse);
    }

    public override void OnStateExcute(PlayerController player)
    {
        player.line.SetPosition(1, player.wireObject.transform.position);
        player.line.SetPosition(0, player.wireStart.position);

        var targetPos = new Vector3(player.wireTarget.x + player.lookVector.x * 3, player.wireTarget.y);
        var movePosition = Vector3.Lerp(transform.position, targetPos, player.wireForce * Time.fixedDeltaTime);

        if (!change) player.rigid.MovePosition(movePosition);
        
        if (Mathf.Abs(transform.position.x - targetPos.x) < 0.3f) change = true;
        if (Mathf.Abs(transform.position.x - player.wireTarget.x) < 0.5f && player.line.enabled) player.line.enabled = false;


        if (player.ani.GetCurrentAnimatorStateInfo(0).IsName("WireAttack") && player.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && change)
        {
            player.ChangeState(PlayerState.IdleState);
        }
    }

    public override void OnStateExit(PlayerController player)
    {        
        player.line.enabled = false;
        player.invinBool = false;
    }
}
