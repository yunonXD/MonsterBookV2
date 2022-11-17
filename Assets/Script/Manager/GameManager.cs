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

public enum ConsoleType
{
    None,
    Player,
    System,
    Error,
}

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [Header("Prefab")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private UnityEngine.UI.Image blackImage;

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
        OutputConsole("GameManager Loaded", ConsoleType.System);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OutputConsole("Scene Loaded : " + SceneManager.GetActiveScene().buildIndex, ConsoleType.System);
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                break;
            case 1:
                FadeEffect(false, 3);
                break;
            case 2:
                FadeEffect(false, 4);
                break;
            case 3:
                FadeEffect(false, 3);
                break;
            default:
                break;
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        if (commandWindow.activeSelf) commandWindow.SetActive(false);

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        else if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Instantiate(playerPrefab, Vector3.zero, playerPrefab.transform.rotation);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        Screen.SetResolution(1920, 1080, true);

        inputCmd.onSubmit.AddListener(delegate { Command(); });

        for (int i = 0; i < command.Length; i++)
        {
            //commadList.Add(command[i].objectCommand, command[i].typeCommand, command[i].sendCommand);
            commandList.Add(command[i].objectCommand, command[i].typeCommand, command[i]);
        }
    }

    public static bool GetConsoleEnable() { return Instance.commandWindow.activeSelf; }

    public static Transform GetPlayerTransform() { return Instance.player.transform; }

    public static void SetInGameInput(bool value)
    {
        Instance.player.input.SetInGameInput(value);
    }

    #region Console Function
    public static void CallCommandWindow(bool value)
    {
       Instance.commandWindow.SetActive(value);
       Instance.inputCmd.text = "";       
       Instance.inputCmd.ActivateInputField();        
    }

    public void Command()
    {
        inputCmd.ActivateInputField();
        var cmd = inputCmd.text;

        if (cmd != "") OutputConsole(cmd, ConsoleType.Player);
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
                if (commandList[input[0], input[1]].type == global::CMDType.Prefab)
                {
                    Spawn(commandList[input[0], input[1]].prefab, int.Parse(input[2]));
                    OutputConsole("Spawned " + commandList[input[0], input[1]].description + input[2], ConsoleType.System);
                    return;
                }
                else if (commandList[input[0], input[1]].type == global::CMDType.Player)
                {
                    player.SendMessage(commandList[input[0], input[1]].sendCommand, input[2]);
                }
                else if (commandList[input[0], input[1]].type == global::CMDType.Scene)
                {
                    if (int.Parse(input[2]) >= 5)
                    {
                        OutputConsole("Out of scene count", ConsoleType.Error);
                        return;
                    }
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
                OutputConsole(commandList[input[0], input[1]].description + " " + input[2], ConsoleType.System);
                return;
            }
        }
        else if (input.Length == 2)
        {
            if (commandList.ContainsKey(input[0], input[1]))
            {
                if (commandList[input[0], input[1]].type == global::CMDType.Prefab)
                {
                    Spawn(commandList[input[0], input[1]].prefab);
                    OutputConsole("Spawned " + commandList[input[0], input[1]].description, ConsoleType.System);
                    return;
                }
                else if (commandList[input[0], input[1]].type == global::CMDType.Player)
                {
                    player.SendMessage(commandList[input[0], input[1]].sendCommand);
                }
                else
                {
                    SendMessage(commandList[input[0], input[1]].sendCommand);
                }
                OutputConsole(commandList[input[0], input[1]].description, ConsoleType.System);
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

        OutputConsole("UnKnown command", ConsoleType.Error);
    }

    private static void OutputConsole(string cmd, ConsoleType tr)
    {
        var result = Instance.consoleText.text;
        Instance.consoleText.text = null;
        switch (tr)
        {
            case ConsoleType.None:
                break;
            case ConsoleType.Player:
                Instance.consoleText.text += "<color=grey><Player>: " + cmd + "</color>\n" + result;
                break;
            case ConsoleType.System:
                Instance.consoleText.text += "<color=green><System>: " + cmd + "</color>\n" + result;
                break;
            case ConsoleType.Error:                
                Instance.consoleText.text += "<color=red><System>: " + cmd + "</color>\n" + result;
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
        var isDamage = int.Parse(value);
        player.isDamage = isDamage;
        OutputConsole("Player's isDamage is set to " + isDamage, ConsoleType.System);
    }

    private void SetPlayerInvin(string value)
    {
        var val = bool.Parse(value);
        player.invinBool = val;
        OutputConsole("Player's invincible is set to " + val, ConsoleType.System);
    }

    private void SetPlayerCurHP(string value)
    {
        var val = int.Parse(value);
        player.SetCurHP(val);
        OutputConsole("Player's current HP is set to " + val, ConsoleType.System);
    }

    private void SetPlayerMaxHP(string value)
    {
        var val = int.Parse(value);
        player.SetMaxHP(val);
        OutputConsole("Player's max HP is set to " + val, ConsoleType.System);
    }

    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void MoveStage(int i)
    {
        switch (i)
        {
            case 1:
                player.transform.position = new Vector3(488.5f, 35, 0);
                break;
        }
    }

    #endregion

    public static void FadeEffect(bool value, float speed)
    {
        Instance.StartCoroutine(Instance.FadeRoutine(value, speed));
        Debug.Log("DSDS");
    }

    private IEnumerator FadeRoutine(bool value, float speed)
    {
        var time = 0f;
        var start = value ? 0 : 1;
        var end = value ? 1 : 0;

        var color = blackImage.color = new Color32(0, 0, 0, (byte)(start * 255));

        while (color.a != end)
        {
            time += Time.deltaTime / speed;
            color.a = Mathf.Lerp(start, end, time);
            blackImage.color = color;
            yield return YieldInstructionCache.waitForFixedUpdate;
        }
    }
}
