
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
        //���� ���º�ȯ -> ����
        if(_NewState == m_CurrentState)
        {
            return;
        }

        m_PreviousState = m_CurrentState;

        //���� ���°� �̹� ������ ����
        if (m_CurrentState != null)
        {
            m_CurrentState.ExitState(Owner);
        }
        m_CurrentState = _NewState;
        
        //New ����� ���°� null �� �ƴϴ� -> ����
        if (m_CurrentState != null)
        {
            m_CurrentState.EnterState(Owner);
        }
    }

    //�ʱⰪ
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

    //��������
    public void StartRevert()
    {
        if (m_PreviousState != null)
        {
            ChangeState(m_PreviousState);
        }
    }

}
