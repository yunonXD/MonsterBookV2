using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterFSM;
using MonsterFSM.RollRollFSM;

public class RollRoll : Non_AttackMonster
{

    public enum EChaseType
    {
        RandomChase,
        NormalChase,
        OppersiteDirection
    }


    [SerializeField]
    private EChaseType ChaseType;
    [SerializeField]
    private GameObject ObstacleObj;
    [SerializeField]
    private float CreateObstacleTime;

    private Animator AnimatorCtrl;




    public EChaseType gChaseType => ChaseType;
    public Animator gAnimator => AnimatorCtrl;
    public float gCreateObstacleTime => CreateObstacleTime;
    public GameObject gObstacleObj => ObstacleObj;


    public bool gIsChase { get; set; }
    




    protected override void Start()
    {
        base.Start();
        AnimatorCtrl = GetComponent<Animator>();
        gPatrolPoint[0].position = transform.position + new Vector3(gMoveDistance, 0.0f, 0.0f);
        gPatrolPoint[1].position = transform.position - new Vector3(gMoveDistance, 0.0f, 0.0f);

        FSM = RollRollFSMCreator.CreatePatrol(this);
    }
    


    
    public override void OnDamage(int damage ,Vector3 Pos)
    {
        gHP -= damage;
    }

    public override void OnRecovery(int heal)
    {
        gHP += heal;
    }

    
}
