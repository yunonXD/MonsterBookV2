using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Anna : MonoBehaviour , IEntity
{
    #region Anna HP Part
     public float AnnaHP_Phase1 = 100;             //������ 1
     public float AnnaHP_Phase2;             //������ 2
    [HideInInspector] public float AnnaHP_Phase3 ;             //������ 3
    [Header("���� Anna HP")] public float Anna_CurrentHP;      //���� HP
    public int AnnaPhase = 1;
    #endregion

    [Header("Matches Normal")]
    public GameObject Matches;

    #region Anna Move
    public float MoveSpeed;
    public float MoveRandomRange;
    public int CurrentPosiiton;
    public int NextPosition1;
    public int NextPosition2;
    public GameObject[] PatrolPoint;
    public float Anna_IdleTime;
    public float Anna_IdleTime_Cur;
    public int Anna_MatchAttack_Probability;
    public int Anna_Halo_Probability;
    public int Anna_ProtectionofGod_Probability;
    public GameObject AnnaBody;
    public GameObject AnnaFace;
    public bool HitMotionAble;
    public float FlySpeed;
    public float FlyPosition;


    #endregion


    //Phase1
    [Header("Phase1")]
    public GameObject WorldEvent;
    public GameObject Halo;
    public float MatchesSpeed_1;
    public float MatchesSpeed_2;
    public float MatchesSpeed_Halo_1;
    public int Matches_Attack01_Damage;
    public int Matches_Attack02_Damage;
    public int Matches_Attack03_Damage;
    public float Match_distance;
    public GameObject MatchObjectPool;
    public Transform MatchSpawnPoint;
    public int MatchCount;
    public int HaloCount;
    public GameObject ProtectArea;
    public bool ProtectAreaActive = false;
    public GameObject[] HaloPoint;
    public GameObject[] HaloMiddlePoint;
    public GameObject[] HaloSpawnPoint;
    public bool ProtectedMoveAble = false;
    private bool BlizzardOneTime;
    public int BlizzardTriggerHP;

    //Phase2
    [Header("Phase2")]
    public float ProtectAreaRoundSpeed;
    public float AutoFollowTime;    //0 ~ 1;
    public float DownSpeed;
    public bool GroundLanding;
    public bool finishAttackAble;
    public bool AnnaFalling;
    public int Matches_Attack04_Damage;
    public int Matches_Attack05_Damage;
    public int Matches_Attack06_Damage;


    

    //etc
    [SerializeField] private StringParticle m_Anna_Particle;
    [HideInInspector] public Material[] NumMat;
   



    #region no Touch

    private StateMachine<Anna> state = null;

    [HideInInspector] public GameObject Anna_Player;
    [HideInInspector] public Animator Anna_Ani;
    [HideInInspector] public Rigidbody Anna_Rigid;
    [HideInInspector] public CapsuleCollider Anna_CapCol;

    [HideInInspector] public bool Isinvincibility = false;
    [HideInInspector] public int Anna_
        = 1;

    #endregion


    protected void Awake()
    {
        Anna_Ani = GetComponent<Animator>();
        Anna_Rigid = GetComponent<Rigidbody>();
        Anna_CapCol = GetComponent<CapsuleCollider>();
        //Anna_Player = GameObject.FindGameObjectWithTag("Player");
        //ResetState_Anna(Anna_PhaseChecker);
    }


    protected void Start()
    {
        BlizzardOneTime = false;
        state = new StateMachine<Anna>();
        GroundLanding = false;
        Anna_CurrentHP = AnnaHP_Phase1;
   
        state.Initial_Setting(this, Anna_Wait_State.Instance);



    }

    protected void FixedUpdate()
    {



        state.Update();

        if (AnnaPhase == 1 && Anna_CurrentHP <= 0)   //�ȳ� 2������ ����
        {
            ResetState_Anna(2);
            WorldEvent.GetComponent<Anna_WorldEvent>().BlizzardEnd();

        }


        if (AnnaPhase == 2 && Anna_CurrentHP <= 0)
        {
            ChangeState(Anna_Grogy_State.Instance);

        }



        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("�̵��Է�");
            ChangeState(AnnaMove_State.Instance);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("����1�Է�");
            ChangeState(Anna_Match_Attack.Instance);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Phase UP");
            if (AnnaPhase == 1)
            {
                HaloCount = 3;
                AnnaPhase++;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AnnaStand();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            OnDamage(10,Vector3.zero);
        }
    }

    public void AnnaStand()
    {
        Anna_Ani.SetTrigger("Anna_Stand");
    }

    public void Anna_StandEnd()
    {
        ChangeState(Anna_Fly_State.Instance);
    }

    public void ProtectedMove()
    {
        ProtectedMoveAble = true;
    }

    public void finishAtackAble()
    {
        finishAttackAble = true;
    }

    public void AnnaFall()
    {
        AnnaFalling = true;
    }

    protected void following_Matches(int MatchNum)
    {



    }


    #region No Reference
    public void ChangeState(FSM_State<Anna> _State)
    {
        state.ChangeState(_State);
    }

    protected void ResetState_Anna(int phase)
    {
        //����� ���� �ٸ� �ʱ�ȭ
        switch (phase)
        {
            case 1:
                Anna_CurrentHP = AnnaHP_Phase1;
                AnnaPhase = 1;
                break;

            case 2:
                Anna_CurrentHP = AnnaHP_Phase2;
                Debug.Log("�ȳ� ������2 HP : "+AnnaHP_Phase2);
                AnnaPhase = 2;
                //NumMat = AnnaBody.GetComponent<SkinnedMeshRenderer>().material;
                //AnnaBody.GetComponent<SkinnedMeshRenderer>().materials = NumMat;
                break;

            case 3:
                Anna_CurrentHP = AnnaHP_Phase3;

                break;

        }


    }

    #endregion

    #region For Animation 

    protected void ParticleBoi_Anna(string name)
    {
        m_Anna_Particle[name].Play();
    }

    #endregion


    public void ChangeState_Idle()
    {
        ChangeState(Idle_State.Instance);
    }
    public void ChangeState_Attack()   //Ȯ�� �߰� �ʿ�
    {
        if (ProtectAreaActive == true)
        {
            ProtectArea.SetActive(false);
            ProtectAreaActive = false;
            ChangeState(Idle_State.Instance);
            return;
        }


        var Randomvalue = Random.Range(0, 100);
        Debug.Log(Randomvalue + ": ������");
        if(Randomvalue <= Anna_MatchAttack_Probability)
        {
            ChangeState(Anna_Match_Attack.Instance);
        }
        else if (Anna_MatchAttack_Probability < Randomvalue && Randomvalue <= Anna_MatchAttack_Probability+ Anna_Halo_Probability)
        {
            ChangeState(Anna_Halo_Attack.Instance);
        }
        else if (Anna_MatchAttack_Probability + Anna_Halo_Probability < Randomvalue && Randomvalue <= Anna_MatchAttack_Probability + Anna_Halo_Probability + Anna_ProtectionofGod_Probability)
        {
            ChangeState(Anna_Protected_Attack.Instance);
        }
        else
        {
            Debug.Log(Randomvalue + "Ȯ�� Range ����");
        }
    }
    public void ChangeState_Move()
    {
        ChangeState(AnnaMove_State.Instance);  

    }


    //===================================================//
    public void OnDamage(int playerDamage, Vector3 pos)
    {
        if (!Isinvincibility)
        {
            Anna_CurrentHP -= playerDamage;
        }
        else
        {
            Debug.LogError(" *Anna is invincible !! ");
        }

        if(HitMotionAble == true)   //Idle State�� Ÿ�ݽ� 
        {
            Anna_Ani.SetTrigger("Anna_Hit");    
        }


        if (finishAttackAble == true)
        {
            Anna_Ani.SetTrigger("Anna_Death");
            Isinvincibility = true;
        }

        if(Anna_CurrentHP < BlizzardTriggerHP && AnnaPhase == 1 && BlizzardOneTime == false)
        {
            WorldEvent.GetComponent<Anna_WorldEvent>().BlizzardStart();
        }

        if (Anna_CurrentHP < BlizzardTriggerHP && AnnaPhase == 1 && BlizzardOneTime == false)
        {
            WorldEvent.GetComponent<Anna_WorldEvent>().GrannyAble = true;
        }




    }

    public void OnRecovery(int heal)
    {
       
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Ground")
        {
            GroundLanding = true;
        }
    }
}
