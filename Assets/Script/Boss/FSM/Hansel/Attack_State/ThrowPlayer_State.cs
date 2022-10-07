using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ThrowPlayer_State : FSM_State<Hansel>
{
    static readonly ThrowPlayer_State instance = new ThrowPlayer_State();
    public static ThrowPlayer_State Instance { get { return instance; } }


    private float m_WaitTimer = 0;

    static ThrowPlayer_State() { }
    private ThrowPlayer_State() { }

    public override void EnterState(Hansel _Hansel)
    {
        //타겟 확인(플레이어)
        if (_Hansel.myTarget == null)
        {
            return;
        }
        
        m_WaitTimer = 0;

        #region Damage Collider
        int m_Damage = _Hansel.Hansel_ThrowPlayer;
        _Hansel.SmashCollider_R.GetComponent<SmashCol>().g_Player_To_Damgage = m_Damage;
        _Hansel.SmashCollider_R.GetComponent<SmashCol>().g_Transform = _Hansel.transform;

        _Hansel.SmashCollider_L.GetComponent<SmashCol>().g_Player_To_Damgage = m_Damage;
        _Hansel.SmashCollider_L.GetComponent<SmashCol>().g_Transform = _Hansel.transform;

        #endregion

        _Hansel.isTP = true;
        _Hansel.Ani.SetTrigger("H_ThrowPlayer");

        _Hansel.myTarget.GetComponent<PlayerController>().SetThrowState(true);
        _Hansel.myTarget.GetComponent<CapsuleCollider>().isTrigger = true;

        _Hansel.m_MyTartgetRot = _Hansel.myTarget.transform.rotation;


    }

    public override void UpdateState(Hansel _Hansel)
    {
        //Dead Check
        if (_Hansel.CurrentHP <= 0)
        {
            _Hansel.ChangeState(HanselDie_State.Instance);
        }
        _Hansel.myTarget.transform.rotation = _Hansel.m_MyTartgetRot;

        if (_Hansel.TP_Throwing)
        {          
            m_WaitTimer += Time.fixedDeltaTime;
            if (m_WaitTimer >= 1)
            {
                _Hansel.TP_Throwing = false;
                _Hansel.ChangeState(HanselMove_State.Instance);
            }
        }


    }

    public override void ExitState(Hansel _Hansel)
    {
        _Hansel.myTarget.transform.position = new Vector3(_Hansel.myTarget.transform.position.x, _Hansel.myTarget.transform.position.y, 0);

        _Hansel.isTP = false;
    }

}
