
using UnityEngine;

public class Anna_Phase1 : FSM_State<Anna>
{
    static readonly Anna_Phase1 instance = new Anna_Phase1();

    public static Anna_Phase1 Instance
    {
        get { return instance; }
    }

    static Anna_Phase1() { }
    private Anna_Phase1() { }


    public override void EnterState(Anna _Anna)
    {
        //타겟 확인(플레이어)
        if (_Anna.Anna_Player == null)
        {
            return;
        }





    }

    public override void UpdateState(Anna _Anna)
    {

    }

    public override void ExitState(Anna _Anna)
    {

    }





}
