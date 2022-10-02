using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommandData", menuName ="DataContainer/CommandData")]
public class CommandContainer : ScriptableObject
{    
    public string command;
    [TextArea]
    [SerializeField] private string description;


}
