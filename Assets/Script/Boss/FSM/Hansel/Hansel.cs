using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



public class Hansel : MonoBehaviour, IEntity
{

    [HideInInspector] public float HanselHP = 100;
    [Header("���� Hanse HP")] public float CurrentHP = 100;
    public GameObject myTarget;
    public GameObject Gretel;

    [Header("=====================Smash")]

    #region  SmashAttack var

    [Header("Smash ������")]
    public int Hansel_SmashDamage = 10;

    [Header("Smash ���� �ּҰŸ�")]
    public float SmashAttackRange = 2f;

    [Header("Smash ���� ������")]
    public float SmashAttackSpeed = 2.0f;

    //Smash �� ����� �ָԿ� ���� Col
    public GameObject SmashCollider;

    #endregion

    [Header("=====================Rush")]

    #region RushAttack Var



    [Header("Rush ������")]
    public int Hansel_RushDamage = 10;

    [Header("Rush �ӵ�")]
    public int Rush_Speed = 5;

    [Header("Rush ������ ���ð�")]
    public int Rush_EndWait = 4;

    public GameObject RushCollider;                         //Rush�� ����� Col   
    public bool isRushing = false;                          //Rush ������ üũ
    [HideInInspector] public bool Col_with_Wall = false;

    #endregion

    [Header("=====================Belly")]

    #region BellyAttack var

    [Header("Belly ������")]
    public int Hansel_BellyDamage = 10;

    [Header("Belly ���ӽð�")]
    public float BellyAttackSpeed = 2;

    [Header("Belly ���� - Force")]
    public float BellyPower = 4;

    [Header("���� - �÷��̾� �浹 �ð�")]
    [SerializeField]private float m_BellyColTime = 0;

    [Header("�浹 n �� �̻��̸� �ߵ���ų �ð�")]
    public int SetBellyTime = 4;


    [Header("���� ī��Ʈ�� n �� - ���콺 �÷�����")]
    [Tooltip("�÷��̾�� ���ݴ����� �� BellyDelay �� �ȿ� m_CountHit ��ŭ ���ݹ����� Belly ���� ����")]
    [SerializeField] private int BellyDelay = 2;

    public GameObject BellyCollider;                         //Belly�� ����� Col 
    public bool isBelly = false;
    private bool m_CanBelly = false;
    private int m_CountHit = 0;                              //���� ���� Ƚ�� ī��Ʈ
    private float m_CurrentTime = 0;
    private int m_bellyTimer = 0;
    #endregion

    [Header("=====================Rolling")]

    #region RollingAttack var

    [Header("Rolling ������")]
    public int Hansel_RollingDamage = 10;

    [Header("�����ٴϱ� WayPoiner")]
    public Transform[] Rolling_Position;

    [Header("�����ٴϱ� �ӵ�")]
    public float RollingSpeed = 3;

    [Header("���� ������ ���ð�")]
    public float RollingWaitTime = 4;

    [Header("�������ִ��� üũ")]
    public bool isRolling = false;


    public GameObject RollingCollider;                         //Rolling�� ����� Col 

    #endregion

    [Header("=====================ThrowUp")]

    #region ThrowUp

    [Header("Throw up ������")]
    public int Hansel_ThrowUpDamage = 10;

    [Header("Throw up ����")]
    public int ThrowUpRange = 5;

    [Header("Throw up ���ӽð�")]
    public int ThrowUpTime = 3;

    [Header("Throw up �ߵ� Ȯ��")]
    [SerializeField] private int m_ThrowUpActivePercent = 30;

    public GameObject ThrowUpCollider;

    #endregion


    [Header("=====================movement")]

    #region Movement var

    [Header("���� ���� �ð�")]
    public float ChaseCancleTime = 5.0f;
    [Header("���� �����ð�")]
    public float ChaseTime = 0;

    #endregion

    [Header("=====================Stun")]

    #region Stun var

    [Header("���� ���ӽð�")] public int StunRemainingTime = 3;
    [HideInInspector] public bool _isStuned = false;

    #endregion

    [Header("=====================Effect")]

    #region Effect
    [Header("����Ʈ")]
    public ParticleSystem Parti_Rush;
    public ParticleSystem Parti_Strike;
    public ParticleSystem Parti_Stun;
    public ParticleSystem Parti_Inflate;
    public ParticleSystem Parti_Brup;
    public ParticleSystem Parti_Damage;

    #endregion

    #region no Touch

    private StateMachine<Hansel> state = null;
    [HideInInspector] public Animator Ani = null;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public bool IsDead = false;                                       //���� üũ
    [HideInInspector] public bool Isinvincibility = false;                              //������ üũ
    [HideInInspector] public bool isAttacked = true;                                    //���� �޾Ҵ��� üũ
    [HideInInspector] public int PhaseChecker = 1;                                      //1������ 2������ üũ
   
    #endregion

    [Header("=====================Rush % instance")]

    #region Random Rush instance

    [Header("�÷��̾� �۶����½ð�")] public float TimerCount = 5;                        //�󸶸�ŭ ������ ������ Rush ����(����) ���� ..
    [SerializeField][Header("�۶������� Rush �ߵ� Ȯ��")] private int m_ForRushP = 30;  //TimerCount ��ŭ ������ �־��� �� Rush �� �ߵ��� Ȯ��
    [SerializeField][Header("�Ϲݰ��� ���� ������ �ߵ��� Ȯ��")] private int m_ForRushInsSmash = 30;

    #endregion

    #region ==Debug==

    [Header("=====================")]
    [Header("==�����== �����̽� : ������ // ���콺 ��Ŭ�� : ���� // ���콺 �� : ��ġ��")]
    [SerializeField] private bool m_isDebug = false;

    #endregion

    #region checkVelocity
    private Vector3 m_oldPosition;
    private Vector3 m_CurrentPosition;
    private double m_Velocity;
    public double g_Speed { get { return m_Velocity; } set { g_Speed = m_Velocity; } }
    #endregion

    //==============================================================//

    void Awake()
    {
        ResetState();
        //m_oldPosition = transform.position; Velocity ������

        myTarget = GameObject.FindGameObjectWithTag("Player");
        Gretel = GameObject.FindGameObjectWithTag("Gretel");
    }

    void Start()
    {
        StartCoroutine(CountTimer(TimerCount));

        Ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        state.Update();
        RayCheckWall();
        RayCheckPlayer();
        //======================�����=====================
        if (m_isDebug)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeState(RollingAttack_State.Instance);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ChangeState(RushAttack_State.Instance);
            }

            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                ChangeState(BellyAttack_State.Instance);
            }

            if (Input.GetKeyDown(KeyCode.Mouse3))
            {
                ChangeState(Stun_State.Instance);
            }


            if (Input.GetKeyDown(KeyCode.Mouse4))
            {
                ChangeState(ThrowUp_State.Instance);
            }
        }
        //======================�����=====================
    
    }


    public void ChangeState(FSM_State<Hansel> _State)
    {
        state.ChangeState(_State);
    }       // ���º���


    #region OnTrigger
    private void OnTriggerEnter(Collider col)
    {
        // ���� , ������ , ���ϻ��� , ��ġ�Ⱑ �ƴ϶��
        // 30 % Ȯ�� + ���� ������� �ʴٸ� Rush ���� , �ش� ������ �ƴ϶�� �������� �Ѿ�� 30% Ȯ���� ����.
        // �͵� �ƴϸ� ���޽� ����.
        // �÷��̾� ����� �� or ���������� ����� �� ,,�� ���� �����ص�
        if (!isRushing && !isRolling && !_isStuned && !isBelly)  //rush / rolling / Stun �̸� �۵�����
        {
            if (col.gameObject.tag == "Player")
            {
                myTarget = col.gameObject;

                if (CheckRange())   //�÷��̾� ~ ���� �Ÿ�����
                {
                    if (RandCalculate(m_ForRushInsSmash) == true && Col_with_Wall == false)
                    {
                        ChangeState(RushAttack_State.Instance);
                    }
                    else if (RandCalculate(m_ThrowUpActivePercent) == true && PhaseChecker == 2)
                    {
                        Debug.Log("����");
                        ChangeState(ThrowUp_State.Instance);
                    }
                    else
                    {
                        ChangeState(SmashAttack_State.Instance);
                    }
                }
                else if (!CheckRange())
                {
                    ChangeState(HanselMove_State.Instance);
                }
            }
            else if (col.CompareTag("BossBuff"))
            {
                Debug.Log("���� ���� �ܰ� ����");
                ChangeState(RollingAttack_State.Instance);
            }

        }
    }

#endregion

    public bool CheckRange()        // Smash �Ÿ�������
    {
        if (Vector3.Distance(myTarget.transform.position, transform.position) <= SmashAttackRange)
        {
            return true;
        }
        return false;
    }

    public bool CheckAngle()        //Smash ����������
    {
        if (Vector3.Dot(myTarget.transform.position, transform.position) >= 0.5f)
        {
            return true;
        }
        return false;
    }

    public void ResetState()        //Hansel ����
    {
        CurrentHP = HanselHP;
        state = new StateMachine<Hansel>();

        state.Initial_Setting(this, HanselMove_State.Instance);

        // Reset Target
        // myTarget = null;
    }     

    public void inPhaseTwo()        //������2 ����
    {
        ResetState();
        Ani.SetTrigger("H_Phase2");
        PhaseChecker++;

        Gretel.SetActive(true);

#if DEBUG
        Debug.Log("Phase 2 XD");
#endif
    }       

    public bool RandCalculate(int RandNum)     //���� % ���(Idle �� Smash ����)
    {
        int m_rand = UnityEngine.Random.Range(1, 101);
#if DEBUG
        //Debug.Log("���� �� : " + m_rand);
#endif


        if (m_rand <= RandNum)
        {
            return true;
        }
        else
            return false;
    }


    #region CoroutineTimer/Belly
   
    public IEnumerator CountTimer(float m_Timer)                //���� �ð� ���� �޴��� üũ
    {
        yield return new WaitForSeconds(m_Timer);

        //���� ���� �ʾҰ� 30% �̻��̶�� (���ѹݺ�)
        if (isAttacked == false && RandCalculate(m_ForRushP) == true && !isRolling && !isBelly)
        {
            ChangeState(RushAttack_State.Instance);
        }
        isAttacked = false;
        m_CountHit = 0;

        StartCoroutine(CountTimer(TimerCount));
    }

    public IEnumerator CountBelly(int LoadTime)                 //2�� �ȿ� 4ȸ ���� ���� ���� �� ��ġ�� �������� ��ȯ
    {
        m_CurrentTime = 0;
        m_bellyTimer = LoadTime;

        while (m_CurrentTime <= m_bellyTimer && isAttacked && m_CountHit >= 1 && !isRushing)
        {
            m_CurrentTime = m_CurrentTime + Time.deltaTime;
            yield return null;
        }
        if (m_CountHit >= 4)
        {
            Debug.Log("��ġ��");
            ChangeState(BellyAttack_State.Instance);
            m_CountHit = 0;
        }

    }


    #endregion

    #region ==Ray==
    private RaycastHit m_hit;
    private RaycastHit m_PlayerHit;
    [Header("=====================")]
    [Header("�� Ž�� Ray ����")][SerializeField] private float RayDistance = 10;
    [Header("�÷��̾� Ž�� Ray ����")][SerializeField] private float RayPlayerDistance = 5;
    [SerializeField] private LayerMask m_LayMask;
    [SerializeField] private LayerMask m_PlayerLayMask;

    void RayCheckWall()
    {
        if (Physics.Raycast(transform.position, transform.forward, out m_hit, RayDistance, m_LayMask))
        {
            Debug.DrawRay(transform.position, transform.forward * RayDistance, Color.green);
            Col_with_Wall = true;

        }
        else
        {
            Debug.DrawLine(transform.position, transform.forward * RayDistance, Color.red);
            Col_with_Wall = false;
        }
    }

    void RayCheckPlayer()       //�÷��̾� ���� �ȿ� ������ (SetBellyTime �ʵ���) ��ġ��
    {
        if (Physics.Raycast(transform.position, transform.forward, out m_hit, RayPlayerDistance, m_PlayerLayMask))
        {
            Debug.DrawRay(transform.position, transform.forward * RayPlayerDistance, Color.blue);
            m_CanBelly = true;


            m_BellyColTime += Time.deltaTime;
            Debug.Log("ī��Ʈ Ÿ�� : " + m_BellyColTime);

            if (m_CanBelly && !isRushing && !isRolling && m_BellyColTime >= SetBellyTime) 
            {
                ChangeState(BellyAttack_State.Instance);
                m_BellyColTime = 0;
            }

        }
        else
        {
            Debug.DrawLine(transform.position, transform.forward * RayPlayerDistance, Color.yellow);
            m_CanBelly = false;
            m_BellyColTime = 0;
        }
    }

    #endregion

    #region ForEffect
    public void StartSmashEffect()
    {
        Parti_Strike.Play();
        return;
    }

    #endregion

    //================================================================//
    public void OnDamage(int PlayerDamage, Vector3 pos)
    {
        if (!Isinvincibility)
        {
            CurrentHP -= PlayerDamage;
            isAttacked = true;

            Parti_Damage.Play();

            if (CurrentHP <= 0)
            {
                ChangeState(HanselDie_State.Instance);
            }

            m_CountHit += 1;                              //���ݹ��� Ƚ�� ī��Ʈ
            StartCoroutine(CountBelly(BellyDelay));       //��ġ�� ī��Ʈ

            if (PhaseChecker == 1 && CurrentHP <= 10)
            {
                inPhaseTwo();
            }

        }
        else
        {
            Debug.Log("��������. �÷��̾� ���� ����");
        }
    }

    public void OnRecovery(int heal)
    {

    }
 
    //================================================================//

    #region No use function

    //private void CheckVelocity()
    //{
    //    m_CurrentPosition = transform.position;
    //    var dis = (m_CurrentPosition - m_oldPosition);
    //    var distance = Math.Sqrt(Math.Pow(dis.x, 2) + Math.Pow(dis.y, 2) + Math.Pow(dis.z, 2));
    //    m_Velocity = distance / Time.deltaTime;
    //    m_oldPosition = m_CurrentPosition;

    //}

    #endregion
}