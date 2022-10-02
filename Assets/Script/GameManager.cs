using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    private enum Target
    {
        None,
        Player,
        System,
        Error,
    }

    public static GameManager Instance;
    
    #region Console
    [SerializeField] private GameObject commandWindow;
    [SerializeField] private TMP_InputField inputCmd;
    [SerializeField] private TMP_Text consoleText;

    #endregion

    private PlayerController player;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        if (commandWindow.activeSelf) commandWindow.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        inputCmd.onSubmit.AddListener(delegate { Command(); });
    }

    public static bool GetConsoleEnable() { return Instance.commandWindow.activeSelf; }

    #region Command
    public static void CallCommandWindow(bool value)
    {
       Instance.commandWindow.SetActive(value);
       Instance.inputCmd.text = "";
       Instance.consoleText.text = "";
       Instance.inputCmd.ActivateInputField();
    }

    public void Command()
    {
        inputCmd.ActivateInputField();
        var cmd = inputCmd.text;

        if (cmd != "") UpdateResult(cmd, Target.Player);
        else return;
        inputCmd.text = null;

        InputCommand(cmd);
    }

    private void InputCommand(string cmd)
    {
        if (SpaceCompare(cmd, "player"))
        {
            cmd = Space(cmd);

            if (SpaceCompare(cmd, "item"))
            {
                cmd = Space(cmd);
                if (cmd == "" || cmd.Contains(" "))
                {
                    UpdateResult("UnKnown item", Target.Error);
                    return;
                }
                var id = int.Parse(cmd);

                //if (itemData.ContainsKey(id))
                //{
                //    if (!player.memory.memoryInventory.Contains(id))
                //    {
                //        player.memory.GetItem(id);
                //        UpdateResult("You get item [" + itemData[id].itemName + "]", Target.System);
                //        return;
                //    }
                //    else UpdateResult("Already you have", Target.Error);
                //}
                //else UpdateResult("UnKnown item", Target.Error); return;
            }
            else UpdateResult("UnKnown command", Target.Error); return;
        }
        else
        {
            UpdateResult("UnKnown command", Target.Error);
        }
    }

    private void UpdateResult(string cmd, Target tr)
    {
        var result = consoleText.text;
        consoleText.text = null;
        switch (tr)
        {
            case Target.None:
                break;
            case Target.Player:
                consoleText.text += "<color=grey><Player>: " + cmd + "</color>\n" + result;
                break;
            case Target.System:
                consoleText.text += "<color=green><System>: " + cmd + "</color>\n" + result;
                break;
            case Target.Error:
                consoleText.text += "<color=red><System>: " + cmd + "</color>\n" + result;
                break;
        }
    }

    private bool SpaceCompare(string cmd, string target)
    {
        if (!cmd.Contains(" ")) return false;
        return target == cmd.Substring(0, cmd.IndexOf(" "));
    }

    private string Space(string va)
    {
        return va.Substring(va.IndexOf(" ") + 1);
    }

    #endregion
}
