using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VaribleInput : MonoBehaviour
{
    [Header("The value we got from the input field")]
    [SerializeField] private GameObject inputField;

    [Header("Showing the reaction to the player")]
    [SerializeField] private string outputText;

    private string _userInput="";
    private TemplateSend _sendTemplate;

    private void Awake()
    {
        _sendTemplate = new TemplateSend();
    }

    private void Start()
    {
        DataInit();
    }

    private void DataInit()
    {
        //just once, before conversation
        //item information
        string itemInfo = "@ObjID =2, @ObjName = Cup, @ObjectInfo = blah, @defaultPrice =10, @expensvie = 100, @tooExpensive =200";
        //npc information
        string npcInfo = "@NpcID = 1, @NpcSex = female, @NpcAge = 17, @NpcPersonality = Bad, @NpcProplemType = relate, @NpcProblemInfo = blah";

        _sendTemplate.Init(itemInfo, npcInfo);
    }

    public void OnClick()
    {
        _userInput = inputField.GetComponent<TMP_InputField>().text;
        VariableList.S_UserAnswer = _userInput;

        _sendTemplate.ChatwithGPT();
    }
}
