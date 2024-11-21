using TMPro;
using UnityEngine;

public class VariableInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField KeyboardText;
    [SerializeField] private TMP_InputField STTText;
    private string _userInput="";

    public void OnClick()
    {
        string userText = "";

        if (UserInputManager.Instance.CurInputMode == Define.UserInputMode.Keyboard)
        {
            userText = KeyboardText.text;
            KeyboardText.text = "";
        }
        else if (UserInputManager.Instance.CurInputMode == Define.UserInputMode.Voice)
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
        
        //TutorialManager.Instance.OnPersuadeToCustomer();
        ChatManager.Instance.Reply.UserAnswer = _userInput;

        if(UserInputManager.Instance.CurInputMode == Define.UserInputMode.Voice)
        {
            STTUI _sttUI = FindObjectOfType<STTUI>().GetComponent<STTUI>();
            _sttUI.OnClickEnter();
        }
    }
}
