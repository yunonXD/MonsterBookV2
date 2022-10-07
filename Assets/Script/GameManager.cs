using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Events;
using System.Reflection;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using System.Data.SqlTypes;

public class DualKeyDictionary<TKey1, TKey2, TValue> : Dictionary<TKey1, Dictionary<TKey2, TValue>>
{
    public TValue this[TKey1 key1, TKey2 key2]
    {
        get
        {
            if (!ContainsKey(key1) || !this[key1].ContainsKey(key2))
            {
                throw new ArgumentOutOfRangeException();
            }

            return base[key1][key2];
        }
        set
        {
            if (!ContainsKey(key1))
            {
                this[key1] = new Dictionary<TKey2, TValue>();
            }

            this[key1][key2] = value;
        }
    }

    public new IEnumerable<TValue> Values
    {
        get
        {
            return from baseDictionary in base.Values
                   from baseKey in baseDictionary.Keys
                   select baseDictionary[baseKey];
        }
    }

    public void Add(TKey1 key1, TKey2 key2, TValue value)
    {
        if (!ContainsKey(key1))
        {
            this[key1] = new Dictionary<TKey2, TValue>();
        }

        this[key1][key2] = value;
    }

    public bool ContainsKey(TKey1 key1, TKey2 key2)
    {
        return base.ContainsKey(key1) && this[key1].ContainsKey(key2);
    }
}

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

    [Header("Prefab")]
    [SerializeField] private GameObject playerPrefab;
    

    #region Console
    [Header("[Console Property]")]
    [SerializeField] private GameObject commandWindow;
    [SerializeField] private TMP_InputField inputCmd;
    [SerializeField] private TMP_Text consoleText;

    #endregion

    private PlayerController player;

    [SerializeField] private CommandContainer command;

    //private Dictionary<string, string> commandList = new Dictionary<string, string>();    
    private DualKeyDictionary<string, string, CommandData> commandList = new DualKeyDictionary<string, string, CommandData>();    

    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode move)
    {
        if (commandWindow.activeSelf) inputCmd.ActivateInputField();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this);

        if (commandWindow.activeSelf) commandWindow.SetActive(false);

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        else
        {
            Instantiate(playerPrefab, Vector3.zero, playerPrefab.transform.rotation);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
                

        inputCmd.onSubmit.AddListener(delegate { Command(); });

        for (int i = 0; i < command.Length; i++)
        {
            //commadList.Add(command[i].objectCommand, command[i].typeCommand, command[i].sendCommand);
            commandList.Add(command[i].objectCommand, command[i].typeCommand, command[i]);
        }
    }

    public static bool GetConsoleEnable() { return Instance.commandWindow.activeSelf; }

    #region Console Function
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
        string[] input = cmd.Split(" ");

        if (input.Length == 3)
        {
            if (commandList.ContainsKey(input[0], input[1]))
            {
                if (commandList[input[0], input[1]].type == CMDType.Prefab)
                {
                    Spawn(commandList[input[0], input[1]].prefab, int.Parse(input[2]));
                    UpdateResult("Spawned " + commandList[input[0], input[1]].description + input[2], Target.System);
                    return;
                }
                else if (commandList[input[0], input[1]].type == CMDType.Player)
                {
                    player.SendMessage(commandList[input[0], input[1]].sendCommand, input[2]);
                }
                else if (commandList[input[0], input[1]].type == CMDType.Scene)
                {
                    var val = 0;
                    if (int.TryParse(input[2], out val))
                    {
                        SendMessage(commandList[input[0], input[1]].sendCommand, int.Parse(input[2]));
                    }
                    else SendMessage(commandList[input[0], input[1]].sendCommand, input[2]);
                }
                else
                {
                    SendMessage(commandList[input[0], input[1]].sendCommand, input[2]);
                }
                UpdateResult(commandList[input[0], input[1]].description + " " + input[2], Target.System);
                return;
            }
        }
        else if (input.Length == 2)
        {
            if (commandList.ContainsKey(input[0], input[1]))
            {
                if (commandList[input[0], input[1]].type == CMDType.Prefab)
                {
                    Spawn(commandList[input[0], input[1]].prefab);
                    UpdateResult("Spawned "+commandList[input[0], input[1]].description, Target.System);
                    return;
                }
                else if (commandList[input[0], input[1]].type == CMDType.Player)
                {
                    player.SendMessage(commandList[input[0], input[1]].sendCommand);
                }
                else
                {
                    SendMessage(commandList[input[0], input[1]].sendCommand);
                }
                UpdateResult(commandList[input[0], input[1]].description, Target.System);
                return;
            }
        }

        #region Old
        //if (command.)

        //if (input.Length == 3)
        //{
        //    if (commadList.ContainsKey(input[0], input[1]))
        //    {
        //        if (input[0] == "player") player.SendMessage(input[1], input[2]);
        //        //else if (input[0] == "spawn") Send
        //    }
        //}
        //else if (input.Length == 2)
        //{
        //    if (commadList.ContainsKey(input[0], input[1]))
        //    {
        //        SendMessage(commadList[input[0], input[1]]);
        //        return;
        //    }
        //}
        #endregion'

        UpdateResult("UnKnown command", Target.Error);
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

    #region Command

    private void Spawn(GameObject prefab, int count = 1)
    {
        var pos = player.transform.position + Vector3.right * 6;
        for (int i = 0; i < count; i++)
        {
            Instantiate(prefab, pos, prefab.transform.rotation);
            pos += Vector3.right * 3;
        }
    }

    private void SetPlayerStr(string value)
    {
        var str = int.Parse(value);
        player.str = str;
        UpdateResult("Player's str is set to " + str, Target.System);
    }

    private void SetPlayerInvin(string value)
    {
        var val = bool.Parse(value);
        player.invinBool = val;
        UpdateResult("Player's invincible is set to " + val, Target.System);
    }

    private void SetPlayerCurHP(string value)
    {
        var val = int.Parse(value);
        player.SetCurHP(val);
        UpdateResult("Player's current HP is set to " + val, Target.System);
    }

    private void SetPlayerMaxHP(string value)
    {
        var val = int.Parse(value);
        player.SetMaxHP(val);
        UpdateResult("Player's max HP is set to " + val, Target.System);
    }

    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    #endregion

}
