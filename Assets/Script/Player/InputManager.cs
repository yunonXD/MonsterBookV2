using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class InputManager : MonoBehaviour
{
    private PlayerAction action;
    private PlayerController player;

    [SerializeField] private float walkVector;
    [SerializeField] private float prevInputTime;
    
    private float prevCurTime;
    private bool prevInputAttack;


    private void Awake()
    {
        action = new PlayerAction();
        player = GetComponent<PlayerController>();
        
        action.InGame.Walk.performed += val => player.SetWalkVector(val.ReadValue<float>());
        action.InGame.Walk.performed += val => walkVector = val.ReadValue<float>();
        action.InGame.Walk.canceled += val => EndWalk();

        action.InGame.Jump.started += val => player.ChangeState(PlayerState.JumpState);
        action.InGame.Attack.started += val => Attack();
        action.InGame.Dash.started += val => player.ChangeState(PlayerState.DashState);

        action.InGame.Rotate.started += val => player.Rotate();

        action.InGame.SpecialAttack.started += val => player.ChangeState(PlayerState.WireSearchState);
        action.InGame.SpecialAttack.canceled += val => Wire();

        action.CMD.Console.started += val => Console();

        if (GameManager.GetConsoleEnable())
        {
            action.InGame.Disable();
        }
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    private void Console()
    {
        var value = !GameManager.GetConsoleEnable();
        GameManager.CallCommandWindow(value);
        if (value) action.InGame.Disable();
        else action.InGame.Enable();        
    }

    public void SetInputAction(bool value)
    {
        if (value) action.Enable();
        else action.Disable();
    }

    private void EndWalk()
    {
        player.walkVector = 0;
        walkVector = 0;
    }

    private void Attack()
    {
        if (player.CheckAttack()) player.ChangeState(PlayerState.CuttingState);
        else player.ChangeState(PlayerState.AttackState);
        if (!player.prevInput) StartCoroutine(InputAttackRoutine());
        else prevCurTime = 0;
    }

    private void Wire()
    {        
        if (player.wirePos != null) player.ChangeState(PlayerState.WireThrowState);
        else player.ChangeState(PlayerState.IdleState);        
    }

    private IEnumerator InputAttackRoutine()
    {        
        player.prevInput = true;
        prevCurTime = 0f;
        while (player.prevInput)
        {
            prevCurTime += Time.deltaTime;            

            if (prevCurTime > prevInputTime)
            {
                player.prevInput = false;
            }
            yield return YieldInstructionCache.waitForFixedUpdate;
        }                
    }

}
