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
    public GameObject ItemSoldPanel;
    public GameObject YoufailedPanel;

    public TMP_InputField UserText;
    public GameObject NPCSpeechBubble;
    public TextMeshProUGUI NPCSpeechText;

    public TextMeshProUGUI StatusText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI CashText;

    private void Start()
    {
        ChatManager.OnPanelUpdated -= ShowPanel;
        ChatManager.OnPanelUpdated += ShowPanel;
    }

    #region 대화 시작하겠습니까?
    public void OnClickYesTalkBtn()
    {
        TutorialManager.Instance.OnRecord();
        ConvoPanel.SetActive(true);
        TalkOrNotPanel.SetActive(false);
    }

    public void OnClickNoTalkBtn()
    {
        Managers.Convo.ConvoFinished();
        TalkOrNotPanel.SetActive(false);
        ConvoPanel.SetActive(false);
    }
    #endregion


    public void ShowPanel(Define.SendChatType sendChatType)
    {
        if (sendChatType == Define.SendChatType.ItemInit)
        {
            ConvoPanel.SetActive(false);
            ChooseItemPanel.SetActive(true);
        }
        else if (sendChatType == Define.SendChatType.ChatBargain)
        {
            ConvoPanel.GetComponentInChildren<IDeal>().gameObject.SetActive(true);
        }
        else if (sendChatType == Define.SendChatType.Success)
        {
            ItemSoldPanel.SetActive(true);
        }
        else if (sendChatType == Define.SendChatType.Fail)
        { 
            YoufailedPanel.SetActive(true);
        }

    }

    public void OnClickSelectItemBtn()
    {
        ChooseItemPanel.SetActive(false);
        ConvoPanel.SetActive(true);
        //ConvoPanel.GetComponentInChildren<Button>()
    }

    #region 물건 사기
    public void OnClickBuy()//딜 버튼 누름
    {
        Managers.Chat.CheckTurnSuccess();
        
        this.gameObject.SetActive(false);
    }

    public void OnEndChat()
    {
        Managers.Chat.Clear();

        ItemSoldPanel.SetActive(false);
        YoufailedPanel.SetActive(false);
        ConvoPanel.GetComponentInChildren<IDeal>().gameObject.SetActive(false);
        ConvoPanel.SetActive(false);

        Managers.Convo.ConvoFinished();
    }
    #endregion
  
    public void OnClickTalkToNPCBtn()
    {

    }

    public void OnClickExitNPCBtn()
    {

    }

}

