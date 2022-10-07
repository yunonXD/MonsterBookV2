using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BellyAttack_State : FSM_State<Hansel>
{
    static readonly BellyAttack_State instance = new BellyAttack_State();

    public static BellyAttack_State Instance
    {
        get {
            return instance; 
        } 
    }
    private float m_AttackTimer = 0f;
    private float m_AttackEndTimer = 0f;
    static BellyAttack_State() { }
    private BellyAttack_State() { }


    public override void EnterState(Hansel _Hansel)
    {
        //타겟 확인(플레이어)
        if (_Hansel.myTarget == null || _Hansel.Col_with_Wall)
        {
            Debug.Log("배치기 앞에 벽이 있거나 플레이어가 없음을 인지");
            return;
        }

        #region Damage Collider
        int m_Damage = _Hansel.Hansel_BellyDamage;
        _Hansel.BellyCollider.GetComponent<BellyCol>().g_Player_To_Damgage = m_Damage;
        _Hansel.BellyCollider.GetComponent<BellyCol>().g_Transform = _Hansel.transform;
        _Hansel.BellyCollider.GetComponent<BellyCol>().g_PlayerTransform = _Hansel.myTarget.transform;

        _Hansel.BellyCollider.GetComponent<BellyCol>().g_BellyForce = _Hansel.BellyForce;
        _Hansel.BellyCollider.GetComponent<BellyCol>().g_BellyForceUp = _Hansel.BellyForceUp;
        #endregion

        _Hansel.OnDircalculator(1);

        _Hansel.Ani.SetTrigger("H_BellyAttack");

        _Hansel.BellyCollider.SetActive(true);
        m_AttackTimer = 0;
        m_AttackEndTimer = 0;
        _Hansel.isBelly = true;
    }

    public override void UpdateState(Hansel _Hansel)
    {

        //Dead Check
        if (_Hansel.CurrentHP <= 0)
        {
            _Hansel.ChangeState(HanselDie_State.Instance);
        }

        
        if (_Hansel.myTarget && _Hansel.isBelly == true)
        {
            m_AttackTimer += Time.fixedDeltaTime;
            if (m_AttackTimer <= _Hansel.BellyAttackSpeed)
            {              
                _Hansel.rb.AddForce(_Hansel.transform.forward * _Hansel.BellyPower, ForceMode.Impulse);
            }
            else
            {
                _Hansel.isBelly = false;
                _Hansel.ChaseTime = 0.0f;
            }
        }
        else
        {
            m_AttackEndTimer += Time.fixedDeltaTime;
            if(m_AttackEndTimer >= _Hansel.BellyAttackSpeed + 1)
            {
                m_AttackEndTimer = 0;
                _Hansel.ChangeState(HanselMove_State.Instance);

            }

        }
    }

    public override void ExitState(Hansel _Hansel)
    {
        //_Hansel.rb.velocity = Vector3.zero;
        _Hansel.BellyCollider.SetActive(false);
        _Hansel.Ani.ResetTrigger("H_BellyAttack");
    }
}
