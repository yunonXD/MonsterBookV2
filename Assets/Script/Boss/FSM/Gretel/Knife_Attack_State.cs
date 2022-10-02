using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife_Attack_State : FSM_State<Gretel>
{

    static readonly Knife_Attack_State instance = new Knife_Attack_State();
    private Transform KnifeTransform;
    private int cutCount = 0;
    private bool AttackTimer = true;

    public static Knife_Attack_State Instance
    {
        get { return instance; }
    }

    private float m_AttackTimer = 0f;
    private bool KnifeAttack = false;

    public override void EnterState(Gretel _Gretel)
    {
        KnifeAttack = false;
        ResetKnife(_Gretel);
        if (_Gretel.myTarget == null)
        {
            return;   //�÷��̾� üũ
        }


    }
 

    public override void UpdateState(Gretel _Gretel)
    {
        if (_Gretel.CurrentHP <= 0) //���Ȯ��
        {
            //_Gretel.ChangeState(GretelDie_State.Instance);
        }

        if (AttackTimer == true)
        {
            m_AttackTimer += Time.deltaTime;   //����Ÿ�̸�
        }

        if (_Gretel.myTarget)  //Ÿ��Ȯ��
        {
            if (true) //������Ÿ�� Ȯ�ο� ���� 1ȸ�� �����ϱ⋚���� �̻��
            {
                if (m_AttackTimer <= _Gretel.KnifeFollowTime && KnifeAttack == false) //5�ʵ��� �Ѿƴٴ�
                {
                    if (cutCount == _Gretel.KnifeAttackCount)
                    {
                        _Gretel.ChangeState(Soup_Attack_State.Instance);
                    }
                    else
                    {
                        KnifeMove(_Gretel);
                    }
                }

                else if (m_AttackTimer >= _Gretel.KnifeFollowTime && KnifeAttack == false)  
                {
                    KnifeDown(_Gretel);
                }   

                if(KnifeAttack == true)
                {
                    KnifeUp(_Gretel);
                }

                if (_Gretel.KnifeObject.transform.position.y <= 1.0f && KnifeAttack == false)  //�������� �ٴڿ� ������ ���������� -> ��������
                {
                    //���������� ���� �����ʿ�
                    KnifeAttack = true;

                       cutCount++;
                        m_AttackTimer = 0;
                       AttackTimer = false;


                }

                if (_Gretel.KnifeObject.transform.position.y >= 12.4f && AttackTimer == false)
                {

                    KnifeAttack = false;
                    AttackTimer = true;
                }

            }
        }

    }

    public override void ExitState(Gretel _Gretel)
    {
        KnifeAttack = false;
        _Gretel.KnifeObject.SetActive(false);
    }

    void ResetKnife(Gretel _Gretel)
    {
        _Gretel.KnifeObject.transform.position = new Vector3(_Gretel.myTarget.transform.position.x,
                                                   _Gretel.myTarget.transform.position.y + 10,
                                                   _Gretel.myTarget.transform.position.z);
        _Gretel.KnifeObject.SetActive(true);
        cutCount = 0;
        m_AttackTimer = 0f;
    }
    void KnifeMove(Gretel _Gretel)
    {
        _Gretel.KnifeObject.transform.position = new Vector3(Vector3.MoveTowards(_Gretel.KnifeObject.transform.position, _Gretel.myTarget.transform.position, _Gretel.KnifefollowSpeed * Time.deltaTime).x,
            _Gretel.KnifeObject.transform.position.y, _Gretel.KnifeObject.transform.position.z);
    }
    void KnifeDown(Gretel _Gretel)
    {
        _Gretel.KnifeObject.transform.position = new Vector3(_Gretel.KnifeObject.transform.position.x,
                        Vector3.MoveTowards(_Gretel.KnifeObject.transform.position, new Vector3(0, -1, 0), _Gretel.KnifeDownSpeed * Time.deltaTime).y,
                        _Gretel.KnifeObject.transform.position.z);
    }

    void KnifeUp(Gretel _Gretel)
    {
        _Gretel.KnifeObject.transform.position = new Vector3(_Gretel.KnifeObject.transform.position.x,
                        Vector3.MoveTowards(_Gretel.KnifeObject.transform.position, new Vector3(0, 13, 0), _Gretel.KnifeDownSpeed * Time.deltaTime).y,
                        _Gretel.KnifeObject.transform.position.z);
    }


}
