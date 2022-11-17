using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TalkSimulator : MonoBehaviour
{
    [SerializeField]
    private ScenarioData scenarioData;

    [SerializeField]
    private int currentIndex = 0;

    public float textDisplayTimePerCharacter = 0.1f;

    [SerializeField]
    private TalkElementData currentTalkData;

    [SerializeField]
    private UITalkCharacter currentTalkCharacter;

    [SerializeField]
    private UITalkCharacter[] talkCharacterSlots;

    [ReadOnly]
    private SerializableDictionary<string, UITalkCharacter> cachingCharacterSlotDic = new SerializableDictionary<string, UITalkCharacter>();

    [SerializeField]
    private UIBaseText characterNameText;
    [SerializeField]
    private UIBaseText talkText;

    public UnityEvent startScenarioEvnet;
    public UnityEvent endScenarioEvnet;

    public UnityEvent nextTalkEvent;
    public UnityEvent startTalkEvent;
    public UnityEvent endTalkEvent;

    private Coroutine textDisplayAnimation;

    [SerializeField]
    private bool isAllowNextTalk = false;


    //TEST��==

    private void Start()
    {
        StartScenario();
    }


    private void Update()
    {
        if (isAllowNextTalk && Input.GetKeyDown(KeyCode.Return))
        {
            NextTalk();
        }
    }
    //=======

    public void StartScenario()
    {
        //NextTalk���� ++�ϰ� �����ϱ� ������, -1���� ���� ���ּ���.
        currentIndex = -1;
        startScenarioEvnet?.Invoke();
        NextTalk();
    }

    public void EndScenario()
    {
        endScenarioEvnet?.Invoke();
    }

    public void NextTalk()
    {
        nextTalkEvent?.Invoke();

        // ����, �ܿ� ��ȭ�� �����ϸ�, �ڵ����� ���� ��ȭ�� �����մϴ�.
        if (currentIndex < scenarioData.TalkElementDataList.Count - 1)
        {
            ++currentIndex;
            currentTalkData = scenarioData.GetTalkElementData(currentIndex);

            StartTalk();
        }
        else
        {
            EndScenario();
        }
    }

    public void StartTalk()
    {
        startTalkEvent?.Invoke();

        if (textDisplayAnimation != null)
        {
            StopCoroutine(textDisplayAnimation);
        }

        AddCharacter(currentTalkData);
        characterNameText.SetText(currentTalkData.CharacterName);

        //��ȭ �ϴ� ģ����, ȭ��Ʈ, 1���
        //��ȭ ���ϰ� ������ ���� ƾƮ, ũ�� 0.9���
        AutoHighlight(currentTalkData.CharacterName);

        textDisplayAnimation = StartCoroutine(CoTextDisplayAnimation());
    }

    public void EndTalk()
    {
        isAllowNextTalk = true;
        endTalkEvent?.Invoke();
    }

    //������ư ������, Ÿ�ڱ� ���� ��ŵ�ϰ� �� �߰� �����
    public void SkipTextDisplayAnimation()
    {
        if (textDisplayAnimation != null)
        {
            StopCoroutine(textDisplayAnimation);
        }

        talkText.SetText(currentTalkData.Talk);
    }

    //���丮 ��ŵ ���
    public void SkipScenario()
    {
        EndScenario();
    }

    public bool ContainCharacter(string characterName)
    {
        return cachingCharacterSlotDic.ContainsKey(characterName);
    }

    private void AddCharacter(TalkElementData talkData)
    {
        UITalkCharacter slot = null;

        switch (talkData.CharacterStandAnchor)
        {
            case TalkElementData.CharacterStandAnchorType.Left:
                slot = talkCharacterSlots[0];
                break;
            case TalkElementData.CharacterStandAnchorType.Right:
                slot = talkCharacterSlots[1];
                break;
        }

        if (cachingCharacterSlotDic.ContainsKey(talkData.CharacterName))
        {
            cachingCharacterSlotDic[talkData.CharacterName] = slot;
        }
        else
        {
            cachingCharacterSlotDic.Add(talkData.CharacterName, slot);
        }

        slot.ChangeCharacter(talkData.CharacterName, talkData.Character);
        slot.gameObject.SetActive(true);

    }

    private void RemoveCharacter(string characterName)
    {
        if (cachingCharacterSlotDic.ContainsKey(characterName))
        {
            var slot = cachingCharacterSlotDic[characterName];
            slot.gameObject.SetActive(false);

            cachingCharacterSlotDic.Remove(characterName);
        }
    }

    public void AutoHighlight(string name)
    {
        for (var i = 0; i < talkCharacterSlots.Length; ++i)
        {
            if (talkCharacterSlots[i].GetCharacterName().Equals(name) && !talkCharacterSlots[i].IsShow)
            {
                talkCharacterSlots[i].Show();
            }
            else
            {
                talkCharacterSlots[i].Hide();
            }
        }
    }


    //�ؽ�Ʈ�� Ÿ�ڱ� ���⿡ �ӵ� n��
    IEnumerator CoTextDisplayAnimation()
    {
        var waitForTime = new WaitForSeconds(textDisplayTimePerCharacter);
        var currentText = "";
        var currentTextIndex = 0;

        StringBuilder stBuilder = new StringBuilder();

        while (currentTalkData.Talk.Length != currentTextIndex)
        {
            stBuilder.Append(currentTalkData.Talk[currentTextIndex]);
            ++currentTextIndex;

            talkText.SetText(stBuilder.ToString());
            yield return waitForTime;
        }

        EndTalk();
    }


}
