using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Protect_State : FSM_State<Gretel>
{

    static readonly Protect_State instance = new Protect_State();
    public GameObject m_Hansel;
    public static Protect_State Instance
    {
        get { return instance; }
    }


    private float m_HitTimer = 0;

    public override void EnterState(Gretel _Gretel)
    {
        //프로텍트 애니메이션 재생
        _Gretel.ProtectiveArea.SetActive(true);
        m_Hansel = GameObject.FindWithTag("Boss");
    }

    public override void ExitState(Gretel _Gretel)
    {
        m_HitTimer = 0;
    }

    public override void UpdateState(Gretel _Gretel)
    {
        m_HitTimer += Time.deltaTime;   
        if(m_HitTimer >= _Gretel.HitTime)
        {
            m_HitTimer = 0;
            _Gretel.ProtectiveArea.SetActive(false);
            m_Hansel.GetComponent<Hansel>().CurrentHP = 100;
            _Gretel.ChangeState(Knife_Attack_State.Instance);    
        }

    }



}
