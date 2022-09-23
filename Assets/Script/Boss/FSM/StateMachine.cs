
public class StateMachine <t>
{
    private t Owner;
    private FSM_State<t> m_CurrentState;
    private FSM_State<t> m_PreviousState;

    public void Awake()
    {
        m_CurrentState = null;
        m_PreviousState = null;
    }

    public void ChangeState(FSM_State<t> _NewState)
    {
        //같은 상태변환 -> 리턴
        if(_NewState == m_CurrentState)
        {
            return;
        }

        m_PreviousState = m_CurrentState;

        //현재 상태가 이미 있으면 종료
        if (m_CurrentState != null)
        {
            m_CurrentState.ExitState(Owner);
        }
        m_CurrentState = _NewState;
        
        //New 적용된 상태가 null 이 아니다 -> 실행
        if (m_CurrentState != null)
        {
            m_CurrentState.EnterState(Owner);
        }
    }

    //초기값
    public void Initial_Setting(t _Owner, FSM_State<t> _InitialState)
    {
        Owner = _Owner;
        ChangeState(_InitialState);
    }

    public void Update()
    {
        if (m_CurrentState != null)
        {
            m_CurrentState.UpdateState(Owner);
        }
    }

    //이전상태
    public void StartRevert()
    {
        if (m_PreviousState != null)
        {
            ChangeState(m_PreviousState);
        }
    }

}
