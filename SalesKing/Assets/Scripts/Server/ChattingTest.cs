using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChattingTest : MonoBehaviour
{
    [Header("The value we got from the input field")]
    [SerializeField] private GameObject inputField;

    [Header("Showing the reaction to the player")]
    [SerializeField] private string outputText;

    private string _userInput="";
    public void OnClick()
    {
        _userInput = inputField.GetComponent<TMP_InputField>().text;
        ServerManager.Instance.GetGPTReply(_userInput);
    }
}
