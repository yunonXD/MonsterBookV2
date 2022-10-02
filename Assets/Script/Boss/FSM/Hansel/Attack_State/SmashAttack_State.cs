using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEngine;

public class SmashAttack_State : FSM_State<Hansel>
{
    static readonly SmashAttack_State instance = new SmashAttack_State();
    public static SmashAttack_State Instance
    {
        get { return instance; }
    }
    private bool isAttack_1 = false;
    private bool isAttack_2 = false;
    private bool isAttack_3 = false;
    private bool isAttack_4 = false;

    static SmashAttack_State() { }
    private SmashAttack_State() { }


    public override void EnterState(Hansel _Hansel)
    {
        //타겟 확인(플레이어)
        if (_Hansel.myTarget == null)
        {
            return;
        }

        int m_Damage = _Hansel.Hansel_SmashDamage;
        _Hansel.SmashCollider.GetComponent<SmashCol>().g_Player_To_Damgage = m_Damage;
        _Hansel.SmashCollider.GetComponent<SmashCol>().g_Transform = _Hansel.transform;
        _Hansel.rb.velocity = Vector3.zero;
        _Hansel.isSmash = true;


        if (_Hansel.isSmashBool)
        {
            _Hansel.RandCalculateForSmash(5);
            _Hansel.isSmashBool = false;
        }


        switch (_Hansel.ForSmashP)
        {
            case 1:
                isAttack_1 = true;
                _Hansel.Ani.SetTrigger("H_SmashAttack");
                _Hansel.SmashCollider.SetActive(true);
                break;

            case 2:
                isAttack_2 = true;
                _Hansel.Ani.SetTrigger("H_SmashAttack2");
                _Hansel.SmashCollider.SetActive(true);
                break;

            case 3:
                isAttack_3 = true;
                _Hansel.Ani.SetTrigger("H_SmashAttack3");
                _Hansel.SmashCollider.SetActive(true);
                break;

            case 4:
                isAttack_4 = true;
                _Hansel.Ani.SetTrigger("H_SmashAttack4");
                _Hansel.SmashCollider.SetActive(true);
                break;
        }

    }


    public override void UpdateState(Hansel _Hansel)
    {
        //Dead Check
        if (_Hansel.CurrentHP <= 0)
        {
            _Hansel.ChangeState(HanselDie_State.Instance);
        }


        if (_Hansel.myTarget && _Hansel.isSmash)
        {
            switch (_Hansel.ForSmashP)
            {
                case 1:

                    if (_Hansel.Attack_Time >= 4)
                    {
                        _Hansel.isSmashBool = true;
                        isAttack_1 = false;
                        _Hansel.isSmash = false;
                        _Hansel.Attack_Time = 0;
                        return;
                    }

                    break;

                case 2:
 
                    if (_Hansel.Attack_Time >= 4)
                    {
                        _Hansel.isSmashBool = true;
                        isAttack_2 = false;
                        _Hansel.isSmash = false;
                        _Hansel.Attack_Time = 0;
                        return;
                    }
                    break;


                case 3:
                    if (_Hansel.Attack_Time >= 4)
                    {

                        _Hansel.isSmashBool = true;
                        isAttack_3 = false;
                        _Hansel.isSmash = false;
                        _Hansel.Attack_Time = 0;
                        return;
                    }
                    break;

                case 4:
                    if (_Hansel.Attack_Time >= 4)
                    {
                        _Hansel.isSmashBool = true;
                        isAttack_4 = false;
                        _Hansel.isSmash = false;
                        _Hansel.Attack_Time = 0;
                        return;
                    }
                    break;

                default:
                    break;

            }
            return;

        }
        else if (!_Hansel.CheckRange()  || (!_Hansel.isSmash &&
            !isAttack_1 && !isAttack_2 && !isAttack_3 && !isAttack_4))
        {
            _Hansel.rb.velocity = Vector3.zero;
            _Hansel.ChangeState(HanselMove_State.Instance);
        }

    }

    public override void ExitState(Hansel _Hansel)
    {

        _Hansel.isSmash = false;

        _Hansel.Ani.ResetTrigger("H_SmashAttack");
        _Hansel.Ani.ResetTrigger("H_SmashAttack2");
        _Hansel.Ani.ResetTrigger("H_SmashAttack3");
        _Hansel.Ani.ResetTrigger("H_SmashAttack4");

        _Hansel.SmashCollider.SetActive(false);
        _Hansel.isSmashBool = true;
        return;
    }

}
