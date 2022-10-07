using UnityEngine;

public class HanselMove_State : FSM_State<Hansel>
{

    static readonly HanselMove_State instance = new HanselMove_State();
    public static HanselMove_State Instance
    {
        get { return instance; }
    }
    private Vector3 m_OldPosition;
    private Vector3 m_CurrentPosition;

    private float m_SpeedBoi = 0;

    static HanselMove_State() { }
    private HanselMove_State() { }


    public override void EnterState(Hansel _Hansel)
    {
        //타겟 확인(플레이어)
        if (_Hansel.myTarget == null)
        {
            return;
        }
        m_OldPosition = _Hansel.transform.position;
        m_SpeedBoi = 0;

    }

    public override void UpdateState(Hansel _Hansel)
    {
        //Dead Check
        if (_Hansel.CurrentHP <= 0)
        {
            _Hansel.ChangeState(HanselDie_State.Instance);
        }

        if (!_Hansel.CheckRange() && !_Hansel.isRushing && !_Hansel._isStuned && !_Hansel.isBelly &&
            !_Hansel.isSmash) 
        {
            //추적시간 넘으면 Tartget Lost
            _Hansel.ChaseTime += Time.deltaTime;
            if (_Hansel.ChaseTime >= _Hansel.ChaseCancleTime)
            {
                _Hansel.ChaseTime = 0.0f;
                return;
            }

            #region TrackingPlayer

            #region Lookat

            var m_lookatVec = (new Vector3(_Hansel.myTarget.transform.position.x, 0, 0)
                - new Vector3(_Hansel.transform.position.x, 0, 0)).normalized;

            _Hansel.transform.rotation = Quaternion.Lerp(_Hansel.transform.rotation,
                Quaternion.LookRotation(m_lookatVec), Time.fixedDeltaTime * _Hansel.Hansel_RotSpeed);

            #endregion

            #region Movement
            var m_Dir = new Vector3(_Hansel.myTarget.transform.position.x -_Hansel.transform.position.x, 0, 0).normalized;
            
            var m_fMaxSpeed = _Hansel.Hansel_Speed;
            var m_currentSpeed = m_fMaxSpeed;
            var Multiple = m_Dir * (m_currentSpeed) * Time.fixedDeltaTime;

            _Hansel.rb.MovePosition(_Hansel.transform.position + Multiple);
            #endregion

            #region velocity cul
            m_CurrentPosition = _Hansel.transform.position;
            var dis = m_CurrentPosition - m_OldPosition;
            var distance = Mathf.Sqrt(Mathf.Pow(dis.x , 2 ) + 0 + Mathf.Pow(dis.z , 2));
            var velocity = distance / Time.deltaTime;
            m_OldPosition = m_CurrentPosition;

            var m_Val = velocity / velocity;

            m_SpeedBoi = Mathf.SmoothDamp(m_SpeedBoi, 1.0f, ref m_Val, 0.1f , m_fMaxSpeed, Time.fixedDeltaTime);

            if (m_SpeedBoi > 1.0f)
            {
                m_SpeedBoi = 1.0f;
            }

            _Hansel.Ani.SetFloat("H_Walk", m_SpeedBoi);
            #endregion


            #endregion

        }
        else
        {
            m_SpeedBoi = 0;
            _Hansel.Ani.SetFloat("H_Walk", 0);
            _Hansel.ChangeState(SmashAttack_State.Instance);
        }

    }

    public override void ExitState(Hansel _Hansel)
    {
        //_Hansel.rb.velocity = Vector3.zero;

        _Hansel.Ani.SetFloat("H_Walk", 0);

        return;
    }

}
