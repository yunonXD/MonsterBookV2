using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOverPopup : UIBasePopup
{
    [System.Serializable]
    public enum State
    {
        Open,
        Retry,
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

    //�˾� �����ϴ� ��
    /*
    Game.UI.UIController.Instance.OpenPopup(new UIGameOverPopupData()                
                {

                });
    */

    public override void Init(UIData uiData)
    {
        action = new PlayerAction();

        action.UI.Move.performed += val => Move(val.ReadValue<Vector2>());
        action.UI.Move.canceled += val => EndMove();
        action.UI.Select.started += val => Select();

        action.UI.Enable();

        Move(Vector2.zero);
        isMove = false;
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
            case State.Retry:
                OnRetry();
                break;
            case State.Exit:
                OnExit();
                break;
            case State.Close:
                break;
        }
    }

    public void OnRetry()
    {
        //TODO :: Go to Retry
        Debug.LogError("UIGameOverPopup :: ��Ʈ���� �ǵ��� ���� ���ּ���.");
    }

    public void OnExit()
    {
        //TODO :: Move to Title
        Debug.LogError("UIGameOverPopup :: Ÿ��Ʋ�� ������ ���� ���ּ���.");
    }

}
