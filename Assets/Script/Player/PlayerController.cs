using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

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
}

[Serializable]
public class StringParticle : SerializableDictionary<String, ParticleSystem> { }

public class SomeComparer : IEqualityComparer<PlayerState>
{
    bool IEqualityComparer<PlayerState>.Equals(PlayerState x, PlayerState y) { return (int)x == (int)y; }
    int IEqualityComparer<PlayerState>.GetHashCode(PlayerState obj) { return ((int)obj).GetHashCode(); }
}

public class PlayerController : MonoBehaviour, IEntity
{
    #region Component
    [HideInInspector] public Rigidbody rigid;
    [HideInInspector] public Animator ani;
    public CapsuleCollider collid { get; private set; }
    public InputManager input { get; private set; }
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
    [SerializeField] private int maxHP;
    [ReadOnly][SerializeField] private int curHP;
    [SerializeField] private float invinTime;
    public bool invinBool { get; set; }
    public int str;

    #endregion

    #region PlayerMove
    [Header("[Player Move]")]
    public float walkSpeed = 4;
    public float jumpForce = 12;   
    public float dashForce = 20;
    public float wireForce;

    public float walkVector { get; set; }

    #endregion

    #region Move Value
    [Header("[Move Value]")]
    [ReadOnly] public bool isGround;
    public LayerMask wallLayer;
    public LayerMask groundLayer;
    public LayerMask wireLayer;
    public Vector2 lookVector { get; set; }

    public WireObject wireObject { get; set; }
    public Vector3 wireTarget { get; set; }    

    #endregion

    #region Inspector
    [Header("[Component Property]")]
    public Weapon[] attackWeapon;    
    public Transform wireStart;    
    [SerializeField] private float gameSpeed = 1;
    public LineRenderer line;

    [Header("[Effect Prefab]")]
    [SerializeField] private StringParticle particle;

    [Header("[Editor]")]
    [SerializeField] private Vector3 drawPos;
    [SerializeField] private Vector3 drawSize;

    #endregion

    #region Initialize
    private void Awake()
    {
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

    private void AttackBoxOff(int i)
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

    #endregion

    public void ChangeState(PlayerState next)
    {
        if (!stateDic.ContainsKey(next) || !curState.CheckState(next)) return;        
        prevState = curState;
        prevState.OnStateExit(this);
        stateDic[next].OnStateEnter(this);
        curState = stateDic[next];
        //Debug.Log("ChangeState " + state.ToString() + " -> " + next.ToString());
    }
   
    private void Update()
    {
        Time.timeScale = gameSpeed;
        CheckGround();
        if (walkVector != 0) ChangeState(PlayerState.WalkState);
        curState.OnStateExcute(this);
    }

    public void SetWalkVector(float value)
    {
        walkVector = value > 0 && value != 0 ? 1 : -1;
    }

    #region Getter
    public PlayerState GetCurState() { return state; }

    public IState GetPrevState() { return prevState; }

    public IState GetStateDictionary(PlayerState state) { return stateDic[state]; }

    #endregion

    #region Entity
    public void OnDamage(int damage, Vector3 pos)
    {
        if (invinBool) return;
        transform.LookAt(new Vector3(pos.x, transform.position.y));
        lookVector = transform.eulerAngles.y == 90 ? Vector3.right : Vector3.left;
        ChangeState(PlayerState.HitState);
        curHP -= damage;
        if (curHP <= 0) curHP = 0;
        ui.UpdateHP(curHP);
    }

    public void OnRecovery(int heal)
    {

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
        isGround = Physics.BoxCast(collid.bounds.center, new Vector3(0.2f, 0.03f, 0.5f), -transform.up, Quaternion.identity, collid.height, groundLayer);             
    }

    public bool CheckAttack()
    {
        Collider[] target = Physics.OverlapBox(transform.position + new Vector3(lookVector.x * 1.5f, 0), new Vector3(3, 1.5f, 1), Quaternion.identity);

        if (target.Length == 0) return false;

        for (int i = 0; i < target.Length; i++)
        {
            ICutOff cut = target[i].GetComponent<ICutOff>();

            if (cut != null)
            {
                if (cut.CheckCutOff()) return true;
            }
        }
        return false;
    }

    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Application.isPlaying)
        {
            Gizmos.DrawWireCube(collid.bounds.center + new Vector3(0, -collid.height), new Vector3(0.4f,0.05f,1f));
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(collid.bounds.center + new Vector3(lookVector.x * collid.radius * 2, 0), new Vector3(0.1f,2.4f,1f));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(collid.bounds.center + drawPos, drawSize);
        }
    }

}
