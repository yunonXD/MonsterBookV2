using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPlayer_State : FSM_State<Hansel>
{
    static readonly ThrowPlayer_State instance = new ThrowPlayer_State();
    public static ThrowPlayer_State Instance { get { return instance; } }

    static ThrowPlayer_State() { }
    private ThrowPlayer_State() { }

    public override void EnterState(Hansel _Hansel)
    {
        //타겟 확인(플레이어)
        if (_Hansel.myTarget == null)
        {
            return;
        }

        _Hansel.isTP = true;

        int m_Damage = _Hansel.Hansel_SmashDamage;
        _Hansel.TPCollider.GetComponent<ThrowUpCol>().g_Player_To_Damgage = m_Damage;
        _Hansel.TPCollider.GetComponent<ThrowUpCol>().g_Transform = _Hansel.transform;

    }

    public override void UpdateState(Hansel _Hansel)
    {
        //Dead Check
        if (_Hansel.CurrentHP <= 0)
        {
            _Hansel.ChangeState(HanselDie_State.Instance);
        }
    }

    public override void ExitState(Hansel _Hansel)
    {
        _Hansel.isTP = false;
    }

}
