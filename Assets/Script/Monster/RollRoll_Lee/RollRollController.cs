using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollRollController : MonoBehaviour, IEntity, ICutOff
{
    public enum CurrentState { idle, run, walk , Hit}

    [Header("[Stat]")]
    private string Name = "RollRoll";
    public float MaxHP = 10.0f;
    public float CurHP;
    public float WalkSpeed = 2.0f;
    public float RunSpeed = 12.0f;
    public float AttackDelay = 3.0f;
    public bool MoveType;

    [Header("[State/ScanDist]")]
    public CurrentState State = CurrentState.idle;
    public GameObject PatrolPoint1;
    public GameObject PatrolPoint2;

    Animator animator;
    private Transform _transform;
    private Transform playerTransform;
    private GameObject player;
    private bool Patrollook = false;
    private bool isDead = false;
    private bool Motioning = false;
    private bool DeadMotioning = false;
    public bool BounddaryAttack = false;
    private bool PlayerDir = false;  //false => 왼쪽, true => 오른쪽
    public GameObject BodyParts;
    private bool Possible_Move = true;

    void Start()
    {
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        CurHP = MaxHP;

        if (MoveType == true)
        {
            _transform.position = new Vector3(playerTransform.position.x, _transform.position.y, _transform.position.z); //x축 player 보간
        }
        else if (MoveType == false)
        {
            _transform.position = new Vector3(_transform.position.x, _transform.position.y, playerTransform.position.z); //z축 player 보간
        }

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }

    void Update()
    {

    }




    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(playerTransform.position, _transform.position);
            if (Motioning == false)     //현재 모션진행중여부 체크 -> 공격도중 거리에서 멀어져 다른 State가 되더라도 공격모션이 끊기는걸 방지
            {
                if (Possible_Move == true)
                {
                    if (BounddaryAttack == true)
                    {
                        State = CurrentState.run;
                    }
                }
            }
        }

    }

    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            switch (State)
            {
                case CurrentState.walk:
                    animator.Play("walk");
                    Patrol();
                    break;
                case CurrentState.run:
                    animator.Play("run");
                    RunAway();
                    break;
                case CurrentState.Hit:
                    Hit();
                    break;
                case CurrentState.idle:
                    animator.Play("idle");
                    
                    if(animator.GetCurrentAnimatorStateInfo(0).IsName("idle") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        State = CurrentState.walk;
                    }
                    break;

            }
            yield return null;
        }
    }



    public void OnDamage(int damage, Vector3 pos)
    {
        //if(CheckCutOff())
        //{
        //    CutOff();
        //}

        CurHP -= damage;
        BounddaryAttack = true;
        State = CurrentState.Hit;
        Motioning = true;
    }
    public void OnRecovery(int heal)
    {
        
    }
    void Hit()
    {
        if (DeadMotioning == false)
        {
            animator.Play("Hit");
            Motioning = true;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Motioning = false;
                Possible_Move = true;
                if (CurHP <= 0)
                {
                    //State = CurrentState.dead;
                }
                else
                {
                    State = CurrentState.idle;
                }
            }
        }

    }

    public bool CheckCutOff()
    {
        if (CurHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CutDamage()
    {
        gameObject.SetActive(false);
        BodyParts.SetActive(true);
    }

    void CutOff()
    {
        this.gameObject.SetActive(false);
        BodyParts.SetActive(true);

    }

    void Patrol()
    {
        if (MoveType == false)
        {

            if (Patrollook == false) //PatrolPoint1 -> PatrolPoint2로 이동
            {
                transform.position = Vector3.MoveTowards(transform.position, PatrolPoint2.transform.position, WalkSpeed * Time.deltaTime);
                //LookTarget(PatrolPoint2.transform);
                LookTarget(true);
                if (transform.position.x == PatrolPoint2.transform.position.x)
                {
                    State = CurrentState.idle;
                    Patrollook = true;
                }

            }
            else                    //PatrolPoint1 -> PatrolPoint2로 이동
            {
                transform.position = Vector3.MoveTowards(transform.position, PatrolPoint1.transform.position, WalkSpeed * Time.deltaTime);
                //LookTarget(PatrolPoint1.transform);
                LookTarget(false);
                if (transform.position.x == PatrolPoint1.transform.position.x)
                {
                    State = CurrentState.idle;
                    Patrollook = false;
                }
            }
        }

        if (MoveType == true)
        {

            if (Patrollook == false) //PatrolPoint1 -> PatrolPoint2로 이동
            {
                transform.position = Vector3.MoveTowards(transform.position, PatrolPoint2.transform.position, WalkSpeed * Time.deltaTime);
                //LookTarget(PatrolPoint2.transform);
                LookTarget(true);
                if (transform.position.z == PatrolPoint2.transform.position.z)
                {
                    State = CurrentState.idle;
                    Patrollook = true;
                }

            }
            else                    //PatrolPoint1 -> PatrolPoint2로 이동
            {
                transform.position = Vector3.MoveTowards(transform.position, PatrolPoint1.transform.position, WalkSpeed * Time.deltaTime);
                //LookTarget(PatrolPoint1.transform);
                LookTarget(false);
                if (transform.position.z == PatrolPoint1.transform.position.z)
                {
                    State = CurrentState.idle;
                    Patrollook = false;
                }
            }
        }
    }

    void RunAway()
    {
        if (MoveType == false)
        {

            if (Patrollook == false) //PatrolPoint1 -> PatrolPoint2로 이동
            {
                transform.position = Vector3.MoveTowards(transform.position, PatrolPoint2.transform.position, RunSpeed * Time.deltaTime);
                LookTarget(true);
                if (transform.position.x == PatrolPoint2.transform.position.x)
                {
                    Patrollook = true;
                }

            }
            else                    //PatrolPoint1 -> PatrolPoint2로 이동
            {
                transform.position = Vector3.MoveTowards(transform.position, PatrolPoint1.transform.position, RunSpeed * Time.deltaTime);
                LookTarget(false);
                if (transform.position.x == PatrolPoint1.transform.position.x)
                {
                    Patrollook = false;
                }
            }
        }

        if (MoveType == true)
        {

            if (Patrollook == false) //PatrolPoint1 -> PatrolPoint2로 이동
            {
                transform.position = Vector3.MoveTowards(transform.position, PatrolPoint2.transform.position, RunSpeed * Time.deltaTime);
                LookTarget(true);
                if (transform.position.z == PatrolPoint2.transform.position.z)
                {
                    Patrollook = true;
                }

            }
            else                    //PatrolPoint1 -> PatrolPoint2로 이동
            {
                transform.position = Vector3.MoveTowards(transform.position, PatrolPoint1.transform.position, RunSpeed * Time.deltaTime);
                LookTarget(false);
                if (transform.position.z == PatrolPoint1.transform.position.z)
                {
                    Patrollook = false;
                }
            }
        }
    }

    void LookTarget(bool dir)
    {
        if (Motioning == false)
        {

            Vector3 DirR = new Vector3(100, 0, 0);
            Vector3 DirL = new Vector3(-100, 0, -200);


            if (dir == true)
            {
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(DirR), 600 * Time.deltaTime);
            }
            else if(dir == false)
            {
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(DirL), 600 * Time.deltaTime);
            }

        }

    }
}
