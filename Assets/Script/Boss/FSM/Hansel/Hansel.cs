using System;
using System.Collections;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
#pragma warning disable 

public class Hansel : MonoBehaviour, IEntity
{
    [HideInInspector] public GameObject myTarget;
    [HideInInspector] public float HanselHP = 100;
    [Header("���� Hanse HP")] public float CurrentHP = 100;
    
    [SerializeField] private int m_PlayerColDamage = 10;    //�÷��̾� ����� �� ������
    [SerializeField] private GameObject m_Gretel;           //2������ Ȱ��ȭ ��ų �׷���

    [Header("=====================Smash")]

    #region  SmashAttack var

    [Header("Smash������")]
    public int Hansel_SmashDamage = 10;

    [Header("Smash ���� �ּҰŸ�")]
    public float SmashAttackRange = 2f;

    //Smash �� ����� �ָԿ� ���� Col
    public GameObject SmashCollider_R;
    public GameObject SmashCollider_L;

    public bool isSmash = false;

    [HideInInspector]public bool isSmashRandomBool = true;        //���� ���� ���� Rand �ѹ��� ������ ó��
    [Header("���� ��ȣ")]
    public float ForSmashP = 0;                             //���� ���� ���� Rand ��� ��
    public float Attack_Time = 0;

    public float AttackPattern_1 = 4;
    public float AttackPattern_2 = 4;
    public float AttackPattern_3 = 4;
    public float AttackPattern_4 = 4;


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

    [Header("Belly ���� �÷��̾� �ڷ� �и� ��")]
    public float BellyForce = 5;
    [Header("Belly ���� �÷��̾� ���� �и� ��")]
    public float BellyForceUp = 5;


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

    [Header("������ ���ϴ� �ӵ�")]
    public float RollingRotation = 5;

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

    [Header("=====================ThrowPlayer")]

    #region ThrowPlayer

    [Header("ThrowPlayer ������")]
    public int Hansel_ThrowPlayer = 10;

    [Header(" �÷��̾ ������ Ray ���� �ȿ� ���Դ°�?")]
    public bool RayPlayer = false;

    [Header("�÷��̾� Parent �ٲ� ��ġ")]
    public Transform HanselHand_L;
    public Transform HanselHand_R;

    [Header("������ �ߵ� Ȯ��")]
    public int TP_Percent = 30;

    [Header("������ ��")]
    public float TP_Force = 0.0f;

    public bool isTP = false;           //������ ���������� Ȯ��
    public GameObject TPCollider;       //������ �� ������Ʈ Ȱ��ȭ

    [HideInInspector] public bool TP_Throwing = false;

    #endregion

    [Header("=====================movement")]

    #region Movement var

    [Header("���� ���� �ð�")]
    public float ChaseCancleTime = 5.0f;
    [Header("���� �����ð�")]
    public float ChaseTime = 0;
    [Header("�ӵ�")]
    public float Hansel_Speed = 5;
    [Header("ȸ���ӵ�")]
    public float Hansel_RotSpeed = 10;


    #endregion


    [Header("=====================Stun")]

    #region Stun var

    //[Header("���� ���ӽð�")] public int StunRemainingTime = 3;
    public bool _isStuned = false;

    [SerializeField] private UnityEvent pazeTwoEvent;

    #endregion

    [Header("=====================Effect")]

    #region Effect
    [Header("����Ʈ")]
    [SerializeField] private StringParticle m_Particle;
    //�÷��̾�� �޾ƿ�!!
    #endregion

    #region no Touch

    private StateMachine<Hansel> state = null;
    [HideInInspector] public Animator Ani = null;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public bool IsDead = false;                                       //���� üũ
    [HideInInspector] public bool Isinvincibility = false;                              //������ üũ
    [HideInInspector] public bool isAttacked = true;                                    //���� �޾Ҵ��� üũ
     public int PhaseChecker = 1;                                      //1������ 2������ üũ
    [HideInInspector] public CapsuleCollider CapCol_Hansel;
    [HideInInspector] public Quaternion m_MyTartgetRot;
    protected Vector3 PlayerYeet;
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

    //==============================================================//

    void Awake()
    {
        ResetState();
        myTarget = GameObject.FindGameObjectWithTag("Player");
        Ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        CapCol_Hansel = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        StartCoroutine(CountTimer(TimerCount));

    }

    void FixedUpdate()
    {
        state.Update();
        RayCheckWall();
        RayCheckPlayer();

        //======================�����=====================
        DebugMode();
        //Debug.Log(transform.localRotation.eulerAngles);
        //======================�����=====================


        if (isSmash == true)    //Smash ���� ���� �� ����� ����
        {
            Attack_Time += Time.deltaTime;
        }
    }

    protected void DebugMode()
    {
        if (m_isDebug)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                ChangeState(RollingAttack_State.Instance);
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                ChangeState(RushAttack_State.Instance);
            }

            if (Input.GetKeyDown(KeyCode.F3))
            {
                ChangeState(BellyAttack_State.Instance);
            }

            if (Input.GetKeyDown(KeyCode.F4))
            {
                ChangeState(Stun_State.Instance);
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {
                ChangeState(ThrowPlayer_State.Instance);
            }
        }
        else
            return;
    }

    public void ChangeState(FSM_State<Hansel> _State)
    {
        state.ChangeState(_State);
    }


    #region OnCollisionEnter Yeet

    private void OnCollisionEnter(Collision collision)
    {
        if (!isRushing && !isRolling && !_isStuned && !isBelly && !isTP)
        {
            if (collision.collider.CompareTag("Player"))
            {
                myTarget = collision.collider.gameObject;      //Ȥ�� ���� �ѹ� �� �־���


                if (CheckRange())   //�÷��̾� ~ ���� �Ÿ�����
                {                
                    if (RandCalculate(m_ForRushInsSmash) && !Col_with_Wall &&
                        (transform.rotation.eulerAngles.y == 90 || transform.rotation.eulerAngles.y == -90 || transform.rotation.eulerAngles.y == -270 || transform.rotation.eulerAngles.y == 270))
                    {
                        ChangeState(RushAttack_State.Instance);
                    }
                    else if (RandCalculate(TP_Percent) && (PhaseChecker == 2) && RayPlayer && !Col_with_Wall)
                    {
                        //ChangeState(ThrowPlayer_State.Instance);
                    }
                    else if (!isSmash)
                    {
                        ChangeState(SmashAttack_State.Instance);
                    }
                    else
                    {
                        return;
                    }
                }
                else if (!CheckRange())
                {
                    ChangeState(HanselMove_State.Instance);
                    return;
                }
            }           
            else if (collision.collider.CompareTag("BossBuff"))
            {
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

    public void ResetState()        //Hansel ����
    {
        CurrentHP = HanselHP;
        state = new StateMachine<Hansel>();

        state.Initial_Setting(this, HanselMove_State.Instance);

        // Reset Target ��.. �ʿ� ������
        // myTarget = null;
    }     

    public void inPhaseTwo()        //������2 ����
    {
        ResetState();
        Ani.SetTrigger("H_Phase2");
        m_CountHit = 0;
        PhaseChecker++;

        m_Gretel.SetActive(true);
        pazeTwoEvent.Invoke();

    }       

    public bool RandCalculate(int RandNum)     //���� % cul. ���������� �����
    {
        var m_rand = UnityEngine.Random.Range(1, 101);
#if DEBUG
        //Debug.LogError("���� �� : " + m_rand);
#endif


        if (m_rand <= RandNum)
        {
            return true;
        }
        else
            return false;
    }

    public void RandCalculateForSmash(int RandNum)
    {
        var m_Srand = UnityEngine.Random.Range(1, RandNum);
#if DEBUG
        //Debug.Log("���޽� ���� �� : " + m_Srand);
#endif

       ForSmashP = m_Srand;
       //ForSmashP = 4;
    }

    public void OnDircalculator(int Yeet)
    {
        //True (1) : �������� �������� LookAt
        //false (0 or something without 1) : �ݴ� �������� LookAt

        var Dir = transform.localRotation.eulerAngles;

        if (Dir.y >= 200)
        {
            if (Yeet == 1)
            {
                //Left
                transform.eulerAngles = new Vector3(transform.localRotation.x, 270, transform.localRotation.z);
                transform.localScale = new Vector3(2.3f * -1f, 2.3f, 2.3f);
            }
            else
            {
                //Right
                transform.eulerAngles = new Vector3(transform.localRotation.x, 90, transform.localRotation.z);
                transform.localScale = new Vector3(2.3f, 2.3f, 2.3f);

            }  

        }
        else
        {
            if (Yeet == 1)
            {
                //Right
                transform.eulerAngles = new Vector3(transform.localRotation.x, 90, transform.localRotation.z);
                transform.localScale = new Vector3(2.3f, 2.3f, 2.3f);
            }
            else
            {
                //Left
                transform.eulerAngles = new Vector3(transform.localRotation.x, 270, transform.localRotation.z);
                transform.localScale = new Vector3(2.3f * -1f, 2.3f, 2.3f);
            }
            
        }
    }


    #region CoroutineTimer/Belly/Protect

    public IEnumerator CountTimer(float m_Timer)                //���� �ð� ���� �޴��� üũ
    {
        yield return new WaitForSeconds(m_Timer);

        if (!isAttacked && RandCalculate(m_ForRushP) && !isRolling && !isBelly && !isRushing &&!_isStuned &&
            (transform.rotation.eulerAngles.y == 90 || transform.rotation.eulerAngles.y == -90 || transform.rotation.eulerAngles.y == -270 || transform.rotation.eulerAngles.y == 270))
        {
            ChangeState(RushAttack_State.Instance);
        }
        isAttacked = false;
        m_CountHit = 0;

        StartCoroutine(CountTimer(TimerCount));
    }

    public IEnumerator CountBelly(int LoadTime)                 //2�� �ȿ� 4ȸ ���� ���� ���� �� ��ġ�� �������� ��ȯ
    {
        m_bellyTimer = LoadTime;
        while (m_CurrentTime <= m_bellyTimer && isAttacked && m_CountHit >= 1 && !isRushing)
        {
            m_CurrentTime = m_CurrentTime + Time.deltaTime;

            if (m_CountHit >= 4 && !isRushing && !_isStuned)
            {
                m_CurrentTime = 0;
                ChangeState(BellyAttack_State.Instance);
                yield return null;
            }
            else
                yield return null;

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

    void RayCheckWall()     //��Ž��
    {
        UnityEngine.Debug.DrawRay(transform.position, transform.forward * RayDistance, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out m_hit, RayDistance, m_LayMask))
        {
            Col_with_Wall = true;
        }
        else
        {
            Col_with_Wall = false;
        }
    }

    void RayCheckPlayer()       //�÷��̾� ���� �ȿ� ������ (SetBellyTime �ʵ���) ��ġ��
    {
        UnityEngine.Debug.DrawRay(transform.position + Vector3.up, transform.forward * RayPlayerDistance, Color.blue);
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out m_PlayerHit, RayPlayerDistance, m_PlayerLayMask))
        {           
            m_CanBelly = true;
            RayPlayer = true;

            m_BellyColTime += Time.deltaTime;
            if (m_CanBelly && !isRushing && !isRolling && !isTP && !_isStuned && (m_BellyColTime >= SetBellyTime))
            {
                ChangeState(BellyAttack_State.Instance);
                m_BellyColTime = 0;
            }

        }
        else
        {
            m_CanBelly = false;
            RayPlayer = false; ;
            m_BellyColTime = 0;
        }
    }

    #endregion

    #region ForAnime

    #region Smash
    //----------------Smash ���ݿ� Animation ��Ʈ�ѷ�---------------//
    protected void SmashAttackColR_On()     //������ ���� �ݶ��̴�
    {
        SmashCollider_R.SetActive(true);
    }

    protected void SmashAttackColL_On()     //�޼� ���� �ݶ��̴�
    {
        SmashCollider_L.SetActive(true);
    }

    protected void SmashAttackColR_Off()
    {
        SmashCollider_R.SetActive(false);
    }
    protected void SmashAttackColL_Off()
    {
        SmashCollider_L.SetActive(false);
    }

    protected void SmashForce (float val)       //���� �� ������ ��¦ AddForce
    {
        //Vector3 m_Dir = new Vector3(transform.position.x, 0,0);
        //rb.AddForce(m_Dir * val, ForceMode.VelocityChange);
        rb.AddForce(transform.forward * val, ForceMode.VelocityChange);
    }

    //-----------------------------------------------------------------//
    #endregion

    #region ThrowPlayer
    //--------------�÷��̾� ������ ���ݿ� Animation ��Ʈ�ѷ�-------------//
    protected void SetParent_R()
    {
        myTarget.transform.SetParent(HanselHand_R );
    }

    protected void SetParent_L()
    {     
        myTarget.transform.SetParent(HanselHand_L);       
    }

    protected void SetParentnullWithForce( )
    {    
        myTarget.transform.SetParent(null);
        myTarget.GetComponent<CapsuleCollider>().isTrigger = false;

    }

    protected void ThrowPlayerYeet()
    {
        TP_Throwing = true;
        myTarget.GetComponent<PlayerController>().SetThrowState(false);
        myTarget.GetComponent<Rigidbody>().AddForce(transform.forward * TP_Force, ForceMode.Impulse);
    }

    //-----------------------------------------------------------------//
    #endregion

    protected void Particle_anime(string name)
    {
        m_Particle[name].Play();
    }


    #endregion

    //================================================================//
    public void OnDamage(int PlayerDamage, Vector3 pos)
    {
        if (!Isinvincibility)
        {
            CurrentHP -= PlayerDamage;
            isAttacked = true;

            m_Particle["OnDamage"].Play();

            m_CountHit += 1;                              //���ݹ��� Ƚ�� ī��Ʈ
            StartCoroutine(CountBelly(BellyDelay));       //��ġ�� ī��Ʈ

            if (PhaseChecker == 1 && CurrentHP <= 10)
            {
                inPhaseTwo();
                m_CountHit = 0;
            }
            if (PhaseChecker == 2 && CurrentHP <= 0)
            {
                ChangeState(Stun_State.Instance);
            }

        }
        else
        {
            UnityEngine.Debug.Log("!!��������. �÷��̾� ���� ����!!");
        }
    }

    public void OnRecovery(int heal)
    {

    }
}