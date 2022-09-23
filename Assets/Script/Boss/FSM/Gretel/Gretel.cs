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

    #region  KnifeAttack var
    public float KnifeFollowTime = 5.0f;
    public float KnifeDownTime = 7.0f;
    public float KnifefollowSpeed = 20.0f;
    public float KnifeDownSpeed = 20.0f;
    public float KnifeAttackCount; //나이프 공격횟수

    #endregion

    #region  SoupAttack var
    public float SoupDownSpeed = 10.0f; // 현재 Rigidbody Gravity를 사용중으로 미사용
    public float SoupSpawnRange = 5.0f;
    public Transform SoupRangePoint1;
    public Transform SoupRangePoint2;
    public int SoupMin = 3;
    public int SoupMax = 4;
    public GameObject SoupObjectPool;
    public float SoupResponseTime = 1.2f;
    public float SoupDeleteTime = 2.0f;
    public float SolidProbability;
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
        if(Hansel.GetComponent<Hansel>().CurrentHP < 30)
        {
            Debug.Log("protect");
        }
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
