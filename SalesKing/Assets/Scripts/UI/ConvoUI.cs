using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Define;
using DG.Tweening; // DoTween 네임스페이스 추가
using UnityEngine.SceneManagement;
using UnityEngine.Profiling;


public class ConvoUI : MonoBehaviour
{
    public GameObject TalkOrNotPanel;
    public GameObject ChooseItemPanel;
    public GameObject ConvoPanel;
    public GameObject SummaryPanel;
    public GameObject EndPanel;
    public GameObject DemoEndPanel;
    public GameObject WaitReplyPanel;

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

    private float todayGoal = 150;

    public GameObject OkayBtn;

    bool isSuccess;
    float smaller;
    public void SeeReaction(bool isSuccess, float price)
    {
        this.isSuccess = isSuccess;
        this.smaller = price;
        isBtn1 = false;
        OkayBtn.SetActive(true);
    }

    bool isBtn1 = false;
    public void PopOkayBtn()
    {
        isBtn1 = true;
        OkayBtn.SetActive(true);
    }

    public void OnOkayBtn()
    {
        if (isBtn1)
        {
            OkayBtn.SetActive(false);
            Managers.Chat.EvalManager.ThingToBuy = "";
            Managers.Chat.TransitionToState(SendChatType.ItemInit);
        }
        else if (!isBtn1)
        {
            OkayBtn.SetActive(false);
            if (isSuccess)
            {
                Managers.Chat.EvalManager.AddItemPriceSold(smaller);
                Managers.Chat._endType = EndType.buy;
                Managers.Chat.TransitionToState(SendChatType.Endpoint);
            }
            else
            {
                Managers.Chat._endType = EndType.reject;
                Managers.Chat.TransitionToState(SendChatType.Endpoint);
            }
        }
        
    }

    private void Awake()
    {
        ChatManager.OnPanelUpdated -= ShowPanel;
        ChatManager.OnPanelUpdated += ShowPanel;
        ServerManager.OnSendReplyUpdate -= SubWaitReply;
        ServerManager.OnSendReplyUpdate += SubWaitReply;

        ChatBargainState.ChatBargainReactState -= SeeReaction;
        ChatBargainState.ChatBargainReactState += SeeReaction;
    }

    private void OnDestroy()
    {
        ChatManager.OnPanelUpdated -= ShowPanel;
        ServerManager.OnSendReplyUpdate -= SubWaitReply;

        ChatBargainState.ChatBargainReactState -= SeeReaction;
    }

    private void SubWaitReply(bool beActive)
    {
        WaitReplyPanel.SetActive(beActive);
    }

    #region 대화 시작하겠습니까?
    public void OnClickYesTalkBtn()
    {
        TutorialManager.Instance.OnRecord();
        ConvoPanel.SetActive(true);
        ActivateDealBtn(false);
        EndPanel.SetActive(false);
        WaitReplyPanel.SetActive(false);
        TalkOrNotPanel.SetActive(false);
        Managers.UI.InitiateInputMode();
    }

    public void OnClickNoTalkBtn()
    {
        Managers.Convo.ConvoFinished();
        TalkOrNotPanel.SetActive(false);
        EndPanel.SetActive(false);
        ConvoPanel.SetActive(false);
    }
    #endregion

    public void ShowPanel(Define.SendChatType sendChatType, Define.EndType endType)
    {
        if (sendChatType == Define.SendChatType.NpcInit)
        {
            PopOkayBtn();
        }
        else if (sendChatType == Define.SendChatType.ItemInit)
        {
            EndPanel.SetActive(false);
            ConvoPanel.SetActive(false);
            ChooseItemPanel.SetActive(true);
        }
        else if (sendChatType == Define.SendChatType.ChatBargain)
        {
            ActivateDealBtn();
        }
        else if (sendChatType == Define.SendChatType.Endpoint)
        {
            TextMeshProUGUI text = EndPanel.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI btnText = EndPanel.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>();

            if (endType == Define.EndType.buy)
            {
                text.text = "물건 판매 성공~!";
                btnText.text = "짱~!";
                if (Managers.Chat.reason == 3)
                    text.text = "제시가가 판매가보다 낮아서\n" + text.text;
                else if (Managers.Chat.reason == 4)
                    text.text = "판매가대로\n" + text.text;
            }
            else if (endType == Define.EndType.reject || endType == Define.EndType.clear)
            {
                text.text = "물건 판매 실패...";
                btnText.text = "우...";
                if (Managers.Chat.reason == 1)
                    text.text = "상대 기분이 나빠짐...\n" + text.text;
                else if (Managers.Chat.reason == 2)
                    text.text = "대화 에너지 다함...\n" + text.text;
            }

            StartCoroutine(ShowEndPanelAfterDelay());
        }
    }

    private IEnumerator ShowEndPanelAfterDelay()
    {
        // 3초 대기
        yield return new WaitForSecondsRealtime(1f);
        EndPanel.SetActive(true);
    }

    public void OnClickSelectItemBtn(GameObject priceTab)
    {
        priceTab.transform.gameObject.SetActive(false);
        ChooseItemPanel.SetActive(false);
        ConvoPanel.SetActive(true);
        EndPanel.SetActive(false);
    }

    #region 물건 사기
    public void OnClickBuy()//딜 버튼 누름
    {
        ShowPanel(Define.SendChatType.Endpoint, Define.EndType.buy);
    }

    public void OnEndChat()
    {
        EndPanel.SetActive(true);
        ConvoPanel.SetActive(false);

        Managers.Convo.ConvoFinished();
        EndPanel.SetActive(false);
    }
    #endregion
  
    public void OnClickExitNPCBtn()
    {
        Managers.Convo.ConvoFinished();
        EndPanel.SetActive(false);
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

    //------------------------------------------------------
    private void ActivateDealBtn(bool activate = true)
    {
        DealBtn dealBtn = ConvoPanel.GetComponentInChildren<DealBtn>(true);
        if (dealBtn != null)
        {
            if (!activate)
            {
                dealBtn.gameObject.SetActive(false);
                return;
            }

            dealBtn.gameObject.SetActive(true);
            dealBtn.GetComponent<Button>().interactable = true;
        }
    }

    public void OnclickEndingBtn()
    {
        if (Managers.Cash.TotalCash >= todayGoal)
        {
            Debug.Log("성공~");
            EndingScene(true);
        }
        else
        {
            Debug.Log("실패~");
            EndingScene(false);
        }
    }

    public void EndingScene(bool isSurvive)
    {
        DemoEndPanel.SetActive(true); // DemoEndPanel 활성화

        GameObject gObject;

        if (isSurvive)
        {
            // DemoEndPanel 내에서 Suceed 오브젝트 찾기
            gObject = DemoEndPanel.transform.Find("Succeed").gameObject;

        } else
        {
            // DemoEndPanel 내에서 Suceed 오브젝트 찾기
            gObject = DemoEndPanel.transform.Find("Fail").gameObject;

            // Suceed 오브젝트 활성화
        }
        gObject.SetActive(true);

    }

    public void OnClickRestart()
    {
        //첫 장면을 가져오게 된다.
        //GetActiveScene.name를 통해 현재 scene의 이름을 받아온다.
        //LoadScene을 통해 해당 scene을 실행한다.
        SceneManager.LoadScene("Start");
    }

    public void OnClickFail()
    {
        SceneManager.LoadScene("Last");
    }

}

