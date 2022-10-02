using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gretel : MonoBehaviour, IEntity
{
    public float GretelHP = 100;
    public float CurrentHP;
    [HideInInspector] public Animator Ani = null;
    public StateMachine<Gretel> state = null;
    private bool isDead = false;

    public GameObject KnifeObject;
    public GameObject ProtectiveArea;

    #region  KnifeAttack var
    public float KnifeDownTime;
    public float KnifeFollowTime;
    public float KnifefollowSpeed;
    public float KnifeDownSpeed;
    public float KnifeAttackCount; //나이프 공격횟수

    #endregion

    #region  SoupAttack var
    public Transform SoupRangePoint1;
    public Transform SoupRangePoint2;
    public int SoupMin = 3;
    public int SoupMax = 4;
    public GameObject SoupObjectPool;
    public float SoupResponseTime = 1.2f;
    public float SoupDeleteTime = 2.0f;
    public float SolidProbability;
    #endregion

    #region  SoupAttack var
    public float HitTime;
    #endregion

    [HideInInspector] public GameObject myTarget;
    [HideInInspector] public GameObject Hansel;

   
    // Start is called before the first frame update
    void Start()
    {
        Hansel = GameObject.FindWithTag("Boss");
        myTarget = GameObject.FindWithTag("Player");
        CurrentHP = GretelHP;
        state = new StateMachine<Gretel>();
        state.Initial_Setting(this, Knife_Attack_State.Instance);

        
    }

    // Update is called once per frame
    void Update()
    {


        state.Update();
    }

    public void ChangeState(FSM_State<Gretel> _State)
    {
        state.ChangeState(_State);
    }


    public void OnDamage(int PlayerDamage, Vector3 pos)
    {
      if (CurrentHP <= 0)
      {
          // ChangeState(HnaselDie_State.Instance);
      }
            CurrentHP -= PlayerDamage;

    }

    public void OnRecovery(int heal)
    {
        throw new System.NotImplementedException();
    }

}
