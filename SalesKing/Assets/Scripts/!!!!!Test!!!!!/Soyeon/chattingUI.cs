using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class chattingUI : MonoBehaviour
{
    [SerializeField] private GameObject NpcSpeechBubble;

    public void SetNpcAnswerText(string text)
    {
        TextMeshProUGUI NpcSpeechText = NpcSpeechBubble.GetComponentInChildren<TextMeshProUGUI>();
        NpcSpeechText.text = text;
        NpcSpeechBubble.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        NpcSpeechBubble.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutBounce).SetUpdate(true);
    }

    [SerializeField] private GameObject WaitReplyPanel;
    private void SubWaitReply(bool beActive)
    {
        WaitReplyPanel.SetActive(beActive);
    }

    [SerializeField] GameObject ConvoPanel;
    [SerializeField] GameObject SummaryPanel;
    [SerializeField] GameObject EndPanel;

    [SerializeField] GameObject RecordPanel;
    [SerializeField] GameObject KeyboardPanel;

    [SerializeField] GameObject OkayBtn;

    private void Awake()
    {
        ChatManager.OnPanelUpdated -= ShowPanel;
        ChatManager.OnPanelUpdated += ShowPanel;
        ServerManager.OnSendReplyUpdate -= SubWaitReply;
        ServerManager.OnSendReplyUpdate += SubWaitReply;
    }

    private void OnDestroy()
    {
        ChatManager.OnPanelUpdated -= ShowPanel;
        ServerManager.OnSendReplyUpdate -= SubWaitReply;
    }

    #region 대화 시작하겠습니까?
    public void OnClickYesTalkBtn()
    {
        TutorialManager.Instance.OnRecord();
        ConvoPanel.SetActive(true);
        EndPanel.SetActive(false);
        WaitReplyPanel.SetActive(false);
        InitiateInputMode();
    }

    public void InitiateInputMode()
    {
        Define.UserInputMode defaultMode = Managers.Input.CurInputMode;

        if (defaultMode == Define.UserInputMode.Keyboard)
        {
            Debug.Log("키보드 인풋 모드로 초기화");
            RecordPanel.SetActive(false);
            KeyboardPanel.SetActive(true);
        }
        else if (defaultMode == Define.UserInputMode.Voice)
        {
            RecordPanel.SetActive(true);
            KeyboardPanel.SetActive(false);
        }
    }

    public void OnClickNoTalkBtn()
    {
        Managers.Convo.ConvoFinished();
        EndPanel.SetActive(false);
        ConvoPanel.SetActive(false);
    }
    #endregion

    public void ShowPanel(Define.SendChatType sendChatType, Define.EndType endType)
    {
        TextMeshProUGUI text = EndPanel.GetComponentInChildren<TextMeshProUGUI>();
            
        if ((endType == Define.EndType.clear && Managers.Chat.reason == 3) || endType == Define.EndType.buy)
            text.text = "판매 성공!";
            
        else
            text.text = "판매 실패...";

        StartCoroutine(ShowEndPanelAfterDelay());
    }

    private IEnumerator ShowEndPanelAfterDelay()
    {
        yield return new WaitForSecondsRealtime(1f);
        EndPanel.SetActive(true);
    }

    #region 대화 끝내기

    public void OnEndChat()
    {
        Managers.Convo.ConvoFinished();
        EndPanel.SetActive(false);
        ConvoPanel.SetActive(false);
    }
    #endregion

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
}

