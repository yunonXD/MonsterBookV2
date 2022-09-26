using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HanselMove_State : FSM_State<Hansel>
{

    static readonly HanselMove_State instance = new HanselMove_State();
    public static HanselMove_State Instance
    {
        get { return instance; }
    }
    
    static HanselMove_State() { }
    private HanselMove_State() { }


    public override void EnterState(Hansel _Hansel)
    {
        //타겟 확인(플레이어)
        if (_Hansel.myTarget == null)
        {
            return;
        }
    }


    public override void UpdateState(Hansel _Hansel)
    {

        //Dead Check
        if (_Hansel.CurrentHP <= 0)
        {
            _Hansel.ChangeState(HanselDie_State.Instance);
        }

        if (!_Hansel.CheckRange() && _Hansel.isRushing == false && _Hansel._isStuned == false && _Hansel.isBelly == false)
        {
            //추적시간 넘으면 Tartget Lost
            _Hansel.ChaseTime += Time.deltaTime;
            if (_Hansel.ChaseTime >= _Hansel.ChaseCancleTime)
            {
                //_Hansel.ChangeState(Attack_State.Instance);
                //_Hansel.myTarget = null;
                _Hansel.ChaseTime = 0.0f;
                return;
            }

            #region Nouse

            //Rotation Angle culcurate
            //Vector3 Dir = _Hansel.myTarget.transform.position - _Hansel.transform.position;
            //Vector3 NorDir = Dir.normalized;
            //Quaternion angle = Quaternion.LookRotation(NorDir);

            //_Hansel.transform.rotation = angle;

            //Movement
            //Vector3 Pos = _Hansel.transform.position;

            //Pos += _Hansel.transform.forward * Time.deltaTime * _Hansel.MoveSpeed;
            //_Hansel.transform.position = Pos;

            #endregion

            #region TrackingPlayer

            Vector3 Dir = new Vector3(_Hansel.myTarget.transform.position.x, _Hansel.transform.position.y, _Hansel.myTarget.transform.position.z);
            _Hansel.transform.LookAt(Dir);

            _Hansel.transform.position = Vector3.Lerp(_Hansel.transform.position, Dir, Time.deltaTime);

            _Hansel.Ani.SetFloat("H_Walk", 1);

            #endregion

        }
        else
        {
            _Hansel.Ani.SetFloat("H_Walk", 0);
            _Hansel.ChangeState(SmashAttack_State.Instance);
        }

    }

    public override void ExitState(Hansel _Hansel)
    {

        _Hansel.Ani.SetFloat("H_Walk", 0);
#if DEBUG
        //Debug.Log("Move_State 종료.");
#endif
        return;
    }

}
