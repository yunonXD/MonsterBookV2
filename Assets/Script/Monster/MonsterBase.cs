using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : MonoBehaviour, IEntity
{

    public enum EStopType
    {
        StopOnCrash,
        OppsiteDirInCaseOfCollision
    }

    [SerializeField]
    private EStopType StopType;
    
    [SerializeField]
    private bool StopJump;

        
    public abstract void OnRecovery(int heal);

    
    // 일단 임시로 protected로 설정 해놓겠습니다.
    [SerializeField] private string MonsterID;
    [SerializeField] private int HP;
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float RunSpeed;
    [SerializeField] private Transform[] PatrolPoint;
    [SerializeField] private float MoveDistance;
    [SerializeField] private int AttackDamage;
    [SerializeField] private float AttackDistance;
    [SerializeField] private float AttackDelay;
    [SerializeField] private float MaxJumpForce;
    



    // 일단 제한자를 protected로 설정 해놨습니다.
    public string gMonsterID { get { return MonsterID; } protected set { MonsterID = value; } }
    public int gHP { get { return HP; } protected set { HP = value; } }
    public float gWalkSpeed { get { return WalkSpeed; } protected set { WalkSpeed = value; } }
    public float gRunSpeed { get { return RunSpeed; } protected set { RunSpeed = value; } }
    public Transform[] gPatrolPoint { get { return PatrolPoint; } protected set { PatrolPoint = value; } }
    public float gMoveDistance { get { return MoveDistance; } protected set { MoveDistance = value; } }
    public int gAttackDamage { get { return AttackDamage; } protected set { AttackDamage = value; } }
    public float gAttackDistance { get { return AttackDistance; } protected set { AttackDistance = value; } }
    public float gAttackDelay { get { return AttackDelay; } protected set { AttackDelay = value; } }
    public float gMaxJumpForce { get { return MaxJumpForce; } protected set { MaxJumpForce = value; } }
    public int CurrentPatrol{get; set;}
    public bool gStopJump => StopJump;
    public EStopType gStopType => StopType;


    public Bounds gBounds {get{
        if (PlayerColloder == null)
            PlayerColloder = transform.GetComponent<CapsuleCollider>();

        return PlayerColloder.bounds;
    }}


    public bool CapsuleCastCheck()
    {

        float capsuleScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
        if (Physics.CapsuleCast(transform.position, transform.position, capsuleScale, transform.forward, 0.1f))
        {
            return true;
        }
        return false;
    }





    private CapsuleCollider PlayerColloder;


    public abstract void OnDamage(int damage, Vector3 pos);
    


}
