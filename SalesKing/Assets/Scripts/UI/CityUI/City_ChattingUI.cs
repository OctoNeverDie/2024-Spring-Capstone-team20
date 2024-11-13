using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class City_ChattingUI : MonoBehaviour
{
    [SerializeField] GameObject WaitReplyPanel;//server waiting panel
    [SerializeField] GameObject NpcSpeechBubble;//user answer panel
    [SerializeField] TMP_InputField UserInputText;//user Input text
    [SerializeField] GameObject EndPanel;
    [SerializeField] GameObject ConvoPanel;
    [SerializeField] GameObject RandItemPanel;

    [SerializeField] GameObject RecordPanel;
    [SerializeField] GameObject KeyboardPanel;

    [SerializeField] GameObject SummaryPanel;

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
            SummaryPanel.SetActive(true);
        }
    }

    public void SetNpcAnswerText(string text)
    {
        TextMeshProUGUI NpcSpeechText = NpcSpeechBubble.GetComponentInChildren<TextMeshProUGUI>();
        NpcSpeechText.text = text;
        NpcSpeechBubble.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        NpcSpeechBubble.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutBounce).SetUpdate(true);
    }

    public void ShowPanel(Define.SendChatType sendChatType, object additionalData = null, bool isEndByUser=false)
    {
        if (sendChatType == Define.SendChatType.ChatInit)
        {
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
