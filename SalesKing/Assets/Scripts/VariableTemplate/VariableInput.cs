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

    public void OnClick()
    {
        _userInput = inputField.GetComponent<TMP_InputField>().text;
        //TODO : 현재 단계가 chatSaleState나, ChatBargain 단계 아니면 작성하지 못하게 하기.
        VariableList.S_UserAnswer = _userInput;
    }
}
