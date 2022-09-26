using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class RushAttack_State : FSM_State<Hansel>
{
    static readonly RushAttack_State instance = new RushAttack_State();

    public static RushAttack_State Instance
    {
        get
        {
            return instance;
        }
    }

    private float lastLosWaitTime = 0;

    static RushAttack_State() { }
    private RushAttack_State() { }

    public override void EnterState(Hansel _Hansel)
    {

        //타겟 확인(플레이어)
        if (_Hansel.myTarget == null)
        {
            return;
        }

        if(_Hansel.Col_with_Wall == true)
        {
            _Hansel.ChangeState(HanselMove_State.Instance);
        }
        else
        {
            int m_Damage = _Hansel.Hansel_RushDamage;
            _Hansel.RushCollider.GetComponent<RushCol>().g_Player_To_Damgage = m_Damage;
            _Hansel.BellyCollider.GetComponent<BellyCol>().g_Transform = _Hansel.transform;


            _Hansel.RushCollider.SetActive(true);
            _Hansel.Ani.SetTrigger("H_RushAttackR");
            _Hansel.isRushing = true;
        }
    }

    public override void UpdateState(Hansel _Hansel)
    {

        //Dead Check
        if (_Hansel.CurrentHP <= 0)
        {
            _Hansel.ChangeState(HanselDie_State.Instance);
        }

        if (_Hansel.myTarget && _Hansel.Col_with_Wall == false && _Hansel.isRushing == true)
        {
            _Hansel.Parti_Rush.Play();
            _Hansel.Ani.SetBool("H_RushAttack_Loop", true);
            _Hansel.rb.AddForce(_Hansel.transform.forward * _Hansel.Rush_Speed, ForceMode.Acceleration);
        }
        else
        {
            lastLosWaitTime += Time.deltaTime;
            _Hansel.Parti_Rush.Stop();
            _Hansel.Ani.SetBool("H_RushAttack_Loop", false);
            _Hansel.RushCollider.SetActive(false);
            _Hansel.rb.velocity = Vector3.zero;

            if (lastLosWaitTime >= _Hansel.Rush_EndWait)
            {
                _Hansel.isRushing = false;    
                _Hansel.ChangeState(HanselMove_State.Instance);
                lastLosWaitTime = 0;
                _Hansel.ChaseTime = 0.0f;
            }
        }
    }

    public override void ExitState(Hansel _Hansel)
    {
        _Hansel.Parti_Rush.Stop();

    }
}