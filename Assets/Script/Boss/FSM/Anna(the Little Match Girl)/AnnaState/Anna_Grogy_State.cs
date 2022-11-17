using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anna_Grogy_State : FSM_State<Anna>
{
    private bool onetime = false;

    static readonly Anna_Grogy_State instance = new Anna_Grogy_State();
    public static Anna_Grogy_State Instance
    {
        get { return instance; }
    }

    public override void EnterState(Anna _Anna)
    {
        _Anna.Anna_Ani.SetTrigger("Groggy_Start");
        onetime = false;
    }

    public override void ExitState(Anna _Anna)
    {
       
    }

    public override void UpdateState(Anna _Anna)
    {
        if(_Anna.AnnaFalling)
        {
            _Anna.transform.position = new Vector3(_Anna.transform.position.x, _Anna.transform.position.y - _Anna.DownSpeed, _Anna.transform.position.z);
        }

        if(_Anna.GroundLanding == true)
        {
            if(onetime == false)
            {
                _Anna.Anna_Ani.SetTrigger("Landing_Ground");
                _Anna.finishAttackAble = true;
                _Anna.AnnaFalling = false;
                onetime = true;
            }
            else
            {

            }
            
  
        }

    }





}
