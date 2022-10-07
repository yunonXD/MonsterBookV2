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
    [Header("현재 Hanse HP")] public float CurrentHP = 100;
    
    [SerializeField] private int m_PlayerColDamage = 10;    //플레이어 닿았을 시 데미지
    [SerializeField] private GameObject m_Gretel;           //2페이즈 활성화 시킬 그레텔

    [Header("=====================Smash")]

    #region  SmashAttack var

    [Header("Smash데미지")]
    public int Hansel_SmashDamage = 10;

    [Header("Smash 공격 최소거리")]
    public float SmashAttackRange = 2f;

    //Smash 에 사용할 주먹에 넣을 Col
    public GameObject SmashCollider_R;
    public GameObject SmashCollider_L;

    public bool isSmash = false;

    [HideInInspector]public bool isSmashRandomBool = true;        //공격 패턴 결정 Rand 한번만 돌도록 처리
    [Header("패턴 번호")]
    public float ForSmashP = 0;                             //공격 패턴 결정 Rand 계산 값
    public float Attack_Time = 0;

    public float AttackPattern_1 = 4;
    public float AttackPattern_2 = 4;
    public float AttackPattern_3 = 4;
    public float AttackPattern_4 = 4;


    #endregion

    [Header("=====================Rush")]

    #region RushAttack Var

    [Header("Rush 데미지")]
    public int Hansel_RushDamage = 10;

    [Header("Rush 속도")]
    public int Rush_Speed = 5;

    [Header("Rush 끝나고 대기시간")]
    public int Rush_EndWait = 4;

    public GameObject RushCollider;                         //Rush에 사용할 Col   
    public bool isRushing = false;                          //Rush 중인지 체크
    [HideInInspector] public bool Col_with_Wall = false;

    #endregion

    [Header("=====================Belly")]

    #region BellyAttack var

    [Header("Belly 데미지")]
    public int Hansel_BellyDamage = 10;

    [Header("Belly 지속시간")]
    public float BellyAttackSpeed = 2;

    [Header("Belly 강도 - Force")]
    public float BellyPower = 4;

    [Header("Belly 맞은 플레이어 뒤로 밀릴 힘")]
    public float BellyForce = 5;
    [Header("Belly 맞은 플레이어 위로 밀릴 힘")]
    public float BellyForceUp = 5;


    [Header("보스 - 플레이어 충돌 시간")]
    [SerializeField]private float m_BellyColTime = 0;

    [Header("충돌 n 초 이상이면 발동시킬 시간")]
    public int SetBellyTime = 4;


    [Header("공격 카운트할 n 초 - 마우스 올려봐요")]
    [Tooltip("플레이어에게 공격당했을 시 BellyDelay 초 안에 m_CountHit 만큼 공격받으면 Belly 공격 실행")]
    [SerializeField] private int BellyDelay = 2;

    public GameObject BellyCollider;                         //Belly에 사용할 Col 
    public bool isBelly = false;
    private bool m_CanBelly = false;
    private int m_CountHit = 0;                              //공격 받은 횟수 카운트
    private float m_CurrentTime = 0;
    private int m_bellyTimer = 0;
    #endregion

    [Header("=====================Rolling")]

    #region RollingAttack var

    [Header("Rolling 데미지")]
    public int Hansel_RollingDamage = 10;

    [Header("굴러다니기 WayPoiner")]
    public Transform[] Rolling_Position;

    [Header("굴러다니기 속도")]
    public float RollingSpeed = 3;

    [Header("음식 먹을때 대기시간")]
    public float RollingWaitTime = 4;

    [Header("벽에서 턴하는 속도")]
    public float RollingRotation = 5;

    [Header("구르고있는지 체크")]
    public bool isRolling = false;


    public GameObject RollingCollider;                         //Rolling에 사용할 Col 

    #endregion

    [Header("=====================ThrowUp")]

    #region ThrowUp

    [Header("Throw up 데미지")]
    public int Hansel_ThrowUpDamage = 10;

    [Header("Throw up 범위")]
    public int ThrowUpRange = 5;

    [Header("Throw up 지속시간")]
    public int ThrowUpTime = 3;

    [Header("Throw up 발동 확률")]
    [SerializeField] private int m_ThrowUpActivePercent = 30;

    public GameObject ThrowUpCollider;

    #endregion

    [Header("=====================ThrowPlayer")]

    #region ThrowPlayer

    [Header("ThrowPlayer 데미지")]
    public int Hansel_ThrowPlayer = 10;

    [Header(" 플레이어가 보스의 Ray 범위 안에 들어왔는가?")]
    public bool RayPlayer = false;

    [Header("플레이어 Parent 바뀔 위치")]
    public Transform HanselHand_L;
    public Transform HanselHand_R;

    [Header("던지기 발동 확률")]
    public int TP_Percent = 30;

    [Header("던지기 힘")]
    public float TP_Force = 0.0f;

    public bool isTP = false;           //던지기 실행중인지 확인
    public GameObject TPCollider;       //데미지 콜 오브젝트 활성화

    [HideInInspector] public bool TP_Throwing = false;

    #endregion

    [Header("=====================movement")]

    #region Movement var

    [Header("추적 지속 시간")]
    public float ChaseCancleTime = 5.0f;
    [Header("현재 추적시간")]
    public float ChaseTime = 0;
    [Header("속도")]
    public float Hansel_Speed = 5;
    [Header("회전속도")]
    public float Hansel_RotSpeed = 10;


    #endregion


    [Header("=====================Stun")]

    #region Stun var

    //[Header("스턴 지속시간")] public int StunRemainingTime = 3;
    public bool _isStuned = false;

    [SerializeField] private UnityEvent pazeTwoEvent;

    #endregion

    [Header("=====================Effect")]

    #region Effect
    [Header("이펙트")]
    [SerializeField] private StringParticle m_Particle;
    //플레이어에서 받아옴!!
    #endregion

    #region no Touch

    private StateMachine<Hansel> state = null;
    [HideInInspector] public Animator Ani = null;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public bool IsDead = false;                                       //죽음 체크
    [HideInInspector] public bool Isinvincibility = false;                              //무적기 체크
    [HideInInspector] public bool isAttacked = true;                                    //공격 받았는지 체크
     public int PhaseChecker = 1;                                      //1페이즈 2페이즈 체크
    [HideInInspector] public CapsuleCollider CapCol_Hansel;
    [HideInInspector] public Quaternion m_MyTartgetRot;
    protected Vector3 PlayerYeet;
    #endregion

    [Header("=====================Rush % instance")]

    #region Random Rush instance

    [Header("플레이어 멍때리는시간")] public float TimerCount = 5;                        //얼마만큼 가만히 있으면 Rush 공격(랜덤) 할지 ..
    [SerializeField][Header("멍때리고나서 Rush 발동 확률")] private int m_ForRushP = 30;  //TimerCount 만큼 가만히 있었을 시 Rush 가 발동할 확률
    [SerializeField][Header("일반공격 말고 러쉬가 발동할 확률")] private int m_ForRushInsSmash = 30;

    #endregion

    #region ==Debug==

    [Header("=====================")]
    [Header("==디버그== 스페이스 : 구르기 // 마우스 우클릭 : 러쉬 // 마우스 휠 : 배치기")]
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

        //======================디버그=====================
        DebugMode();
        //Debug.Log(transform.localRotation.eulerAngles);
        //======================디버그=====================


        if (isSmash == true)    //Smash 연속 공격 에 사용할 목적
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
                myTarget = collision.collider.gameObject;      //혹시 몰라서 한번 더 넣어줌


                if (CheckRange())   //플레이어 ~ 보스 거리측정
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

    public bool CheckRange()        // Smash 거리측정기
    {

        if (Vector3.Distance(myTarget.transform.position, transform.position) <= SmashAttackRange)
        {

            return true;
        }

        return false;

    }

    public void ResetState()        //Hansel 리셋
    {
        CurrentHP = HanselHP;
        state = new StateMachine<Hansel>();

        state.Initial_Setting(this, HanselMove_State.Instance);

        // Reset Target 은.. 필요 없을듯
        // myTarget = null;
    }     

    public void inPhaseTwo()        //페이즈2 진입
    {
        ResetState();
        Ani.SetTrigger("H_Phase2");
        m_CountHit = 0;
        PhaseChecker++;

        m_Gretel.SetActive(true);
        pazeTwoEvent.Invoke();

    }       

    public bool RandCalculate(int RandNum)     //만능 % cul. 여러곳에서 사용중
    {
        var m_rand = UnityEngine.Random.Range(1, 101);
#if DEBUG
        //Debug.LogError("랜덤 값 : " + m_rand);
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
        //Debug.Log("스메쉬 랜덤 값 : " + m_Srand);
#endif

       ForSmashP = m_Srand;
       //ForSmashP = 4;
    }

    public void OnDircalculator(int Yeet)
    {
        //True (1) : 정상적인 방향으로 LookAt
        //false (0 or something without 1) : 반대 방향으로 LookAt

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

    public IEnumerator CountTimer(float m_Timer)                //일정 시간 공격 받는지 체크
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

    public IEnumerator CountBelly(int LoadTime)                 //2초 안에 4회 연속 공격 받을 시 배치기 공격으로 전환
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
    [Header("벽 탐지 Ray 길이")][SerializeField] private float RayDistance = 10;
    [Header("플레이어 탐지 Ray 길이")][SerializeField] private float RayPlayerDistance = 5;
    [SerializeField] private LayerMask m_LayMask;
    [SerializeField] private LayerMask m_PlayerLayMask;

    void RayCheckWall()     //벽탐지
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

    void RayCheckPlayer()       //플레이어 보스 안에 있으면 (SetBellyTime 초동안) 배치기
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
    //----------------Smash 공격용 Animation 컨트롤러---------------//
    protected void SmashAttackColR_On()     //오른손 공격 콜라이더
    {
        SmashCollider_R.SetActive(true);
    }

    protected void SmashAttackColL_On()     //왼손 공격 콜라이더
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

    protected void SmashForce (float val)       //공격 후 앞으로 살짝 AddForce
    {
        //Vector3 m_Dir = new Vector3(transform.position.x, 0,0);
        //rb.AddForce(m_Dir * val, ForceMode.VelocityChange);
        rb.AddForce(transform.forward * val, ForceMode.VelocityChange);
    }

    //-----------------------------------------------------------------//
    #endregion

    #region ThrowPlayer
    //--------------플레이어 던지기 공격용 Animation 컨트롤러-------------//
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

            m_CountHit += 1;                              //공격받은 횟수 카운트
            StartCoroutine(CountBelly(BellyDelay));       //배치기 카운트

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
            UnityEngine.Debug.Log("!!무적상태. 플레이어 공격 무시!!");
        }
    }

    public void OnRecovery(int heal)
    {

    }
}