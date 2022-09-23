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

    }


    public override void UpdateState(Hansel _Hansel)
    {

        //Dead Check
        if (_Hansel.CurrentHP <= 0)
        {
            _Hansel.ChangeState(HanselDie_State.Instance);
        }

        //Target Check 
        if (_Hansel.myTarget != null)
        {
            if (!_Hansel.CheckRange() && _Hansel.isRushing != true)
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

                //Vector3 Pos = _Hansel.transform.position;
                //Pos += _Hansel.transform.forward * Time.deltaTime * _Hansel.MoveSpeed;
                //_Hansel.transform.position = Pos;
                #endregion

                #region TrackingPlayer

                Vector3 Dir = new Vector3(_Hansel.myTarget.transform.position.x, _Hansel.transform.position.y, _Hansel.myTarget.transform.position.z);

                //Rotation
                _Hansel.transform.LookAt(Dir);


                //Movement
                Vector3 Pos = _Hansel.transform.position;

                Pos += _Hansel.transform.forward * Time.deltaTime * _Hansel.MoveSpeed;
                _Hansel.Ani.SetFloat("H_Walk", 1);
                _Hansel.transform.position = Pos;


                #endregion

            }
            else
            {
                _Hansel.ChangeState(SmashAttack_State.Instance);
            }

        }
        else //there is no target 
        {
            return;
            //Player cant find :/
        }
    }

    public override void ExitState(Hansel _Hansel)
    {
#if DEBUG
        //Debug.Log("Move_State 종료.");
#endif
        _Hansel.Ani.SetFloat("H_Walk", 0);

    }

}
