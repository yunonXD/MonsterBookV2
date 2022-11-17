using UnityEngine;

public class Soup_Attack_State : FSM_State<Gretel>
{

    static readonly Soup_Attack_State instance = new Soup_Attack_State();
    public static Soup_Attack_State Instance
    {
        get { return instance; }
    }

    private float SoupInterval;             //���� ����
    private int SpownCounter = 0;           //������ ���� ī��Ʈ
    private int SoupCount;                  //������ ���� ����
    public bool SolidFoodCounder = false;   //'�Ǵ��Ⱑ ����ִ� ������ �����Ͽ��°�' �Ǵ�
    private bool dropSoup = true;
    private float xpos = 0;
    private Transform GretelTransform;
    private int Lookcount = 0;

    public override void EnterState(Gretel _Gretel)
    {
        GretelTransform = GameObject.FindWithTag("Gretel").transform;
        SolidFoodCounder = false;
        SoupCount = Random.Range(_Gretel.SoupMin, _Gretel.SoupMax);
        _Gretel.SoupResponseTime = 2.1f/ SoupCount;

        SoupInterval = (_Gretel.SoupRangePoint2.position.x - _Gretel.SoupRangePoint1.position.x) / SoupCount;
        _Gretel._Ani.SetBool("SoupAttackStart", true);
    }

    public override void UpdateState(Gretel _Gretel)
    {
        if (_Gretel.RotateTrigger == true)
        {
            LookTarget(_Gretel.LeftLook.transform, _Gretel.CenterLook.transform, GretelTransform, Lookcount);  //��¦ ������ 
        }
        
        if (_Gretel.AttackTimerTrigger == true)
        { 
            _Gretel.m_AttackTimer += Time.deltaTime;

            if (_Gretel.m_AttackTimer >= _Gretel.SoupResponseTime)
            {
                xpos = Random.Range(_Gretel.SoupRangePoint1.position.x + (SoupInterval * SpownCounter),
                _Gretel.SoupRangePoint1.position.x + (SoupInterval * (SpownCounter + 1)));
                dropSoup = false;


                if (SolidFoodCounder == false)
                {
                    int x = Random.Range(1, 101);
                    if (SoupCount == (SpownCounter + 1))
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
                _Gretel.m_AttackTimer = 0;



                if (SpownCounter == SoupCount)   //������ ������ ���� ���������� �������� ������Ʈ ����
                {

                    //Debug.LogError("���� ���� ��������");
                    if(_Gretel.Hansel.GetComponent<Hansel>().CurrentHP <= 0 && _Gretel.Hansel.GetComponent<Hansel>()._isStuned == true)
                    {
                        _Gretel.ChangeState(Protected_State.Instance);
                        Lookcount = 2;
                    }
                    else
                    {

                        Lookcount = 2;
                        _Gretel.ChangeState(Knife_Attack_State.Instance);
                    }
                }
            }
        }
    }

    public override void ExitState(Gretel _Gretel)
    {
        _Gretel.AttackTimerTrigger = false;
        _Gretel._Ani.SetBool("SoupAttackStart", false);
        SoupInterval = 0;
        SpownCounter = 0;
        _Gretel.RotateTrigger = false;

    }

    void LookTarget(Transform Target, Transform Center ,Transform _Gretel,int counter)
    {
        if (counter == 1)
        {
            Vector3 dir = Target.position - _Gretel.transform.position;
            Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);

            if (dirXZ == Vector3.zero)
                return;
            _Gretel.transform.rotation = Quaternion.RotateTowards(_Gretel.transform.rotation, Quaternion.LookRotation(dirXZ), 50 * Time.deltaTime);
        }
        else
        {
            Vector3 dir = Center.position - _Gretel.transform.position;
            Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);

            if (dirXZ == Vector3.zero)
                return;
            _Gretel.transform.rotation = Quaternion.RotateTowards(_Gretel.transform.rotation, Quaternion.LookRotation(dirXZ), 50 * Time.deltaTime);
        }
    }


}
