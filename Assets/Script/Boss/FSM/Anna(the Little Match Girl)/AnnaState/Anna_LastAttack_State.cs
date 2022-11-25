using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anna_LastAttack_State : FSM_State<Anna>
{
    static readonly Anna_LastAttack_State instance = new Anna_LastAttack_State();
    public static Anna_LastAttack_State Instance
    {
        get { return instance; }
    }

    private bool onetime = false;

    public override void EnterState(Anna _Anna)
    {
        _Anna.Anna_Ani.Play("Idle");
        _Anna.Isinvincibility = true;
        onetime = false;
        _Anna.WorldEvent.GetComponent<Anna_WorldEvent>().LastMatch();

    }

    public override void ExitState(Anna _Anna)
    {
       
    }

    public override void UpdateState(Anna _Anna)
    {
        if(_Anna.LastMatchClear == 5)
        {
            if(onetime == false)
            {
                _Anna.ChangeState(Anna_Grogy_State.Instance);
                onetime = true;
            }
            
        }
    }

}
