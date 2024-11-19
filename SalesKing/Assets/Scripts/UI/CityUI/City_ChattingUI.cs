using System.Collections;
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
    //[SerializeField] GameObject RandItemPanel;

    [SerializeField] City_TabletUI Tablet;

    [SerializeField] Button UserEndBtn; //end conversation
    [SerializeField] Button DealBtn; //deal ended
    private void Awake()
    {
        ServerManager.OnSendReplyUpdate -= SubWaitReply;
        ServerManager.OnSendReplyUpdate += SubWaitReply;

        UserEndBtn.onClick.AddListener(OnClickLeaveFSM);
        DealBtn.onClick.AddListener(OnClickFinal);
    }

    private void OnDestroy()
    {
        ServerManager.OnSendReplyUpdate -= SubWaitReply;
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
        EndPanel.SetActive(false);
        ConvoPanel.SetActive(false);

        if (ChatManager.Instance.npcNum >= 3)
        {
            Tablet.ShowSummary();
        }
    }

    private void SetNpcName(string name)
    {
        Transform infoTransform = NpcSpeechBubble.transform.Find("Info");
        if (infoTransform != null)
        {
            TextMeshProUGUI infoText = infoTransform.GetComponent<TextMeshProUGUI>();
            if (infoText != null)
                infoText.text = name;
        }
    }

    public void SetNpcAnswerText(string text)
    {
        TextMeshProUGUI NpcSpeechText = NpcSpeechBubble.GetComponentInChildren<TextMeshProUGUI>();
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
            //TODO :ItemInfo randItem, npc item 룰렛
        }

        else if (sendChatType == Define.SendChatType.Chatting)
        {
            if (additionalData is ChattingState.GptResult gptResult)
            {
                SetNpcAnswerText(gptResult.reaction);//reply 보여줌
                if (gptResult.persuasion >= 3)
                { //TODO : ++ 효과, 초록색, gptResult.reason 뒤에 따라옴.
                }
                else if (gptResult.persuasion <= -3)
                { //TODO : -- 효과, 빨간색, gptResult.reason 뒤에 따라옴.
                }
            }
        }

        else if (sendChatType == Define.SendChatType.Endpoint)
        {
            ConvoManager.Instance.ConvoFinished();
            StartCoroutine(ShowEndPanelAfterDelay());
        }
    }

    private IEnumerator ShowEndPanelAfterDelay()
    {
        yield return new WaitForSecondsRealtime(1f);
        EndPanel.SetActive(true);
    }
}
