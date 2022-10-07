using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Stun_State : FSM_State<Hansel>
{
    static readonly Stun_State instance = new Stun_State();

    public static Stun_State Instance
    {
        get { return instance; }
    }

    private float m_StunTimer = 0;


    static Stun_State() { }
    private Stun_State() {}


    public override void EnterState(Hansel _Hansel)
    {
        //타겟 확인(플레이어)
        if (_Hansel.myTarget == null)
        {
            return;
        }
        _Hansel.Isinvincibility = true;
        _Hansel._isStuned = true;
        m_StunTimer = 0;
        Debug.Log("Hansel stuned... ");

    }

    public override void UpdateState(Hansel _Hansel)
    {

        _Hansel._isStuned = true;
        _Hansel.rb.velocity = Vector3.zero;

        //m_StunTimer += Time.deltaTime;
        //if( m_StunTimer > _Hansel.StunRemainingTime)
        //{
        //    _Hansel.Parti_Stun.Pause();
        //    _Hansel.ChangeState(HanselMove_State.Instance);
        //    _Hansel._isStuned = false;
        //}

        if (_Hansel.CurrentHP >= 80)
        {
            _Hansel.ChangeState(HanselMove_State.Instance);
            return;
        }


    }

    public override void ExitState(Hansel _Hansel)
    {
        _Hansel.Isinvincibility = false;
        _Hansel._isStuned = false;
        return;
    }
}
