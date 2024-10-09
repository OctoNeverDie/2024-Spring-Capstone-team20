using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Define;

public class ConvoUI : MonoBehaviour
{
    public GameObject TalkOrNotPanel;
    public GameObject ChooseItemPanel;
    public GameObject ConvoPanel;
    public GameObject SummaryPanel;
    public GameObject EndPanel;

    public GameObject NPCSpeechBubble;
    public TextMeshProUGUI NPCSpeechText;
    public GameObject UserSpeechBubble;
    public TextMeshProUGUI UserSpeechText;
    public TextMeshProUGUI todayCashText;


    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI CashText;
    public TextMeshProUGUI TurnText;

    public GameObject RecordPanel;
    public GameObject KeyboardPanel;

    public TMP_InputField UserInputField;

    private float todayGoal = 300;

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
        Managers.UI.InitiateInputMode();
    }

    public void OnClickNoTalkBtn()
    {
        Managers.Convo.ConvoFinished();
        TalkOrNotPanel.SetActive(false);
        ConvoPanel.SetActive(false);
    }
    #endregion


    public void ShowPanel(Define.SendChatType sendChatType, Define.EndType endType)
    {
        if (sendChatType == Define.SendChatType.ItemInit)
        {
            ConvoPanel.SetActive(false);
            ChooseItemPanel.SetActive(true);
        }
        else if (sendChatType == Define.SendChatType.ChatBargain)
        {
            DealBtn dealBtn = ConvoPanel.GetComponentInChildren<DealBtn>(true);
            if (dealBtn != null)
            {
                dealBtn.gameObject.SetActive(true);
                dealBtn.GetComponent<Button>().interactable = true;
            }
        }
        else if (sendChatType == Define.SendChatType.Endpoint)
        {
            TextMeshProUGUI text = EndPanel.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI btnText = EndPanel.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>();

            if (endType == Define.EndType.Success)
            {
                Debug.Log("????");
                text.text = "물건 판매 성공~!";
                btnText.text = "짱~!";
            }
            else if (endType == Define.EndType.Fail || endType == Define.EndType.Leave)
            {
                Debug.Log("!!!!");
                text.text = "물건 판매 실패...";
                btnText.text = "우...";
            }

            EndPanel.SetActive(true);
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
        Button dealBtn = ConvoPanel.GetComponentInChildren<DealBtn>().GetComponent<Button>();
        if (dealBtn != null)
        {
            dealBtn.interactable = false;
        }
        ShowPanel(Define.SendChatType.Endpoint, Define.EndType.Success);
    }

    public void OnChatLeave()
    {
        Managers.Chat._endType = EndType.Leave;
        OnEndChat();
    }

    public void OnEndChat()
    {
        Managers.Chat.TransitionToState(SendChatType.Endpoint);

        EndPanel.SetActive(false);
        
        if (ConvoPanel.GetComponentInChildren<DealBtn>(true).GetComponent<Button>() != null)
        {
            ConvoPanel.GetComponentInChildren<DealBtn>(true).GetComponent<Button>().interactable = false;
        }
        ConvoPanel.SetActive(false);

        Managers.Convo.ConvoFinished();
    }
    #endregion
  
    public void OnClickTalkToNPCBtn()
    {

    }

    public void OnClickExitNPCBtn()
    {
        Managers.Convo.ConvoFinished();
        ConvoPanel.SetActive(false);
    }

    public void OnClickSwitchBtn()
    {
        // switch to voice
        if (Managers.Input.CurInputMode == Define.UserInputMode.Keyboard)
        {
            RecordPanel.SetActive(true);
            KeyboardPanel.SetActive(false);
            Managers.Input.CurInputMode = Define.UserInputMode.Voice;
        }
        else if (Managers.Input.CurInputMode == Define.UserInputMode.Voice)
        {
            RecordPanel.SetActive(false);
            KeyboardPanel.SetActive(true);
            Managers.Input.CurInputMode = Define.UserInputMode.Keyboard;

        }
    }

    public void OnclickEndingBtn()
    {
        if (Managers.Cash.TotalCash >= todayGoal)
        {
            Debug.Log("성공~");
        }
        else
        {
            Debug.Log("실패~");
        }
    }

}

