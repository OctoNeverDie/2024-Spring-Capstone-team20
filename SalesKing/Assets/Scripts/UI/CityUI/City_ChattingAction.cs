using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City_ChattingAction : MonoBehaviour
{

    public bool InitiateInputMode()
    {
        Define.UserInputMode defaultMode = Managers.Input.CurInputMode;

        if (defaultMode == Define.UserInputMode.Keyboard)
        {
            Debug.Log("키보드 인풋 모드로 초기화");
            {
                RecordPanel.SetActive(false);
                KeyboardPanel.SetActive(true);
            }
        }
        else if (defaultMode == Define.UserInputMode.Voice)
        {
            RecordPanel.SetActive(true);
            KeyboardPanel.SetActive(false);
        }
    }
}
