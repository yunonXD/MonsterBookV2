using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WireSearchState : IState
{
    private Transform target;

    private void Awake()
    {
        canState.Add(PlayerState.IdleState);
        canState.Add(PlayerState.WireThrowState);
        canState.Add(PlayerState.HitState);
        canState.Add(PlayerState.KnockBackState);
    }

    public override void OnStateEnter(PlayerController player)
    {
        //if (!player.isGround) player.SetTimeScale(0.5f);
        //player.state = PlayerState.WireSearchState;
        player.ani.SetTrigger("Search");
        player.wirePos = Vector3.zero;

        Find(player); 
    }

    public override void OnStateExcute(PlayerController player)
    {
        player.ui.SetWireAim(player.wirePos, true);
    }

    public override void OnStateExit(PlayerController player)
    {
        //player.SetTimeScale(1);
        player.ui.SetWireAim();
    }

    private void Find(PlayerController player)
    {
        RaycastHit[] target = Physics.BoxCastAll(transform.position, new Vector3(12, 10, 2), player.lookVector, Quaternion.identity, 6.5f, player.wireLayer);
        
        if (target.Length > 0)
        {            
            var dir = (target[0].collider.GetComponent<Collider>().bounds.center - player.wireStart.position).normalized;            
            RaycastHit ray;
            if (Physics.Raycast(player.wireStart.position, dir, out ray, 100, player.wireLayer))
            {
                player.wirePos = ray.point;
                //Debug.Log(target[0].GetComponent<Collider>().bounds.center + "   -    " + ray.point);
            }
        }
    }
}
