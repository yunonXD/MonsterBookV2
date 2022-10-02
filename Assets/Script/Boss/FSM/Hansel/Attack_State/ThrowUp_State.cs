using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ThrowUp_State : FSM_State<Hansel>
{

    static readonly ThrowUp_State instance = new ThrowUp_State();

    public static ThrowUp_State Instance
    {
        get { return instance; }
    }

    private float ThrowUpTimer = 0;


    static ThrowUp_State() { } 
    private ThrowUp_State() { }



    public override void EnterState(Hansel _Hansel)
    {
        //타겟 확인(플레이어)
        if (_Hansel.myTarget == null)
        {
            return;
        }
       
        int m_Damage = _Hansel.Hansel_ThrowUpDamage;
        _Hansel.ThrowUpCollider.GetComponent<ThrowUpCol>().g_Player_To_Damgage = m_Damage;
        _Hansel.ThrowUpCollider.GetComponent<ThrowUpCol>().g_Transform = _Hansel.transform;

        _Hansel.ThrowUpCollider.GetComponent<ThrowUpCol>().SphereCollider.radius = _Hansel.ThrowUpRange;
        _Hansel.ThrowUpCollider.SetActive(true);
        ThrowUpTimer = 0;

    }

    public override void UpdateState(Hansel _Hansel)
    {
        ThrowUpTimer += Time.deltaTime;
        if (ThrowUpTimer <= _Hansel.ThrowUpTime)
        {
            _Hansel.ThrowUpCollider.SetActive(true);
        }
        else
        {
            _Hansel.ChangeState(HanselMove_State.Instance);
            _Hansel.ThrowUpCollider.SetActive(false);
        }

    }

    public override void ExitState(Hansel _Hansel)
    {
        _Hansel.ThrowUpCollider.SetActive(false);
        return;
    }

}
