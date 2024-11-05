using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class chattingUI : MonoBehaviour
{
    [SerializeField] private GameObject NpcSpeechBubble;
    [SerializeField] private GameObject UserSpeechBubble;
    public void SetNpcAnswerText(string text)
    {
        TextMeshProUGUI NpcSpeechText = NpcSpeechBubble.GetComponentInChildren<TextMeshProUGUI>();
        NpcSpeechText.text = text;
        NpcSpeechBubble.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        NpcSpeechBubble.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutBounce).SetUpdate(true);
    }

    public void SetUserAnswerText(string text)
    {
        TextMeshProUGUI UserSpeechText = UserSpeechBubble.GetComponentInChildren<TextMeshProUGUI>();
        UserSpeechText.text = text;
        UserSpeechBubble.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        UserSpeechBubble.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutBounce).SetUpdate(true);
    }

    [SerializeField] private GameObject WaitReplyPanel;
    private void SubWaitReply(bool beActive)
    {
        WaitReplyPanel.SetActive(beActive);
    }

    public GameObject ConvoPanel;
    public GameObject SummaryPanel;
    public GameObject EndPanel;

    public GameObject RecordPanel;
    public GameObject KeyboardPanel;

    public GameObject OkayBtn;

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
        Managers.UI.InitiateInputMode();
    }

    public void InitiateInputMode()
    {
        Define.UserInputMode defaultMode = Managers.Input.CurInputMode;

        if (defaultMode == Define.UserInputMode.Keyboard)
        {
            Debug.Log("키보드 인풋 모드로 초기화");
            if (ui != null)
            {
                ui.RecordPanel.SetActive(false);
                ui.KeyboardPanel.SetActive(true);
            }
        }
        else if (defaultMode == Define.UserInputMode.Voice)
        {
            ui.RecordPanel.SetActive(true);
            ui.KeyboardPanel.SetActive(false);
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
        // 3초 대기
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

