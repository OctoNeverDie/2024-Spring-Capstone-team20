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
    public TextMeshProUGUI CashText;

    private void Start()
    {
        ChatManager.OnPanelUpdated += ShowPanel;
    }
    public void OnClickYesTalkBtn()
    {
        ConvoPanel.SetActive(true);
        TalkOrNotPanel.SetActive(false);
    }

    public void OnClickNoTalkBtn()
    {
        Managers.Convo.ConvoFinished();
        TalkOrNotPanel.SetActive(false);
        ConvoPanel.SetActive(false);
    }

    public void ShowPanel(Define.SendChatType sendChatType)
    {
        if (sendChatType == Define.SendChatType.ItemInit)
        {
            ConvoPanel.SetActive(false);
            ChooseItemPanel.SetActive(true);
        }
    }

    public void OnClickSelectItemBtn()
    {
        ChooseItemPanel.SetActive(false);
        ConvoPanel.SetActive(true);
        ConvoPanel.GetComponentInChildren<Button>()
    }

    public void OnClickTalkToNPCBtn()
    {

    }

    public void OnClickExitNPCBtn()
    {

    }

}

