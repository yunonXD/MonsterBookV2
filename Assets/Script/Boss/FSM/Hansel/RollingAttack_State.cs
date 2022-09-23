using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//WayPoiner 로 구현 

public class RollingAttack_State : FSM_State<Hansel>
{
    static readonly RollingAttack_State instance = new RollingAttack_State();

    public static RollingAttack_State Instance
    {
        get { return instance; }
    }

    //음식 먹기 딜레이
    private float m_WaitForFood = 0;
    //현재 노드
    private int m_CountPointer = 0;

    static RollingAttack_State() { }
    private RollingAttack_State() { }
 

    public override void EnterState(Hansel _Hansel)
    {
        //타겟 확인(플레이어)
        if (_Hansel.myTarget == null)
        {
            return;
        }
        m_CountPointer = 0;
        m_WaitForFood = 0;

        int m_Damage = _Hansel.Hansel_RollingDamage;
        _Hansel.RollingCollider.GetComponent<RollingCol>().g_Player_To_Damgage = m_Damage;
        _Hansel.BellyCollider.GetComponent<BellyCol>().g_Transform = _Hansel.transform;

        _Hansel.Ani.SetTrigger("H_RollingAttackR");
        _Hansel.isRolling = true;
    }

    public override void UpdateState(Hansel _Hansel)
    {
        //Dead Check
        if (_Hansel.CurrentHP <= 0)
        {
            _Hansel.ChangeState(HanselDie_State.Instance);
        }

        m_WaitForFood += Time.deltaTime;
        if (m_WaitForFood >= _Hansel.RollingWaitTime)
        {
            _Hansel.RollingCollider.SetActive(true);
            _Hansel.transform.position = Vector3.MoveTowards(
    _Hansel.transform.position, _Hansel.Rolling_Position[m_CountPointer].transform.position,
     _Hansel.RollingSpeed * Time.deltaTime);


            _Hansel.Ani.SetBool("H_RollingAttack", true);


            _Hansel.transform.LookAt(_Hansel.Rolling_Position[m_CountPointer].transform);

            if (_Hansel.transform.position == _Hansel.Rolling_Position[m_CountPointer].transform.position)
            {
                m_CountPointer++;
            }

            if (m_CountPointer == _Hansel.Rolling_Position.Length)
            {
                Debug.Log("고갱님 종착역 입니다~");

                _Hansel.Ani.SetBool("H_RollingAttack", false);
                _Hansel.ChangeState(HanselMove_State.Instance);
                return;

            }
        }

    }

    public override void ExitState(Hansel _Hansel)
    {
        _Hansel.isRolling= false;
        _Hansel.Ani.SetBool("H_RollingAttack", false);
        _Hansel.RollingCollider.SetActive(false);
    }
}
