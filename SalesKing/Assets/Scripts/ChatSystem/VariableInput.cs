using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VariableInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField KeyboardText;
    [SerializeField] private TMP_InputField STTText;
    private string _userInput="";

    public void OnClick()
    {
        string userText = "";

        if (Managers.Input.CurInputMode == Define.UserInputMode.Keyboard)
        {
            userText = KeyboardText.text;
            KeyboardText.text = "";
        }
        else if (Managers.Input.CurInputMode == Define.UserInputMode.Voice)
        {
            userText = STTText.text;
            STTText.text = "";
        }

        if(userText == null)
        {
            Debug.Log("No inputfield");
            return;
        }

        _userInput = userText;
        
        TutorialManager.Instance.OnPersuadeToCustomer();
        //TODO : 현재 단계가 chatSaleState나, ChatBargain 단계 아니면 작성하지 못하게 하기.
        Managers.Chat.ReplyManager.UserAnswer = _userInput;

        if(Managers.Input.CurInputMode == Define.UserInputMode.Voice)
        {
            STTUI _sttUI = FindObjectOfType<STTUI>().GetComponent<STTUI>();
            _sttUI.OnClickEnter();
        }

    }
}
