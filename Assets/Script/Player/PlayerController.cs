using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Schema;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public enum PlayerState
{
    IdleState,
    WalkState,
    DashState,
    AttackState,
    AttackReturnState,
    JumpState,
    WireSearchState,
    WireThrowState,
    WireState,
    WireAttackState,
    DeadState,
    FallState,
    HitState,
    CuttingState,
    CuttingReturnState,
    LandState,
    KnockBackState,
}

public enum Axis
{ 
    XAxis,
    ZAxis,
}

[Serializable]
public class StringParticle : SerializableDictionary<String, ParticleSystem> { }
[Serializable]
public class StringGameObject : SerializedDictionary<String, GameObject> { }

public class SomeComparer : IEqualityComparer<PlayerState>
{
    bool IEqualityComparer<PlayerState>.Equals(PlayerState x, PlayerState y) { return (int)x == (int)y; }
    int IEqualityComparer<PlayerState>.GetHashCode(PlayerState obj) { return ((int)obj).GetHashCode(); }
}

public class PlayerController : MonoBehaviour, IEntity, IKnockBack, IRotate
{
    #region Component
    [HideInInspector] public Rigidbody rigid;
    [HideInInspector] public Animator ani;
    public CapsuleCollider collid { get; private set; }
    public InputManager input { get;  set; }
    [HideInInspector] public UIController ui;
    #endregion

    #region State    
    private Dictionary<PlayerState, IState> stateDic = new Dictionary<PlayerState, IState>(new SomeComparer());
    private IState curState;
    private IState prevState;

    #endregion

    #region PlayerValue
    [Header("[Plyaer State]")]
    [ReadOnly] public PlayerState state;
    public bool mode;
    [SerializeField] private int maxHP;
    [ReadOnly][SerializeField] private int curHP;
    [SerializeField] private float invinTime;
    public bool invinBool { get; set; }
    public int str;

    #endregion

    #region PlayerMove
    [Header("[Player Move]")]
    public Axis curAxis;
    public float walkSpeed = 4;
    public float jumpForce = 12;   
    public float dashForce = 20;
    public float wireForce;
    [ReadOnly] public int attackCount;
    [ReadOnly] public bool prevInput;

    public float walkVector { get; set; }

    #endregion

    #region Move Value
    [Header("[Move Value]")]
    [ReadOnly] public bool isGround;
    public LayerMask wallLayer;
    public LayerMask groundLayer;
    public LayerMask wireLayer;
    [SerializeField] private LayerMask monsterLayer;
    public Vector3 lookVector { get; set; }

    public WireObject wireObject { get; set; }
    public Vector3 wirePos { get; set; }

    public bool canAxisRoate;

    #endregion

    #region Inspector
    [Header("[Component Property]")]
    public Weapon[] attackWeapon;    
    public Transform wireStart;    
    [SerializeField] private float gameSpeed = 1;
    public LineRenderer line;
    public Transform effectEuler;

    [Header("[Effect Prefab]")]
    [SerializeField] private StringParticle particle;
    [SerializeField] private StringGameObject particleObj;

    [Header("[Editor]")]
    [SerializeField] private Vector3 drawPos;
    [SerializeField] private Vector3 drawSize;

    #endregion

    #region Initialize
    private void Awake()
    {
        input = GetComponent<InputManager>();
        rigid = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        collid = GetComponent<CapsuleCollider>();
        ui = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        wireObject = GameObject.FindGameObjectWithTag("WireObject").GetComponent<WireObject>();

    }

    private void Start()
    {
        int enumCount = Enum.GetValues(typeof(PlayerState)).Length;

        for (int i = 0; i < enumCount; i++)
        {
            var name = ((PlayerState)i).ToString();
            var state = gameObject.AddComponent(Type.GetType(name)) as IState;

            stateDic.Add((PlayerState)i, state);
        }

        attackWeapon[0].SetDamage(str);
        attackWeapon[1].SetDamage(str * 3);

        PlayerInitialize();        
    }

    #endregion

    #region Anim Event
    private void AttackBoxOn(int i)
    {
        attackWeapon[i].Collider(true); 
    }

    public void AttackBoxOff(int i)
    {
        attackWeapon[i].Collider(false);
    }

    private void AttackMove(float value)
    {
        rigid.AddForce(lookVector * value, ForceMode.Impulse);
    }

    private void DashInit()
    {
        rigid.velocity = Vector3.zero;
    }

    private void AttackEffect(int i)
    {
        particle["Attack" + i.ToString()].Play();
    }

    public void ParticlePlay(string name)
    {        
        particle[name].Play();
    }

    public void ParticlShot(string name)
    {
        Instantiate(particleObj[name], transform.position, particleObj[name].transform.rotation);
    }

    private void SetIdle()
    {
        if (state != PlayerState.IdleState) ChangeState(PlayerState.IdleState);
    }

    private void ShotWire()
    {
        wireObject.Shot(wireStart.position, new Vector3(wirePos.x, wirePos.y, wirePos.z));
        line.enabled = true;
    }

    #endregion
    int statcount;
    public void ChangeState(PlayerState next)
    {
        if (!stateDic.ContainsKey(next) || !curState.CheckState(next) || state == next) return;
        //Debug.Log(vaff++ + " =Count");
        prevState = curState;
        prevState.OnStateExit(this);
        stateDic[next].OnStateEnter(this);
        curState = stateDic[next];
        //Debug.Log("ChangeState " + vaff++ + "  " + state.ToString() + " -> " + next.ToString());        
        Debug.Log("ChangeState : " + ++statcount + " / " + state.ToString()+attackCount + "=>" + next.ToString());
        state = next;
    }

    public void AgainState()
    {
        curState.OnStateExit(this);
        curState.OnStateEnter(this);
        Debug.Log("AgainState : " + state.ToString());
    }

    private void FixedUpdate()
    {
        ani.SetBool("Trans", mode);
        Time.timeScale = gameSpeed;
        CheckGround();
        if (walkVector != 0) ChangeState(PlayerState.WalkState);
        curState.OnStateExcute(this);
    }

    public void Move()
    {
        if (CheckMonster()) rigid.velocity = Vector3.zero;
        else if (CheckWall())
        {
            var move = curAxis == Axis.XAxis ? new Vector3(walkVector * walkSpeed, rigid.velocity.y, 0) : new Vector3(0, rigid.velocity.y, walkVector * walkSpeed);
            rigid.velocity = move;
        }
    }

    #region Setter
    public void SetWalkVector(float value)
    {
        walkVector = value > 0 && value != 0 ? 1 : -1;
    }

    public void SetTimeScale(float speed)
    {
        gameSpeed = speed;
    }

    public void SetChange()
    {
        CameraController.RotateCameraView(new Vector3(0, -90));
    }

    public void SetSTR(int value) { str = value; }    

    public void SetInvincible(bool value) { invinBool = value; }

    public void SetCurHP(int hp) { curHP = hp; }

    public void SetMaxHP(int hp) { maxHP = hp; }

    public void SetThrowState(bool value)
    {
        input.SetInputAction(!value);
        rigid.isKinematic = value;
    }

    #endregion

    #region Command
    public void SetSTR(string value)
    {
        var str = int.Parse(value);
        this.str = str;
    }

    public void SetInv(string value)
    {
        var val = bool.Parse(value);
        invinBool = val;
    }

    public void SetHP(string value)
    {
        var val = int.Parse(value);
        curHP = val;
    }

    public void SetMaxHP(string value)
    {
        var val = int.Parse(value);
        maxHP = val;
    }

    #endregion

    #region Getter
    public PlayerState GetCurState() { return state; }

    public IState GetPrevState() { return prevState; }

    public IState GetStateDictionary(PlayerState state) { return stateDic[state]; }

    #endregion

    #region Interface
    public void OnDamage(int damage, Vector3 pos)
    {
        if (invinBool) return;
        var look = pos.x > transform.position.x ? 1 : -1;
        transform.localScale = new Vector3(2, 2, 2 * look);
        lookVector = new Vector2(look, 0);
        ChangeState(PlayerState.HitState);
        curHP -= damage;
        if (curHP <= 0)
        {
            curHP = 0;
            Dead();
        }
        ui.UpdateHP(curHP);
    }

    public void OnRecovery(int heal)
    {

    }

    public void Dead()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CheckRotate(bool value)
    {
        canAxisRoate = value;
    }

    public void Rotate(bool value)
    {
        if (!canAxisRoate) return;
        var val = value ? new Vector3(0, -90, 0) : Vector3.zero;
        CameraController.RotateCameraView(val);
        CheckRotation();
    }

    public void Rotate()
    {
        if (canAxisRoate)
        {
            curAxis = Axis.ZAxis;
            CameraController.RotateCameraView(new Vector3(0, -90, 0));
            transform.rotation = Quaternion.Euler(Vector3.zero);
            CheckRotation();
        }
    }

    public void KnockBack(Vector3 direction)
    {
        if (state == PlayerState.KnockBackState) return;
        ChangeState(PlayerState.KnockBackState);
        rigid.velocity = Vector3.zero;
        var look = direction.x < 0 ? 1 : -1;
        lookVector = new Vector2(look, 0);
        transform.localScale = new Vector3(2, 2, 2 * look);
        rigid.AddForce(direction, ForceMode.Impulse);        
    }

    private void PlayerInitialize()
    {
        curHP = maxHP;
        lookVector = Vector2.right;
        curState = stateDic[PlayerState.IdleState];
        ui.UpdateHP(curHP);
    }

    #endregion

    #region Check Func
    public void CheckGround()
    {
        isGround = Physics.BoxCast(collid.bounds.center, new Vector3(0.35f, 0.5f, 0.5f), -transform.up, Quaternion.identity, collid.height, groundLayer);             
    }

    public bool CheckAttack()
    {
        if (mode) return false;
        RaycastHit[] target = Physics.BoxCastAll(transform.position, new Vector3(3.5f, 1.5f, 1), lookVector, Quaternion.identity, 2.8f, wireLayer);

        //Collider[] target = Physics.OverlapBox(transform.position + new Vector3(lookVector.x * 1.5f, 0), new Vector3(3, 1.5f, 1), Quaternion.identity);

        if (target.Length == 0) return false;

        for (int i = 0; i < target.Length; i++)
        {
            ICutOff cut = target[i].collider.GetComponent<ICutOff>();

            if (cut != null)
            {
                if (cut.CheckCutOff()) return true;
            }
        }
        return false;
    }

    public bool CheckWall()
    {
        return !Physics.BoxCast(collid.bounds.center, new Vector3(0.2f, 1.1f, 0.5f), new Vector3(lookVector.x, 0), Quaternion.identity, collid.radius * 2, wallLayer);
    }

    public bool CheckMonster()
    {
        return Physics.BoxCast(collid.bounds.center, new Vector3(0.4f, 1.1f, 0.5f), new Vector3(lookVector.x, 0), Quaternion.identity, collid.radius * 2, monsterLayer);
    }

    public void CheckRotation()
    {
        if (walkVector == 0) return;
        var look = walkVector > 0 ? 1 : -1;
        //player.transform.rotation = Quaternion.Euler(0, 90 * look, 0);
        transform.localScale = new Vector3(2, 2, 2 * look);
        effectEuler.rotation = look == 1 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
        if (curAxis == Axis.XAxis) lookVector = new Vector3(look, 0);
        else lookVector = new Vector3(0, 0, look);
    }

    #endregion    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Application.isPlaying)
        {
            Gizmos.DrawWireCube(collid.bounds.center + new Vector3(0, -collid.height), new Vector3(0.7f,0.1f,1f));
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(collid.bounds.center + new Vector3(lookVector.x * collid.radius * 2, 0), new Vector3(0.8f,2.2f,1f));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(collid.bounds.center + drawPos, drawSize);
        }
    }

}
