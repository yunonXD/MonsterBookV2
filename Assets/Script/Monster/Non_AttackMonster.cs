using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterFSM;
using MonsterFSM.RollRollFSM;

public abstract class Non_AttackMonster : MonsterBase
{
    protected MonsterFSMBase FSM;
    protected Rigidbody rigd;

    protected virtual void Start()
    {
        rigd = GetComponent<Rigidbody>();
    }


    protected virtual void Update()
    {
        if (FSM != null)
        {
            FSM.UpdateExecute();
            var Trans = FSM.Transition();
            if (Trans != null)
                FSM = FSM.Transition();
        }
    }
    protected virtual void FixedUpdate()
    {
        if (FSM != null)
        {
            FSM.FixedExecute(rigd);
        }

    }        


}
