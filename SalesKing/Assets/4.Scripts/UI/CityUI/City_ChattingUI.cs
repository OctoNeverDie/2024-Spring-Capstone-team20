using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class City_ChattingUI : MonoBehaviour
{
    [SerializeField] GameObject WaitReplyPanel;//server waiting panel
    [SerializeField] GameObject NpcSpeechBubble;//user answer panel
    [SerializeField] GameObject EndPanel;
    [SerializeField] GameObject ConvoPanel;
    [SerializeField] GameObject TxtPopUpUI;
    [SerializeField] GameObject RandItemPanel;

    [SerializeField] City_TabletDataManager Tablet;

    [SerializeField] Button UserEndBtn; //end conversation
    [SerializeField] Button DealBtn; //deal ended
    [SerializeField] Button ItemBtn;

    TextMeshProUGUI NpcSpeechText;
    Image CheckMark;

    private enum PersuasionLevel
    {
        Like,
        Normal,
        Dislike
    }

    private void Awake()
    {
        ServerManager.OnSendReplyUpdate -= SubWaitReply;
        ServerManager.OnSendReplyUpdate += SubWaitReply;
        STTConnect.OnSendClovaUpdate -= SubWaitReply;
        STTConnect.OnSendClovaUpdate += SubWaitReply;

        UserEndBtn.onClick.AddListener(OnClickLeaveFSM);
        DealBtn.onClick.AddListener(OnClickFinal);
        ItemBtn.onClick.AddListener(OnClickItem);

        TxtPopUpUI.SetActive(false);
        ConvoPanel.SetActive(false);

        NpcSpeechText = NpcSpeechBubble.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        ServerManager.OnSendReplyUpdate -= SubWaitReply;
        STTConnect.OnSendClovaUpdate -= SubWaitReply;
    }

    private void SubWaitReply(bool beActive)
    {
        WaitReplyPanel.SetActive(beActive);
    }

    private void OnClickLeaveFSM()
    {
        ChatManager.Instance.EndByUser();
    }

    public void OnClickFinal()
    {
        SetNpcAnswerText("");
        TxtPopUpUI.SetActive(false);
        EndPanel.SetActive(false);
        ConvoPanel.SetActive(false);
        PlayerManager.Instance.player.PlayerExitConvo();

        if (ChatManager.Instance.npcNum >= 3)
        {
            Tablet.ShowSummary();
        }
    }

    public void OnClickItem()
    {
        ChatManager.Instance.TransitionToState(Define.SendChatType.Chatting);
    }

    private void SetNpcName(string name)
    {
        Transform infoTransform = NpcSpeechBubble.transform.Find("Info");
        if (infoTransform != null)
        {
            TextMeshProUGUI infoText = infoTransform.GetComponent<TextMeshProUGUI>();
            if (infoText != null)
                infoText.text = name;

            //check check mark
            CheckMark = infoTransform.GetComponentInChildren<Image>(true);
            CheckMark.gameObject.SetActive(false);
        }
    }

    public void SetNpcAnswerText(string text)
    {
        NpcSpeechText.text = text;
        NpcSpeechBubble.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        NpcSpeechBubble.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutBounce).SetUpdate(true);
    }

    public void ShowPanel(Define.SendChatType sendChatType, object additionalData = null, string name=null, bool isEndByUser =false)
    {
        if (sendChatType == Define.SendChatType.ChatInit)
        {
            SetNpcName(name);
            ConvoPanel.SetActive(true);// show convo: npc name, 
            RandItemPanel.SetActive(true);
        }

        else if (sendChatType == Define.SendChatType.Chatting)
        {
            if (additionalData is ChattingState.GptResult gptResult)
            {
                SetNpcAnswerText(gptResult.reaction);//reply 보여줌
                NPCManager.Instance.curTalkingNPC.PlayNPCAnimByEmotion(gptResult.emotion);//애니메이션 보여줌

                if (gptResult.Persuasion >= 2)
                {
                    TxtPopup(gptResult.reason, PersuasionLevel.Like);//++ 효과, 초록색, gptResult.reason 뒤에 따라옴.
                }
                else if (gptResult.Persuasion <= -2)
                {
                    TxtPopup(gptResult.reason, PersuasionLevel.Dislike);//-- 효과, 빨간색, gptResult.reason 뒤에 따라옴.
                }
                else
                {
                    TxtPopup(gptResult.reason, PersuasionLevel.Normal);
                }
            }
        }

        else if (sendChatType == Define.SendChatType.Endpoint)
        {
            if (additionalData is bool isSuccess)
            {
                ShowCheckMark(isSuccess);
            }

            StartCoroutine(ShowEndPanelAfterDelay());
        }
    }

    private void ShowCheckMark(bool isSuccess)
    {
        CheckMark.color = isSuccess ? Color.green : Color.red;

        CheckMark.gameObject.SetActive(true);

        //animation
        CheckMark.transform.localScale = CheckMark.transform.localScale * 0.1f;
        List<(float scale, float duration)> tweenFactors = new List<(float, float)>
        {
            (20f, 1.5f),
            (12f, 0.75f),
            (10f, 0.75f),
        };
        Util.PopDotween(CheckMark.transform, tweenFactors);
    }

    private IEnumerator ShowEndPanelAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2.3f);
        EndPanel.SetActive(true);
    }

    private void TxtPopup(string reason, PersuasionLevel level)
    {
        if (level == PersuasionLevel.Like)
        {
            reason = "<color=green>" + "++ " + reason + "</color>";
        }
        else if (level == PersuasionLevel.Dislike)
        {
            reason = "<color=red>" + "-- " + reason + "</color>";
        }
        else if(level == PersuasionLevel.Normal)
        {
            reason = "<color=grey>" + reason + "</color>";
        }

        TxtPopUpUI.GetComponentInChildren<TextMeshProUGUI>().text = reason;

        float vecX = Random.Range(-350f, 350f);
        float vecY = Random.Range(-150, 205);
        RectTransform rectTransform = TxtPopUpUI.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(vecX, vecY);
        
        TxtPopUpUI.SetActive(false);
        TxtPopUpUI.SetActive(true);
    }
}
