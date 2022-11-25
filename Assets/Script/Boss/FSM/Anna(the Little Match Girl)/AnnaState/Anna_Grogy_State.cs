using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anna_Grogy_State : FSM_State<Anna>
{
    private bool onetime = false;
    private float time;
    private bool OneTimeSoundPlay = false;
    static readonly Anna_Grogy_State instance = new Anna_Grogy_State();
    public static Anna_Grogy_State Instance
    {
        get { return instance; }
    }

    public override void EnterState(Anna _Anna)
    {
        _Anna.GroundLanding = false;
        OneTimeSoundPlay = false;
        _Anna.Anna_Ani.SetTrigger("Groggy_Start");
        time = 1;
        onetime = false;
    }

    public override void ExitState(Anna _Anna)
    {
        _Anna.AnnaSoundLoopEnd("2StageAnna_Groggy");
    }

    public override void UpdateState(Anna _Anna)
    {
        time -= Time.deltaTime;
        if(_Anna.AnnaFalling)
        {
            if(OneTimeSoundPlay == false)
            {
                _Anna.AnnaSoundLoop("2StageAnna_GroggyScream");
                Debug.Log("家府瘤福扁风橇 积己");
                OneTimeSoundPlay = true;
            }
            _Anna.transform.position = new Vector3(_Anna.transform.position.x, _Anna.transform.position.y - _Anna.DownSpeed, _Anna.transform.position.z);
            _Anna.Halo.GetComponent<Renderer>().sharedMaterial.SetFloat("_Mask_Dissolve_Control", time);
        }

        if(_Anna.finishAttackAble == true)
        {

        }


        if(_Anna.GroundLanding == true)
        {
            if(onetime == false)
            {
                _Anna.Anna_Ani.SetTrigger("Landing_Ground");
                _Anna.AnnaSoundLoopEnd("2StageAnna_GroggyScream");
                _Anna.AnnaSoundLoop("2StageAnna_Groggy");
                _Anna.AnnaSoundLoopEnd("2StageAnna_Groggy");
                _Anna.finishAttackAble = true;
                _Anna.AnnaFalling = false;
                onetime = true;
            }
            else
            {

            }
            
  
        }

    }





}
