using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WireSearchState : IState
{
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
        player.state = PlayerState.WireSearchState;
        player.ani.Play("Search");
        player.wireTarget = Vector3.zero;

        Find(player);        
    }

    public override void OnStateExcute(PlayerController player)
    {
        player.ui.SetWireAim(player.wireTarget, true);
    }

    public override void OnStateExit(PlayerController player)
    {
        //player.SetTimeScale(1);
        player.ui.SetWireAim();
    }

    private void Find(PlayerController player)
    {
        Collider[] target = Physics.OverlapBox(transform.position + new Vector3(player.lookVector.x * 6.5f, 4f), new Vector3(6f, 5, 1), Quaternion.identity, player.wireLayer);
        //Debug.Log(target.Length);
        if (target.Length > 0)
        {            
            var dir = (target[0].GetComponent<Collider>().bounds.center - player.wireStart.position).normalized;            
            RaycastHit ray;
            if (Physics.Raycast(player.wireStart.position, dir, out ray, 100, player.wireLayer))
            {
                //player.line.enabled = true;
                //player.line.SetPosition(0, player.wireStart.position);
                //player.line.SetPosition(1, ray.point);
                player.wireTarget = ray.point;
                //Debug.Log(target[0].GetComponent<Collider>().bounds.center + "   -    " + ray.point);
            }
        }
    }
}
