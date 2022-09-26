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
            return;   //�÷��̾� üũ
        }

        Soupcount = Random.Range(_Gretel.SoupMin, _Gretel.SoupMax);
        SoupInterval = (_Gretel.SoupRangePoint2.position.x - _Gretel.SoupRangePoint1.position.x) / Soupcount;
        Debug.LogError("���� ���� : " + Soupcount);
        Debug.LogError("���� ���� : " + SoupInterval);

        return;
    }
    public override void UpdateState(Gretel _Gretel)
    {
        m_AttackTimer += Time.deltaTime;   //���� ��Ÿ�� 

        if(m_AttackTimer >= _Gretel.SoupResponseTime)
        {
            xpos = Random.Range(_Gretel.SoupRangePoint1.position.x + (SoupInterval * SpownCounter),
            _Gretel.SoupRangePoint1.position.x + (SoupInterval * SpownCounter + 1));

            if (SolidFoodCounder == false)
            {
                int x = Random.Range(1, 101);
                Debug.LogError(x);
                if (Soupcount == (SpownCounter + 1))
                {
                    _Gretel.SoupObjectPool.GetComponent<Soup_Object_Pool>().MakeSolidSoup(new Vector3(xpos,
                    _Gretel.SoupRangePoint1.position.y,
                    _Gretel.SoupRangePoint1.position.z));
                    SolidFoodCounder = true;
                    Debug.LogError("Ȯ����÷");
                }

                else if (x <= _Gretel.SolidProbability)
                {
                    _Gretel.SoupObjectPool.GetComponent<Soup_Object_Pool>().MakeSolidSoup(new Vector3(xpos,
                    _Gretel.SoupRangePoint1.position.y,
                    _Gretel.SoupRangePoint1.position.z));
                    SolidFoodCounder = true;
                    Debug.LogError("Ȯ����÷");

                }
                else
                {
                    _Gretel.SoupObjectPool.GetComponent<Soup_Object_Pool>().MakeSoup(new Vector3(xpos,
                    _Gretel.SoupRangePoint1.position.y,
                    _Gretel.SoupRangePoint1.position.z));
                    Debug.LogError("���� �ȳ���");
                }
 
            }
            else
            {
                _Gretel.SoupObjectPool.GetComponent<Soup_Object_Pool>().MakeSoup(new Vector3(xpos,
            _Gretel.SoupRangePoint1.position.y,
            _Gretel.SoupRangePoint1.position.z));
                Debug.LogError("�̹� ����");
            }
            
            SpownCounter++;
            m_AttackTimer = 0;

            if (SpownCounter == Soupcount)   //������ ������ ���� ���������� �������� ������Ʈ ����
            {
                _Gretel.ChangeState(Knife_Attack_State.Instance);
            }
        }

    }
    public override void ExitState(Gretel _Gretel)
    {
        SoupInterval = 0;
        SpownCounter = 0;

    }



}