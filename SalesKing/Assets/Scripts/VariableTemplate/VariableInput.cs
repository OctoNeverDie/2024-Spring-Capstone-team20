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
        _sendTemplate.Init(); //just once, before conversation
    }

    public void OnClick()
    {
        _userInput = inputField.GetComponent<TMP_InputField>().text;
        VariableList.S_UserAnswer = _userInput;

        _sendTemplate.SendToGPT();
    }
}
