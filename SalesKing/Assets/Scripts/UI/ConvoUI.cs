using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConvoUI : MonoBehaviour
{
    public GameObject TalkOrNotPanel;
    public GameObject ChooseItemPanel;
    public GameObject ConvoPanel;

    public TMP_InputField UserText;
    public GameObject NPCSpeechBubble;
    public TextMeshProUGUI NPCSpeechText;

    public TextMeshProUGUI StatusText;
    public TextMeshProUGUI TimeText;

    public void YesTalkBtn()
    {
        ChooseItemPanel.SetActive(true);
        TalkOrNotPanel.SetActive(false);
    }

    public void NoTalkBtn()
    {
        Managers.Convo.ConvoFinished();
        TalkOrNotPanel.SetActive(false);
        ConvoPanel.SetActive(false);
    }

    public void SelectItemBtn()
    {
        ChooseItemPanel.SetActive(false);
        ConvoPanel.SetActive(true);
    }

}

