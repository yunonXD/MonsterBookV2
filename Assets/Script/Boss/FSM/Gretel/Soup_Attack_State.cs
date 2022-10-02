using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soup_Attack_State : FSM_State<Gretel>
{

    static readonly Soup_Attack_State instance = new Soup_Attack_State();
    public static Soup_Attack_State Instance
    {
        get { return instance; }
    }
    private float m_AttackTimer = 0f;
    private int Soupcount;
    private float SoupInterval;
    private int SpownCounter = 0;
    private float xpos = 0;
    public bool SolidFoodCounder = false;


    public override void EnterState(Gretel _Gretel)
    {
        m_AttackTimer = 0f;

        SolidFoodCounder = false;

        if (_Gretel.myTarget == null)
        {
            return;   //플레이어 체크
        }

        Soupcount = Random.Range(_Gretel.SoupMin, _Gretel.SoupMax);
        SoupInterval = (_Gretel.SoupRangePoint2.position.x - _Gretel.SoupRangePoint1.position.x) / Soupcount;

        return;
    }
    public override void UpdateState(Gretel _Gretel)
    {
        m_AttackTimer += Time.deltaTime;   //공격 쿨타임 

        if(m_AttackTimer >= _Gretel.SoupResponseTime)
        {
            xpos = Random.Range(_Gretel.SoupRangePoint1.position.x + (SoupInterval * SpownCounter),
            _Gretel.SoupRangePoint1.position.x + (SoupInterval * SpownCounter + 1));

            if (SolidFoodCounder == false)
            {
                int x = Random.Range(1, 101);
                if (Soupcount == (SpownCounter + 1))
                {
                    _Gretel.SoupObjectPool.GetComponent<Soup_Object_Pool>().MakeSolidSoup(new Vector3(xpos,
                    _Gretel.SoupRangePoint1.position.y,
                    _Gretel.SoupRangePoint1.position.z));
                    SolidFoodCounder = true;
                }

                else if (x <= _Gretel.SolidProbability)
                {
                    _Gretel.SoupObjectPool.GetComponent<Soup_Object_Pool>().MakeSolidSoup(new Vector3(xpos,
                    _Gretel.SoupRangePoint1.position.y,
                    _Gretel.SoupRangePoint1.position.z));
                    SolidFoodCounder = true;

                }
                else
                {
                    _Gretel.SoupObjectPool.GetComponent<Soup_Object_Pool>().MakeSoup(new Vector3(xpos,
                    _Gretel.SoupRangePoint1.position.y,
                    _Gretel.SoupRangePoint1.position.z));
                }
 
            }
            else
            {
                _Gretel.SoupObjectPool.GetComponent<Soup_Object_Pool>().MakeSoup(new Vector3(xpos,
            _Gretel.SoupRangePoint1.position.y,
            _Gretel.SoupRangePoint1.position.z));
            }

            
            SpownCounter++;
            m_AttackTimer = 0;

            if (SpownCounter == Soupcount)   //생성한 스프와 랜덤 스프갯수가 같아지면 스테이트 종료
            {
                if (_Gretel.Hansel.GetComponent<Hansel>().CurrentHP <= 0 && _Gretel.Hansel.GetComponent<Hansel>().PhaseChecker == 2)
                {
                    _Gretel.Hansel.GetComponent<Hansel>().Isinvincibility = true;
                    _Gretel.ProtectiveArea.SetActive(true);
                    _Gretel.ChangeState(Protect_State.Instance);
                }
                else
                {
                    _Gretel.ChangeState(Knife_Attack_State.Instance);
                }

            }
        }

    }
    public override void ExitState(Gretel _Gretel)
    {
        SoupInterval = 0;
        SpownCounter = 0;

    }



}