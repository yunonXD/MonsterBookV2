using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _Hansel.Parti_Stun.Play();
        m_StunTimer = 0;

    }

    public override void UpdateState(Hansel _Hansel)
    {
        Debug.Log("Hansel stuned... ");
        _Hansel._isStuned = true;

        m_StunTimer += Time.deltaTime;
        if( m_StunTimer > _Hansel.StunRemainingTime)
        {
            _Hansel.Parti_Stun.Pause();
            _Hansel.ChangeState(HanselMove_State.Instance);
            _Hansel._isStuned = false;
        }

    }

    public override void ExitState(Hansel _Hansel)
    {
        return;
    }
}
