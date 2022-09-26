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
    }

    public override void OnStateEnter(PlayerController player)
    {
        player.state = PlayerState.WireSearchState;
        player.wireTarget = Vector3.zero;

        Find(player);
    }

    public override void OnStateExcute(PlayerController player)
    {
        
    }

    public override void OnStateExit(PlayerController player)
    {

    }

    private void Find(PlayerController player)
    {
        Collider[] target = Physics.OverlapBox(transform.position + new Vector3(player.lookVector.x * 6.5f, 4f), new Vector3(6f, 5, 1), Quaternion.identity, player.wireLayer);

        if (target.Length > 0)
        {
            var dir = (target[0].transform.position - transform.position).normalized;
            RaycastHit ray;
            if (Physics.Raycast(transform.position, dir, out ray, 100, player.wireLayer))
            {
                player.wireTarget = ray.point;                
            }
        }
    }
}
