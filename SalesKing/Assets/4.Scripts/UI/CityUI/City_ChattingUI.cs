using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class City_ChattingUI : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject WaitReplyPanel;//server waiting panel
    [SerializeField] GameObject NpcSpeechBubble;//user answer panel
    [SerializeField] GameObject EndPanel;
    [SerializeField] GameObject ConvoPanel;
    [SerializeField] GameObject TxtPopUpUI;
    [SerializeField] GameObject TipPopUpUI;
    [SerializeField] GameObject RandItemPanel;
    [SerializeField] GameObject TabletAlert;

    [Header("Scripts")]
    [SerializeField] City_TabletDataManager Tablet;

    [Header("Buttons")]
    [SerializeField] Button UserEndBtn; //end conversation
    [SerializeField] Button DealBtn; //deal ended
    [SerializeField] Button ItemBtn;

    [Header("Etc")]
    [SerializeField] TextMeshProUGUI npcItem;
    [SerializeField] Sprite Success;
    [SerializeField] Sprite Failed;

    TextMeshProUGUI NpcSpeechText;
    Image CheckMark;
    Define.GameMode thisGame;

    public static event Action<Define.Emotion> OnEmotionSetup;

    private enum PersuasionLevel
    {
        Like,
        Normal,
        Dislike
    }

    private void Awake()
    {
        ServerManager.OnSendReplyUpdate += SetUIafterReply;
        ServerManager.OnSendReplyUpdate += SubWaitReply;
        STTConnect.OnSendClovaUpdate += SubWaitReply;

        UserEndBtn.onClick.AddListener(OnClickLeaveFSM);
        DealBtn.onClick.AddListener(OnClickFinal);
        ItemBtn.onClick.AddListener(OnClickItem);

        TxtPopUpUI.SetActive(false);
        ConvoPanel.SetActive(false);

        NpcSpeechText = NpcSpeechBubble.GetComponentInChildren<TextMeshProUGUI>();
        if (MuhanNpcDataManager.Instance == null)
            thisGame = Define.GameMode.Story;
        else
            thisGame = Define.GameMode.Infinity;
    }

    private void OnDestroy()
    {
        ServerManager.OnSendReplyUpdate -= SetUIafterReply;
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
        NPCManager.Instance.curTalkingNPC.NPCExitConvo();

        if (ChatManager.Instance.npcNum >= 3)
        {
            TurnManager.Instance.EndDayShowSummary();
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

    public void ShowPanel(Define.SendChatType sendChatType, object additionalData = null, NpcInfo npcInfo=null)
    {
        if (sendChatType == Define.SendChatType.ChatInit)
        {
            //태블릿 경고
            if(npcInfo.NpcID !=0)
                TabletAlert.SetActive(true);

            //전체 패널
            SetNpcName(npcInfo.NpcName);
            ConvoPanel.SetActive(true);// show convo: npc name, 

            //랜덤 아이템 패널
            npcItem.text = $"상대가 원했던 물품 : " + npcInfo.WantItem;
            RandItemPanel.SetActive(true);
        }

        else if (sendChatType == Define.SendChatType.Chatting)
        {
            if (additionalData is ChattingState.GptResult gptResult)
            {
                SetNpcAnswerText(gptResult.reaction);//reply 보여줌
                NPCManager.Instance.curTalkingNPC.PlayNPCAnimByEmotion(gptResult.emotion);//애니메이션 보여줌

                if (gptResult.Persuasion > 0)
                {
                    AudioManager.Instance.PlaySFX("Good");
                    TxtPopup(gptResult.reason, PersuasionLevel.Like);//++ 효과, 초록색, gptResult.reason 뒤에 따라옴.
                    OnEmotionSetup?.Invoke(Define.Emotion.best);
                }
                else if (gptResult.Persuasion < 0)
                {
                    AudioManager.Instance.PlaySFX("Bad");
                    TxtPopup(gptResult.reason, PersuasionLevel.Dislike);//-- 효과, 빨간색, gptResult.reason 뒤에 따라옴.
                    OnEmotionSetup?.Invoke(Define.Emotion.worst);
                }
                else
                {
                    TxtPopup(gptResult.reason, PersuasionLevel.Normal);
                    OnEmotionSetup?.Invoke(Define.Emotion.normal);
                }
            }
        }

        else if (sendChatType == Define.SendChatType.Endpoint)
        {
            if(thisGame == Define.GameMode.Story)
                TipPopUpUI.SetActive(false);
            OnEmotionSetup?.Invoke(Define.Emotion.normal);
            if (additionalData is bool isSuccess)
            {
                if (CheckMark != null)
                {
                    ShowCheckMark(isSuccess);
                } 
            }

            StartCoroutine(ShowEndPanelAfterDelay());
        }
    }

    private void ShowCheckMark(bool isSuccess)
    {
        if (isSuccess)
        {
            AudioManager.Instance.PlaySFX("Clear");
            CheckMark.color = Color.green;
            Util.ChangeSprite(CheckMark, Success);
        }
        else 
        {
            AudioManager.Instance.PlaySFX("Fail");
            CheckMark.color = Color.red;
            Util.ChangeSprite(CheckMark, Failed);
        }

        CheckMark.gameObject.SetActive(true);

        //animation
        CheckMark.transform.localScale = CheckMark.transform.localScale * 0.1f;
        List<(float scale, float duration)> tweenFactors = new List<(float, float)>
        {
            (20f, 1.5f),
            (12f, 0.75f),
            (10f, 0.75f),
        };
        Sequence seq = Util.PopDotween(CheckMark.transform, tweenFactors);
        seq.Play();
    }

    private IEnumerator ShowEndPanelAfterDelay()
    {
        yield return new WaitForSeconds(2.3f);
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
        else if (level == PersuasionLevel.Normal)
        {
            if(reason == null || reason == "")
                reason = "";
            else
                reason = "<color=grey>" + reason + "</color>";
        }

        TxtPopUpUI.GetComponentInChildren<TextMeshProUGUI>().text = reason;

        float vecX = UnityEngine.Random.Range(-350f, 350f);
        float vecY = UnityEngine.Random.Range(-150, 205);
        RectTransform rectTransform = TxtPopUpUI.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(vecX, vecY);

        TxtPopUpUI.SetActive(false);
        TxtPopUpUI.SetActive(true);
    }

    private void SetUIafterReply(bool isDeactive)
    {
        if(!isDeactive && (thisGame == Define.GameMode.Story))
            TipPopUpUI.SetActive(true);
    }
}
