using System.Collections;
using UnityEngine;
using UnityEngine.Events;
#pragma warning disable 

public class Hansel : MonoBehaviour, IEntity
{
    [HideInInspector] public GameObject myTarget;
    public float HanselHP;
    [Header("���� Hanse HP")] public float CurrentHP = 100;
    
    [SerializeField] private int m_PlayerColDamage = 10;    
    [SerializeField] private GameObject m_Gretel;           
    public float HanselIdleTime;
    public float HanselIdleTime_Cur;
    public bool LookPlayer;
    public bool LookGretel;
    public GameObject EventSystem;
    public GameObject RuntoGretelPosition_X;
    public GameObject RuntoGretelPosition_Y;
    public GameObject ComeBackPosition;
    public GameObject WallL;
    public GameObject WallR;
    [HideInInspector] public GameObject RushPunch_L;          //Effect
    [HideInInspector] public GameObject RushPunch_R;
    [HideInInspector] public GameObject RushPunch_end_L;
    [HideInInspector] public GameObject RushPunch_end_R;
    public GameObject[] RollingPoint;
    [HideInInspector] public RigidbodyConstraints Constraints;
    [SerializeField] private UnityEvent GretelHitEvent;
    [SerializeField] private UnityEvent HanselHitEvent;
    
    private ContactPoint HitPoint;
    public GameObject Lamp;

    public bool StunMove = false;
    public GameObject WorldSound;


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
    public float AttackPattern_2 = 7;
    public float AttackPattern_3 = 10;
    public float AttackPattern_4 = 6;
    public GameObject Attack3_Damagecol;
    public bool HanselFSMStart;


    #endregion

    [Header("=====================Rush")]

    #region RushAttack Var

    [Header("Rush ������")]
    public int Hansel_RushDamage = 10;

    [Header("Rush �ӵ�")]
    public int Rush_Speed = 5;

    [Header("Rush ������ ���ð�")]
    public float Rush_EndWait = 4;

    public GameObject RushCollider;                         //Rush�� ����� Col   
    public bool isRushing = false;                          //Rush ������ üũ
    public bool Col_with_Wall = false;                      //�� ���� (�Ÿ���ŭ ���Դ���)
    public bool Colli_EndRush = false;                      //�� ���� (���� ���� �� üũ)

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
    public bool isThrow = false;
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

    [Header("���� ������ ��")]
    public float TP_ForceUp = 0.0f;

    public bool isTP = false;           //������ ���������� Ȯ��

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
    public bool Isinvincibility = false;                              //������ üũ
    [HideInInspector] public bool isAttacked = true;                                    //���� �޾Ҵ��� üũ   
    [HideInInspector] public int PhaseChecker = 0;                                      //1������ 2������ üũ
    [HideInInspector] public CapsuleCollider CapCol_Hansel;                             //Hansel �ݶ��̴�
    [HideInInspector] public Quaternion m_MyTartgetRot;                                 //�÷��̾� ȸ�� ����(�������)
    #endregion


    #region ==Debug==

    [Header("=====================")]
    [Header("==�����== �����̽� : ������ // ���콺 ��Ŭ�� : ���� // ���콺 �� : ��ġ��")]
    [SerializeField] private bool m_isDebug = false;

    #endregion

    void Awake()
    {
        PhaseChecker = 1;
        ResetState();
        myTarget = GameObject.FindGameObjectWithTag("Player");
        Ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        CapCol_Hansel = GetComponent<CapsuleCollider>();
        Ani.SetTrigger("StageStart");
        Constraints = rb.constraints;
        Hansel_Start();
        
    }

    void Start()
    {
        ResetState();
    }

    void FixedUpdate()
    {
        if(IsDead == true)
        {
            ChangeState(HanselStunMoveState_Y.Instance);
        }
        if (isSmash == true)    //Smash ���� ���ݿ� timer
        {
            Attack_Time += Time.deltaTime;
        }
        state.Update();
        RayCheckWall();
        LookChaek_Player();
        LookChaek_Gretel();

        if(Input.GetKeyDown(KeyCode.T))
        {
            Hansel_Start();
        }

    }

    public void IsRushingReset()
    {
        isRushing = false;
    }
    public void BellyColOff()
    {
        BellyCollider.SetActive(false);
    }
    public void BellyColOn()
    {
        BellyCollider.SetActive(true);
    }

    public void RushColOn()
    {
        RushCollider.SetActive(true);
    }

    public void Attack3_DamagecolOn()
    {
        Attack3_Damagecol.SetActive(true);
    }
    public void Attack3_DamagecolOff()
    {
        Attack3_Damagecol.SetActive(false);
    }
    public void Hansel_Start()
    {
        Ani.SetTrigger("Hansel_Start");

        WorldSound.GetComponent<WorldSound>().WorldSoundPlay("1StageBoss_LightElectricity",0);   //���� �������Ҹ� ����
        rb.constraints = Constraints;
        HanselFSMStart = true;
    }

    void ResetWalk()
    {
        Ani.SetFloat("H_Walk", 0);
    }

    void LookChaek_Player()
    {
        if (myTarget.transform.position.x <= gameObject.transform.position.x && (int)gameObject.transform.rotation.eulerAngles.y >= 265 &&
            myTarget.transform.position.x <= gameObject.transform.position.x && (int)gameObject.transform.rotation.eulerAngles.y <= 275)
        {
            LookPlayer = true;
        }
        else if (myTarget.transform.position.x >= gameObject.transform.position.x && (int)gameObject.transform.rotation.eulerAngles.y >= 85 &&
            myTarget.transform.position.x >= gameObject.transform.position.x && (int)gameObject.transform.rotation.eulerAngles.y <= 95)
        {
            LookPlayer = true;
        }
        else
        {
            LookPlayer = false;
        }
    }
    void LookChaek_Gretel()
    {
        if (RuntoGretelPosition_X.transform.position.x <= gameObject.transform.position.x && (int)gameObject.transform.rotation.eulerAngles.y == 270)
        {
            LookGretel = true;
        }
        else if (RuntoGretelPosition_X.transform.position.x >= gameObject.transform.position.x && (int)gameObject.transform.rotation.eulerAngles.y == 90)
        {
            LookGretel = true;
        }
        else
        {
            LookGretel = false;
        }
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
            if (collision.collider.CompareTag("BossBuff"))
            {
                ChangeState(RollingAttack_State.Instance);
                Destroy(collision.gameObject);
            }

        }
    }

    #endregion

    public bool CheckRange()        // Smash �Ÿ�������
    {
        if (Vector3.Distance(myTarget.transform.position, transform.position) <= SmashAttackRange && LookPlayer)
        {
            return true;
        }

        return false;

    }
    public float CheckRange(int i)
    {
        return Vector3.Distance(myTarget.transform.position, transform.position);
    }

    public void ResetState()        //Hansel ����
    {
        CurrentHP = HanselHP;
        state = new StateMachine<Hansel>();
        state.Initial_Setting(this, HanselIdelState.Instance);
        _isStuned = false;
        Isinvincibility = false; 
    }     
    public void ResetState_2()      
    {
        CurrentHP = HanselHP;
        state.Initial_Setting(this, HanselIdelState.Instance);
        Ani.Play("Cry_End");
        _isStuned = false;
        Isinvincibility = false; 
    }
    public void ResetState_Protected()      
    {
        CurrentHP = HanselHP;
        //state.Initial_Setting(this, HanselIdelState.Instance);
        Ani.Play("Cry_End");
        _isStuned = false;
        Isinvincibility = false;
    }

    public void inPhaseTwo()        //������2 �̺�Ʈ
    {

        ResetState_2();
        MoveWall();
        Lamp.SetActive(false);
        PhaseChecker++;
        Ani.Play("Cry_End");
        WorldSound.GetComponent<WorldSound>().WorldSoundStop("1StageBoss_LightElectricity");
        WorldSound.GetComponent<WorldSound>().WorldSoundPlay("1StageBoss_BurningFire", 1);
        WorldSound.GetComponent<WorldSound>().WorldSoundPlay("1StageBoss_PotBoilling", 2);
        WorldSound.GetComponent<WorldSound>().WorldSoundPlay("1StageBoss_Cooking", 3);
        pazeTwoEvent.Invoke();
    }

    void MoveWall()  //Phase2 Event �� �о����⶧���� ���� ��ġ �̵� �� ������ �߻��ϴ� ����Ʈ��ġ �̵�
    {
        WallL.transform.position = new Vector3(WallL.transform.position.x - 8, WallL.transform.position.y, WallL.transform.position.z);
        WallR.transform.position = new Vector3(WallR.transform.position.x + 8, WallR.transform.position.y, WallR.transform.position.z);

        RushPunch_L.transform.position = new Vector3(RushPunch_L.transform.position.x - 8, RushPunch_L.transform.position.y, RushPunch_L.transform.position.z);
        RushPunch_end_L.transform.position = new Vector3(RushPunch_end_L.transform.position.x - 8, RushPunch_end_L.transform.position.y, RushPunch_end_L.transform.position.z);

        RushPunch_R.transform.position = new Vector3(RushPunch_R.transform.position.x + 8, RushPunch_R.transform.position.y, RushPunch_R.transform.position.z);
        RushPunch_end_R.transform.position = new Vector3(RushPunch_end_R.transform.position.x + 8, RushPunch_end_R.transform.position.y, RushPunch_end_R.transform.position.z);
        RollingPoint[0].transform.position = new Vector3(RollingPoint[0].transform.position.x - 8, RollingPoint[0].transform.position.y, RollingPoint[0].transform.position.z);
        RollingPoint[2].transform.position = new Vector3(RollingPoint[2].transform.position.x - 8, RollingPoint[2].transform.position.y, RollingPoint[2].transform.position.z);
        RollingPoint[1].transform.position = new Vector3(RollingPoint[1].transform.position.x + 8, RollingPoint[1].transform.position.y, RollingPoint[1].transform.position.z);
        RollingPoint[3].transform.position = new Vector3(RollingPoint[3].transform.position.x + 8, RollingPoint[3].transform.position.y, RollingPoint[3].transform.position.z);
    }

    public bool RandCalculate(int RandNum)     //���� % cul. ���������� �����
    {
        var m_rand = UnityEngine.Random.Range(1, 101);

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
       ForSmashP = m_Srand;
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

    #region ==Ray==
    private RaycastHit m_hit;
    private RaycastHit m_WallHit;
    private RaycastHit m_PlayerHit;
    [Header("=====================")]
    [Header("�� Ž�� Ray ����")][SerializeField] private float m_RayDistance = 10;
    [Header("�� �浹 Ray ����")][SerializeField] private float m_ColDistnace = 3;
    [Header("�÷��̾� Ž�� Ray ����")][SerializeField] private float m_RayPlayerDistance = 5;
    [SerializeField] private LayerMask m_LayMask;
    [SerializeField] private LayerMask m_PlayerLayMask;

    void RayCheckWall()     //��Ž�� // �浹Ž�� // ��ġ�� Ž��
    {
        UnityEngine.Debug.DrawRay(transform.position + Vector3.up, transform.forward * m_RayDistance, Color.green);
        UnityEngine.Debug.DrawRay(transform.position + Vector3.up, transform.forward * m_ColDistnace, Color.red);
        UnityEngine.Debug.DrawRay(transform.position, transform.forward * m_RayPlayerDistance, Color.blue);

        #region distance from the wall
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out m_hit, m_RayDistance, m_LayMask))
        {
            Col_with_Wall = true;
        }
        else
        {
            Col_with_Wall = false;
        }
        #endregion

        #region distance form ColWall
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out m_WallHit, m_ColDistnace, m_LayMask))
        {
            Colli_EndRush = true;
        }
        else
        {
            Colli_EndRush = false;
        }
        #endregion

        #region CheckBellyRay
        if (Physics.Raycast(transform.position, transform.forward, out m_PlayerHit, m_RayPlayerDistance, m_PlayerLayMask))
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
        #endregion

    }
    #endregion



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
        rb.AddForce(transform.forward * val*1.5f, ForceMode.VelocityChange);  
    }

    //-----------------------------------------------------------------//
    #endregion

    //--------------�÷��̾� ������ ���ݿ� Animation ��Ʈ�ѷ�-------------//
    protected void SetParent_R()
    {
        GameObject Player = myTarget;
        Player.transform.SetParent(HanselHand_R ,true);
        Player.transform.position = HanselHand_R.transform.position;
    }

    protected void SetParent_L()
    {
        
        GameObject Player = myTarget;
        Player.transform.SetParent(HanselHand_L , true);
        Player.transform.position = HanselHand_L.transform.position;
    }

    protected void SetParentnullWithForce( )
    {
        GameObject Player = myTarget;
        Player.transform.SetParent(null , true);
        myTarget.GetComponent<CapsuleCollider>().isTrigger = false;
        myTarget.GetComponent<PlayerController>().SetThrowState(false);
        myTarget.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    protected void ThrowPlayerYeet()
    {

        var Dir = new Vector3(this.transform.rotation.eulerAngles.x, 0, 0).normalized;
        //myTarget.GetComponent<Rigidbody>().AddForce(Dir * 200 * TP_Force, ForceMode.Impulse);
        myTarget.GetComponent<Rigidbody>().AddForce(this.transform.forward.normalized* 50 * TP_Force, ForceMode.VelocityChange);


    }


    protected void Particle_anime(string name)
    {
        if (name == "Attack01" || name == "Attack02" || name == "Attack03" || name == "RushPunch_End" || name == "RushPunch")
        {
           
            if(gameObject.transform.rotation.eulerAngles.y >= 89 && gameObject.transform.rotation.eulerAngles.y <= 91) //enlerAngles �Ҽ��� ���� ���� 
            {
                Debug.Log(name + "_R");
                m_Particle[name + "_R"].Play();
            }
            else
            {
                Debug.Log(name + "_L");
                m_Particle[name + "_L"].Play();
            }
        }
        else
        {
            m_Particle[name].Play();
        }
    }

    protected void Particle_anime_end(string name)
    {
        m_Particle[name].Stop();
    }
    
    public void HanselSound(string name)
    {
        SoundManager.PlayVFXSound(name, transform.position);
    }





    //================================================================//
    public void OnDamage(int PlayerDamage, Vector3 pos)
    {
        Debug.Log("asd");
        if (!Isinvincibility)
        {
            CurrentHP -= PlayerDamage;
            isAttacked = true;
            Debug.Log("asd2");
            //HanselHitEvent.Invoke();

            //���� Hit �̺�Ʈ
            if (Ani.GetFloat("H_Walk") <= 0.5f)
            {
                if( (!isRushing && !isRolling && !_isStuned && !isBelly && !isTP && !isSmash))
                {
                    Ani.SetFloat("H_Walk", 2);
                    Ani.Play("I&W");
                }
            }
            Debug.Log("asd3");
            if (myTarget.transform.position.x < gameObject.transform.position.x)
            {
                m_Particle["OnDamage_L"].Play();
            }
            else
            {
                m_Particle["OnDamage_R"].Play();
            }

            Debug.Log(PhaseChecker);
            Debug.Log("CurrentHP"+ CurrentHP);
            if (PhaseChecker == 1 && CurrentHP <= 10)
            {
                ChangeState(Stun_State.Instance);   //���� ������Ʈ -> �ִϸ��̼� ���                
                StartCoroutine(PazeTwoRoutine());
                m_CountHit = 0;
            }
            if (PhaseChecker == 2 && CurrentHP <= 0 &&(!isRushing && !isRolling && !_isStuned && !isBelly && !isTP))
            {
                StunMove = true;
                Ani.Play("I&W");
                ChangeState(HanselStunMoveState_Y.Instance);
            }
        }

        else 
        {
            if (m_Gretel.GetComponent<Gretel>()._Ani.GetBool("DamageTime")) 
            {
                //�׷��� Hit �̺�Ʈ
                m_Gretel.GetComponent<Gretel>().CurrentHP -= PlayerDamage;
                GretelHitEvent.Invoke();

                if (myTarget.GetComponent<PlayerController>().lookVector.x == 1)
                {
                    m_Particle["OnDamage_L"].Play();
                }
                else
                {
                    m_Particle["OnDamage_R"].Play();
                }
            }
        }
    }

    public void GretelAble()
    {
        EventSystem.SetActive(true);   //�� �Ѿ�� �̺�Ʈ
        m_Gretel.GetComponent<Gretel>()._Ani.SetBool("GretelEnable", true);
    }

    private IEnumerator PazeTwoRoutine()
    {
        yield return YieldInstructionCache.waitForSeconds(3);
        CameraController.CameraShaking(0.8f, 1f);
        yield return YieldInstructionCache.waitForSeconds(1.2f);
        var targetPos = new Vector3(-5, -41.9f, 25.5f);
        while (Vector3.Distance(m_Gretel.transform.parent.localPosition, targetPos) >= 0.08f)
        {
            //m_Gretel.transform.parent.localPosition = Vector3.MoveTowards(m_Gretel.transform.parent.localPosition, targetPos, Time.deltaTime * 27);            
            m_Gretel.transform.parent.localPosition = Vector3.Lerp(m_Gretel.transform.parent.localPosition, targetPos, Time.deltaTime * 7.2f);
            yield return YieldInstructionCache.waitForFixedUpdate;
        }        
        yield return YieldInstructionCache.waitForSeconds(0.5f);
        GretelAble();
        m_Gretel.transform.parent.localRotation = Quaternion.Euler(0, 180, 0);
    }

    public void OnRecovery(int heal)
    {

    }



}