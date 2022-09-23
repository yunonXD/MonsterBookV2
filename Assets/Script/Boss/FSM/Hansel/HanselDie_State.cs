using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanselDie_State : FSM_State<Hansel>
{
    static readonly HanselDie_State instance = new HanselDie_State();

    public static HanselDie_State Instance
    {
        get { return instance; }
    }
    //private float Count = 2.0f;
    //private float time = 0f;

    static HanselDie_State() { }
    private HanselDie_State() { }


    public override void EnterState(Hansel _Hansel)
    {
        _Hansel.GetComponent<Animator>().SetTrigger("H_Dead");
        _Hansel.IsDead = true;
    }
    public override void UpdateState(Hansel _Hansel)
    {

    }

    public override void ExitState(Hansel _Hansel)
    {
        
    }


}
