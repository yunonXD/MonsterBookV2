using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MacaronController : MonoBehaviour, IEntity, ICutOff
{
    public enum CurrentState { idle, trace, attack}

    [Header("[Stat]")]
    private string Name = "Macaron";    
    public float MaxHP = 20.0f;           
    public float CurHP;                    
    public float WalkSpeed = 0.1f;     
    public float RunSpeed = 12.0f;      
    public float AttackDelay = 3.0f;      
    public GameObject AttackArea;

    [Header("[State/ScanDist]")]
    public CurrentState State = CurrentState.idle;

    Animator animator;
    private Transform _transform;
    private Transform playerTransform;
    private GameObject player;
    private bool Patrollook = false;
    public float attackDist = 2.0f;
    private bool isDead = false;
    private bool Motioning = false;
    private bool Chasing = false;
    private bool DeadMotioning = false;
    public bool BounddaryAttack = false;
    private bool PlayerDir = false;  //false => 왼쪽, true => 오른쪽
    public GameObject BodyParts;

    void Start()
    {
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        CurHP = MaxHP;

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
        Time.timeScale = 0.1f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            CutOff();
        }
    }


    IEnumerator CheckState()
    {
        while(!isDead)
        {
            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(playerTransform.position, _transform.position);
            if (Motioning == false)     //현재 모션진행중여부 체크 -> 공격도중 거리에서 멀어져 다른 State가 되더라도 공격모션이 끊기는걸 방지
            {

                if (BounddaryAttack == true) //트리거가 켜지면
                {
                    if (dist <= attackDist)
                    {
                        State = CurrentState.attack;
                    }
                    else if (dist > attackDist)
                    {
                        State = CurrentState.trace;
                    }
                }
                else
                {
                    State = CurrentState.idle;
                }
            }
            
        }

    }

    IEnumerator CheckStateForAction()
    {
        while(!isDead) 
        {
            AttackArea.SetActive(false);
            switch (State)
            {
                case CurrentState.idle:
                    animator.Play("idle");
                    break;
                case CurrentState.trace:
                    Chasing = true;
                    LookTarget(playerTransform);
                    animator.Play("walk");
                    transform.position = new Vector3(Vector3.MoveTowards(transform.position, playerTransform.position, RunSpeed * Time.deltaTime).x, 0, playerTransform.position.z);
                    break;
                case CurrentState.attack:
                    LookTarget(playerTransform);
                    Attack();                          
                    break;
            }
            yield return null;
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

    void CutOff()
    {
        this.gameObject.SetActive(false);
        BodyParts.SetActive(true);

    }

    public void OnDamage(int damage, Vector3 pos)
    {
        if (CheckCutOff())
        {
            CutOff();
        }
        CurHP -= damage;
    }
       public void OnRecovery(int heal)
    {

    }

    void Attack()
    {
        Motioning = true;
        animator.Play("attack");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.5f) //50%이하
        {
            AttackArea.SetActive(false); //첫모션
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f && //50퍼이상 ~ 80이하
                                                                                animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7f))
        {
            AttackArea.SetActive(true);  //공격범위 활성화
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.99f) //50%이하
        {
            AttackArea.SetActive(false); //첫모션
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)  //100퍼이상
        {
            Motioning = false;  //모션종료
            AttackArea.SetActive(false);
            State = CurrentState.idle;

        }

    }
    void LookTarget(Transform Target)
    {
        if (Motioning == false)
        {
            Vector3 dir = Target.position - this.transform.position;
            Vector3 dirXZ = new Vector3(dir.x, 0.0f, dir.z);   

            Vector3 DirR = new Vector3(100, 0, 0);
            Vector3 DirL = new Vector3(150, 0, -250);

            if (dirXZ == Vector3.zero)
                return;

            if(dir.x > 0)
            {
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(DirR), 400 * Time.deltaTime);
            }
            else
            {
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(DirL), 400 * Time.deltaTime);
            }



        }

    }
}
