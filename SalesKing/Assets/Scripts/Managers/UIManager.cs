using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameObject myCanvas;
    ConvoUI ui;

    void Start()
    {
        myCanvas = GameObject.Find("Canvas");
        ui = myCanvas.GetComponent<ConvoUI>();
    }

    public void ShowTalkOrNotPanel()
    {
        ui.TalkOrNotPanel.SetActive(true);
    }

    public void SetConvoPanel()
    {

    }

    public void SetNPCAnswerText(string text)
    {
        ui.NPCSpeechText.text = text;
    }
}
