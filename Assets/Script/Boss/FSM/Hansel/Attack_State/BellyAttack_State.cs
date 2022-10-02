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
    static BellyAttack_State() { }
    private BellyAttack_State() { }


    public override void EnterState(Hansel _Hansel)
    {
        //Ÿ�� Ȯ��(�÷��̾�)
        if (_Hansel.myTarget == null)
        {
            return;
        }
        if (_Hansel.Col_with_Wall == true)
        {
            return;
        }


        int m_Damage = _Hansel.Hansel_BellyDamage;
        _Hansel.BellyCollider.GetComponent<BellyCol>().g_Player_To_Damgage = m_Damage;
        _Hansel.BellyCollider.GetComponent<BellyCol>().g_Transform = _Hansel.transform;
        _Hansel.BellyCollider.GetComponent<BellyCol>().g_PlayerTransform = _Hansel.myTarget.transform;

        _Hansel.BellyCollider.GetComponent<BellyCol>().g_BellyForce = _Hansel.BellyForce;
        _Hansel.BellyCollider.GetComponent<BellyCol>().g_BellyForceUp = _Hansel.BellyForceUp;



        _Hansel.rb.velocity = Vector3.zero;
        m_AttackTimer = 0;
        _Hansel.isBelly = true;
    }

    public override void UpdateState(Hansel _Hansel)
    {

        //Dead Check
        if (_Hansel.CurrentHP <= 0)
        {
            _Hansel.ChangeState(HanselDie_State.Instance);
        }

        m_AttackTimer += Time.deltaTime;
        if (_Hansel.myTarget && _Hansel.isBelly == true)
        {
            if (m_AttackTimer <= _Hansel.BellyAttackSpeed && _Hansel.isBelly == true)
            {
                _Hansel.BellyCollider.SetActive(true);
                _Hansel.rb.AddForce(_Hansel.transform.forward * _Hansel.BellyPower, ForceMode.Force);
            }
            else
            {
                _Hansel.isBelly = false;
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
        _Hansel.rb.velocity = Vector3.zero;
        _Hansel.BellyCollider.SetActive(false);
    }
}
