
public class RushAttack_State : FSM_State<Hansel>
{
    static readonly RushAttack_State instance = new RushAttack_State();

    public static RushAttack_State Instance
    {
        get
        {
            return instance;
        }
    }

    private float lastLosWaitTime = 0;

    static RushAttack_State() { }
    private RushAttack_State() { }

    public override void EnterState(Hansel _Hansel)
    {
        //타겟 확인(플레이어)
        if (_Hansel.myTarget == null)
        {
            return;
        }


        #region Damage Collider
        int m_Damage = _Hansel.Hansel_RushDamage;
        _Hansel.RushCollider.GetComponent<RushCol>().g_Player_To_Damgage = m_Damage;
        _Hansel.RushCollider.GetComponent<RushCol>().g_Transform = _Hansel.transform;
        #endregion

        _Hansel.Isinvincibility = true;
        _Hansel.CapCol_Hansel.isTrigger = true;
        _Hansel.RushCollider.SetActive(true);
        _Hansel.OnDircalculator(1);
        _Hansel.Ani.SetTrigger("H_RushAttackR");
        _Hansel.isRushing = true;

    }

    public override void UpdateState(Hansel _Hansel)
    {
        //Dead Check
        if (_Hansel.CurrentHP <= 0)
        {
            _Hansel.ChangeState(HanselDie_State.Instance);
        }

        if (_Hansel.myTarget && !_Hansel.Col_with_Wall && _Hansel.isRushing)
        {

            _Hansel.Ani.SetBool("H_RushAttack_Loop", true);
            _Hansel.rb.AddForce(_Hansel.transform.forward * _Hansel.Rush_Speed, UnityEngine.ForceMode.Acceleration);
        }
        else
        {
            _Hansel.Ani.ResetTrigger("H_RushAttack_Loop");
            _Hansel.Ani.SetBool("H_RushAttack_Loop", false);
            _Hansel.Isinvincibility = false;

            _Hansel.RushCollider.SetActive(false);
            _Hansel.rb.velocity = UnityEngine.Vector3.zero;

            lastLosWaitTime += UnityEngine.Time.deltaTime;
            if (lastLosWaitTime >= _Hansel.Rush_EndWait)
            {
                _Hansel.rb.velocity = UnityEngine.Vector3.zero;
                lastLosWaitTime = 0;
                _Hansel.ChaseTime = 0.0f;
                _Hansel.CapCol_Hansel.isTrigger = false;
                _Hansel.ChangeState(HanselMove_State.Instance);
 
            }
        }
    }

    public override void ExitState(Hansel _Hansel)
    {
        _Hansel.Isinvincibility = false;
        _Hansel.CapCol_Hansel.isTrigger = false;
        _Hansel.Ani.ResetTrigger("H_RushAttack_Loop");
        _Hansel.isRushing = false;
        return;
    }
}