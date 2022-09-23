
abstract public class FSM_State<t>
{
    abstract public void EnterState(t _Hansel);
    abstract public void UpdateState(t _Hansel);
    abstract public void ExitState(t _Hansel);
}
