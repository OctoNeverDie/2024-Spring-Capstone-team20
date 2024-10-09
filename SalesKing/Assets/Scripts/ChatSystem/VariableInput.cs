using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VaribleInput : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    private string _userInput="";

    public void OnClick()
    {
        _userInput = inputField.text;
        TutorialManager.Instance.OnPersuadeToCustomer();
        //TODO : 현재 단계가 chatSaleState나, ChatBargain 단계 아니면 작성하지 못하게 하기.
        Managers.Chat.ReplyManager.UserAnswer = _userInput;
        inputField.text = "";

    }
}
