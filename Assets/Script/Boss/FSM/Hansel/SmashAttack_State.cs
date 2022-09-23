using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashAttack_State : FSM_State<Hansel>
{
    static readonly SmashAttack_State instance = new SmashAttack_State();
    public static SmashAttack_State Instance
    {
        get { return instance; }
    }
    private float m_AttackTimer = 0f;

    static SmashAttack_State() { }
    private SmashAttack_State() { }


    public override void EnterState(Hansel _Hansel)
    {
        //타겟 확인(플레이어)
        if (_Hansel.myTarget == null)
        {
            return;
        }
        m_AttackTimer = 0;

        int m_Damage = _Hansel.Hansel_SmashDamage;
        _Hansel.SmashCollider.GetComponent<SmashCol>().g_Player_To_Damgage = m_Damage;
        _Hansel.BellyCollider.GetComponent<BellyCol>().g_Transform = _Hansel.transform;
    }


    public override void UpdateState(Hansel _Hansel)
    {
        //Dead Check
        if (_Hansel.CurrentHP <= 0)
        {
            _Hansel.ChangeState(HanselDie_State.Instance);
        }

        m_AttackTimer += Time.deltaTime;
        if(_Hansel.myTarget && _Hansel.CheckRange() && _Hansel.CheckAngle())
        {
            if (m_AttackTimer >= _Hansel.SmashAttackSpeed)
            {              
                

                _Hansel.Ani.SetTrigger("H_SmashAttack");
                _Hansel.SmashCollider.SetActive(true);

                m_AttackTimer = 0;
                _Hansel.ChaseTime = 0.0f;
            }
        }
        else
        {
            _Hansel.ChangeState(HanselMove_State.Instance);
        }
    }

    public override void ExitState(Hansel _Hansel)
    {
        _Hansel.Ani.ResetTrigger("H_SmashAttack");
        //_Hansel.Parti_Strike.Play();
        _Hansel.SmashCollider.SetActive(false);
    }

}
