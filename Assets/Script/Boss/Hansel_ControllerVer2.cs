using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Hansel_ControllerVer2 : MonoBehaviour, IEntity
{

    public enum g_CurrentState {
        idle,
        Chase,
        SmashAttack,
        BellyAttack,
        RushAttack,
        RollingAttack,
    };

    public g_CurrentState curState = g_CurrentState.idle;

    private GameObject m_Player;
    private Transform m_transform;
    private Animator m_animator;
    public Rigidbody m_rb;
    private int CheckPhase = 1;
    private Vector3 m_LookAt;

    private bool _isDead = false;
    private bool _isCount = false;      //Rand Check 
    public  bool _isOnSkill = false;
    public bool _isProtect = false;


    [Header("�⺻ �����Ÿ�")]
    public float ChaseDistance = 10.0f;
    [Header("Smash �Ÿ�")]
    public float AttackDistance = 2.0f;


    [Header("���� �ӵ�")]
    [SerializeField] private int m_MoveSpeed = 4;


    public float g_HanselHP = 100;                      //H_Boss HP
    [SerializeField] private int m_CountHit = 0;        //���ݹ��� Ƚ�� ī��Ʈ
    [SerializeField] private TextMeshProUGUI m_Text;    //�ӽ�. ��ġ�� 2�� ī��Ʈ

    [SerializeField] private GameObject m_Gratel;

    [SerializeField] private Slider Hansel_HPBar;

    [Header("���� -��ų-")]
    [SerializeField] private GameObject m_Smash;
    [SerializeField] private GameObject m_Belly;
    [SerializeField] private GameObject m_Rush;
    [SerializeField] private GameObject m_Rolling;



    private void Start()
    {
        m_transform = GetComponent<Transform>();
        m_animator = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody>();
        m_Player = GameObject.FindWithTag("Player");


        StartCoroutine(CheckState());
        StartCoroutine(CheckStateAction());
        transform.LookAt(m_Player.transform);
        Hansel_HPBar.value = g_HanselHP;
        m_CountHit = 0;
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("BossBuff"))
        {
            Debug.Log("���ļ���");
            StopCoroutine(_SmashAttack());
            StopCoroutine(_RushAttack());
            curState = g_CurrentState.RollingAttack; 
        }
    }




    IEnumerator CheckState()
    {
        while (!_isDead)
        {
            yield return new WaitForSeconds(5.0f);

            float dist = Vector3.Distance(m_Player.transform.position, m_transform.position);


            if (dist <= AttackDistance)
            {
                curState = g_CurrentState.SmashAttack;
            }
            else if (dist >= ChaseDistance)
            {
                curState = g_CurrentState.RushAttack;

            }
            else if (dist <= ChaseDistance)
            {
                curState = g_CurrentState.Chase;
            }
            else
            {
                curState = g_CurrentState.idle;
            }

            //if (_isCount == true)
            //{
            //    CheckRand();
            //}
            if (!m_Player)
            {
                yield break;
            }          
        }
        yield break;
    }

    IEnumerator CheckStateAction()
    {
        while (!_isDead)
        {
            
            switch (curState)
            {
                case g_CurrentState.idle:
                    m_Text.text = "IDLE";

                    m_animator.SetFloat("H_Walk", 0);

                    break;

                case g_CurrentState.Chase:

                    if (!_isOnSkill)
                    {
                        m_Text.text = "Chase";

                        m_LookAt = new Vector3(m_Player.transform.position.x, transform.position.y, transform.position.z);

                        transform.LookAt(m_LookAt);

                        //Vector3 vec = m_Player.transform.position - transform.position;
                        //vec.Normalize();
                        //Quaternion q = Quaternion.LookRotation(vec);
                        //transform.rotation = q;

                        m_animator.SetFloat("H_Walk", 1);
                        transform.position = Vector3.MoveTowards(transform.position, m_Player.transform.position, m_MoveSpeed * Time.deltaTime);

                        //float velocity = m_rb.velocity.normalized.magnitude;
                        //Debug.Log("���� : " + velocity);


                    }
                    break;

                case g_CurrentState.SmashAttack:
                    m_Text.text = "SmashAttack";

                    StartCoroutine(_SmashAttack());

                    break;

                case g_CurrentState.BellyAttack:
                    m_Text.text = "BellyAttack";

                    StartCoroutine(_BellyAttack());
                    m_animator.SetFloat("H_Walk", 0);

                    break;

                case g_CurrentState.RushAttack:
                    m_Text.text = "RushAttack";

                    //StartCoroutine(_RushAttack());
                    m_animator.SetFloat("H_Walk", 0);

                    m_Rush.SetActive(true);

                    curState = g_CurrentState.idle;

                    break;

                case g_CurrentState.RollingAttack:
                    m_Text.text = "RollingAttack";

                    StartCoroutine(_RollingAttack());

                    break;

            }
            yield return null;
        }
        yield break;
    }

    IEnumerator CountBelly(float bellycool)
    {
        while (bellycool > 0.0f)
        {
            bellycool -= Time.deltaTime;
            string txt = bellycool.ToString("0.0");
            m_Text.text = "��ġ�� ��ų �ð�:" + txt;
            yield return new WaitForFixedUpdate();

            if (bellycool <= 0)
            {
                m_CountHit = 0;
                yield break;
            }

            if (m_CountHit >= 4)
            {
                Debug.Log("4 HIt!!");
                curState = g_CurrentState.BellyAttack;
                m_CountHit = 0;

            }
        }
    }

    IEnumerator _SmashAttack()
    {
        m_animator.SetFloat("H_Walk", 0);
        m_rb.velocity = Vector3.zero;
        m_Smash.SetActive(true);
        _isCount = true;
 
        yield return new WaitForSeconds(2.0f);

        curState = g_CurrentState.idle;
        _isCount = false;
        yield break;

    }

    IEnumerator _BellyAttack()
    {
        m_Belly.SetActive(true);
        StopCoroutine(CheckState());
        yield return new WaitForSeconds(2.0f);

        if (Vector3.Distance(transform.position, m_Player.transform.position) <= 2)
        {
            curState = g_CurrentState.SmashAttack;
        }
        yield return new WaitForSeconds(2.0f);

        StartCoroutine(CheckState());
    }

    IEnumerator _RushAttack()
    {
        StopCoroutine(CheckState());
        StopCoroutine(_SmashAttack());
        m_Rush.SetActive(true);
        yield return new WaitForSeconds(6.5f);
        StartCoroutine(CheckState());
        yield break;
    }

    IEnumerator _RollingAttack()
    {
        m_Rolling.SetActive(true);

        yield return new WaitForSeconds(5.0f);

        curState = g_CurrentState.idle;
        yield break;
    }

    private void CheckRand()
    {
        int A = Random.Range(1, 101);
        if (A <= 30)
        {
            curState = g_CurrentState.RushAttack;
        }
    }


    public void OnDamage(int Playerdamage, Vector3 pos)
    {
        Debug.Log("Call Damage To Boss");
        PhaseChecker();
        Hansel_is_Dead();

        StartCoroutine(CountBelly(2.0f));

        if (!_isProtect)
        {
            g_HanselHP -= Playerdamage;
            Hansel_HPBar.value = g_HanselHP;
            m_CountHit++;
        }
        else
        {
            Debug.Log(" �����ؼ����� ������ ���� ");
        }
    }

    public void OnRecovery(int heal)
    {

    }

    private void PhaseChecker()
    {
        //HP �� ������ 1�ܰ����� Ȯ���ϰ� �������� �ѱ�� ��Ʈ
        if (g_HanselHP <= 20 && CheckPhase == 1)
        {
            CheckPhase = 2;
            m_animator.SetTrigger("H_Phase2");
            Debug.Log("Hansel in Phase 2");

            Phase2();
        }
    }

    private void Phase2()
    {
        //������ 2�� �����Ѵٸ� Hensel �� HP ���� �� Gratel ��ȯ
        g_HanselHP = 100;
        Hansel_HPBar.value = g_HanselHP;
        m_Gratel.SetActive(true);

    }

    private void Hansel_is_Dead()
    {
        //Hansel Dead ���� üũ
        if (g_HanselHP <= 0)
        {
            _isDead = true;
            Debug.Log("Hansel is DEAD");

        }
    }

    public void OnSkillDamage(float SkillDamage)
    {
        //���� ��� ����
    }

}
