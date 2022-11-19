using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPausePopup : UIBasePopup
{
    [System.Serializable]
    public enum State
    {
        Open,
        Resume,
        Setting,
        Exit,
        Close,
    }

    [SerializeField, ReadOnly] private State curState;
    [SerializeField, ReadOnly]
    private int selectMenuIndex;
    [SerializeField]
    private State[] menuStates;

    [SerializeField]
    private GameObject[] menuHighlights;

    bool isMove = false;

    private PlayerAction action;

    public override void Init(UIData uiData)
    {
        action = new PlayerAction();

        action.UI.Move.performed += val => Move(val.ReadValue<Vector2>());
        action.UI.Move.canceled += val => EndMove();
        action.UI.Select.started += val => Select();

        action.UI.Enable();
    }

    private void Move(Vector2 dir)
    {
        if (isMove)
            return;

        isMove = true;

        menuHighlights[selectMenuIndex].SetActive(false);

        selectMenuIndex += (int)dir.y;

        if (selectMenuIndex < 0)
        {
            selectMenuIndex = 0;
        }
        else if (selectMenuIndex >= menuStates.Length)
        {
            selectMenuIndex = menuStates.Length - 1;
        }

        curState = menuStates[selectMenuIndex];
        menuHighlights[selectMenuIndex].SetActive(true);
    }

    private void EndMove()
    {
        isMove = false;
    }

    private void Select()
    {
        switch (curState)
        {
            case State.Open:
                break;
            case State.Resume:
                //TODO :: 일시정지 풀리게 하면 되요.
                Close();
                break;
            case State.Setting:
                //TODO :: 세팅 열기
                Game.UI.UIController.Instance.OpenPopup(new UISettingPopupData(){ endCloseAction = ()=>{ gameObject.SetActive(true); } });
                gameObject.SetActive(false);
                break;
            case State.Exit:
                SceneManager.LoadScene(0);
                break;
            case State.Close:
                break;
        }
    }

    public override void BeginClose()
    {
        action.Disable();
        base.BeginClose();
    }

}
