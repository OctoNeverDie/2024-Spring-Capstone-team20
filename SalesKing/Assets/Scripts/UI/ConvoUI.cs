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

    public void OnClickYesTalkBtn()
    {
        ChooseItemPanel.SetActive(true);
        TalkOrNotPanel.SetActive(false);
    }

    public void OnClickNoTalkBtn()
    {
        Managers.Convo.ConvoFinished();
        TalkOrNotPanel.SetActive(false);
        ConvoPanel.SetActive(false);
    }

    public void OnClickSelectItemBtn()
    {
        ChooseItemPanel.SetActive(false);
        ConvoPanel.SetActive(true);
    }

    public void OnClickTalkToNPCBtn()
    {

    }

    public void OnClickExitNPCBtn()
    {

    }

}

