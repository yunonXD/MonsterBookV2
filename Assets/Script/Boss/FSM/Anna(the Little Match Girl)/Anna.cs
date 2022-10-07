using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Anna : MonoBehaviour , IEntity
{
    #region Anna HP Part
    [HideInInspector] public float AnnaHP_Phase1 = 100;             //페이즈 1
    [HideInInspector] public float AnnaHP_Phase2 = 100;             //페이즈 2
    [HideInInspector] public float AnnaHP_Phase3 = 100;             //페이즈 3
    [Header("현재 Anna HP")] public float Anna_CurrentHP = 100;      //현재 HP
    #endregion

    [Header("Matches Normal")]
    public GameObject Matches;



    //Phase1
    [Header("TossingMatches")]
    public float MatchesSpeed = 4;


    //Phase2



    //Phase3





    //etc
    [SerializeField] private StringParticle m_Anna_Particle;



    #region no Touch

    private StateMachine<Anna> state = null;

    [HideInInspector] public GameObject Anna_Player;
    [HideInInspector] public Animator Anna_Ani;
    [HideInInspector] public Rigidbody Anna_Rigid;
    [HideInInspector] public CapsuleCollider Anna_CapCol;

    [HideInInspector] public bool Isinvincibility = false;
    [HideInInspector] public int Anna_PhaseChecker = 1;

    #endregion


    protected void Awake()
    {
        Anna_Ani = GetComponent<Animator>();
        Anna_Rigid = GetComponent<Rigidbody>();
        Anna_CapCol = GetComponent<CapsuleCollider>();
        Anna_Player = GameObject.FindGameObjectWithTag("Player");
        ResetState_Anna(Anna_PhaseChecker);

    }


    protected void Start()
    {

    }

    protected void FixedUpdate()
    {
        state.Update();
    }



    protected void following_Matches(int MatchNum)
    {



    }


    #region No Reference
    protected void ChangeState(FSM_State<Anna> _State)
    {
        state.ChangeState(_State);
    }

    protected void ResetState_Anna(int phase)
    {
        //페이즈에 따라 다른 초기화
        switch (phase)
        {
            case 1:
                Anna_CurrentHP = AnnaHP_Phase1;

               

                ChangeState(Anna_Phase1.Instance);

                break;

            case 2:
                Anna_CurrentHP = AnnaHP_Phase2;

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

    }

    public void OnRecovery(int heal)
    {
       
    }
}
