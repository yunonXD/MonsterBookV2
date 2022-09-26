using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



public class Hansel : MonoBehaviour, IEntity
{

    [HideInInspector] public float HanselHP = 100;
    [Header("현재 Hanse HP")] public float CurrentHP = 100;
    public GameObject myTarget;
    public GameObject Gretel;

    [Header("=====================Smash")]

    #region  SmashAttack var

    [Header("Smash 데미지")]
    public int Hansel_SmashDamage = 10;

    [Header("Smash 공격 최소거리")]
    public float SmashAttackRange = 2f;

    [Header("Smash 공격 딜레이")]
    public float SmashAttackSpeed = 2.0f;

    //Smash 에 사용할 주먹에 넣을 Col
    public GameObject SmashCollider;

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


    [Header("=====================movement")]

    #region Movement var

    [Header("추적 지속 시간")]
    public float ChaseCancleTime = 5.0f;
    [Header("현재 추적시간")]
    public float ChaseTime = 0;

    #endregion

    [Header("=====================Stun")]

    #region Stun var

    [Header("스턴 지속시간")] public int StunRemainingTime = 3;
    [HideInInspector] public bool _isStuned = false;

    #endregion

    [Header("=====================Effect")]

    #region Effect
    [Header("이펙트")]
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
    [HideInInspector] public bool IsDead = false;                                       //죽음 체크
    [HideInInspector] public bool Isinvincibility = false;                              //무적기 체크
    [HideInInspector] public bool isAttacked = true;                                    //공격 받았는지 체크
    [HideInInspector] public int PhaseChecker = 1;                                      //1페이즈 2페이즈 체크
   
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
        //m_oldPosition = transform.position; Velocity 측정용

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
        //======================디버그=====================
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
        //======================디버그=====================
    
    }


    public void ChangeState(FSM_State<Hansel> _State)
    {
        state.ChangeState(_State);
    }       // 상태변경


    #region OnTrigger
    private void OnTriggerEnter(Collider col)
    {
        // 러쉬 , 구르기 , 스턴상태 , 배치기가 아니라면
        // 30 % 확률 + 벽에 닿아있지 않다면 Rush 공격 , 해당 조건이 아니라면 다음으로 넘어가서 30% 확률로 구토.
        // 것도 아니면 스메쉬 공격.
        // 플레이어 닿았을 시 or 버프아이템 닿았을 시 ,,는 따로 구분해둠
        if (!isRushing && !isRolling && !_isStuned && !isBelly)  //rush / rolling / Stun 이면 작동안함
        {
            if (col.gameObject.tag == "Player")
            {
                myTarget = col.gameObject;

                if (CheckRange())   //플레이어 ~ 보스 거리측정
                {
                    if (RandCalculate(m_ForRushInsSmash) == true && Col_with_Wall == false)
                    {
                        ChangeState(RushAttack_State.Instance);
                    }
                    else if (RandCalculate(m_ThrowUpActivePercent) == true && PhaseChecker == 2)
                    {
                        Debug.Log("우웩");
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
                Debug.Log("음식 섭취 단계 진입");
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

    public bool CheckAngle()        //Smash 각도측정기
    {
        if (Vector3.Dot(myTarget.transform.position, transform.position) >= 0.5f)
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

        // Reset Target
        // myTarget = null;
    }     

    public void inPhaseTwo()        //페이즈2 진입
    {
        ResetState();
        Ani.SetTrigger("H_Phase2");
        PhaseChecker++;

        Gretel.SetActive(true);

#if DEBUG
        Debug.Log("Phase 2 XD");
#endif
    }       

    public bool RandCalculate(int RandNum)     //러쉬 % 계산(Idle 및 Smash 이전)
    {
        int m_rand = UnityEngine.Random.Range(1, 101);
#if DEBUG
        //Debug.Log("랜덤 값 : " + m_rand);
#endif


        if (m_rand <= RandNum)
        {
            return true;
        }
        else
            return false;
    }


    #region CoroutineTimer/Belly
   
    public IEnumerator CountTimer(float m_Timer)                //일정 시간 공격 받는지 체크
    {
        yield return new WaitForSeconds(m_Timer);

        //공격 받지 않았고 30% 이상이라면 (무한반복)
        if (isAttacked == false && RandCalculate(m_ForRushP) == true && !isRolling && !isBelly)
        {
            ChangeState(RushAttack_State.Instance);
        }
        isAttacked = false;
        m_CountHit = 0;

        StartCoroutine(CountTimer(TimerCount));
    }

    public IEnumerator CountBelly(int LoadTime)                 //2초 안에 4회 연속 공격 받을 시 배치기 공격으로 전환
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
            Debug.Log("배치기");
            ChangeState(BellyAttack_State.Instance);
            m_CountHit = 0;
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

    void RayCheckPlayer()       //플레이어 보스 안에 있으면 (SetBellyTime 초동안) 배치기
    {
        if (Physics.Raycast(transform.position, transform.forward, out m_hit, RayPlayerDistance, m_PlayerLayMask))
        {
            Debug.DrawRay(transform.position, transform.forward * RayPlayerDistance, Color.blue);
            m_CanBelly = true;


            m_BellyColTime += Time.deltaTime;
            Debug.Log("카운트 타임 : " + m_BellyColTime);

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

            m_CountHit += 1;                              //공격받은 횟수 카운트
            StartCoroutine(CountBelly(BellyDelay));       //배치기 카운트

            if (PhaseChecker == 1 && CurrentHP <= 10)
            {
                inPhaseTwo();
            }

        }
        else
        {
            Debug.Log("무적상태. 플레이어 공격 무시");
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