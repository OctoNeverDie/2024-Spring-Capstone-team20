using System.Collections;
using UnityEngine;
using static Define;
using System;

/// <summary>
/// Scene load하면 사라짐.
/// Chatting State의 상태를 변경하기
/// Chatting State에게 필요한 object 제공
/// Chatting State 간 정보교환 제공
/// </summary>

public class ChatManager : Singleton<ChatManager> , ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => false;

    [SerializeField] City_ChattingUI cityChattingUI;
    [SerializeField] City_TabletDataManager cityTabletData;
    [SerializeField] NewsSpawner newsSpawner;

    public NpcInfo ThisNpc { get; private set; }

    public int npcNum { get; private set; } = 0;
    public string playerItemName = "사탕"; 

    public ReplySubManager Reply = new ReplySubManager();

    private ChatStateMachine _chatStateMachine;
    public bool isConvo { get; private set; } = false;

    public void Init(int NpcID=0)
    {
        isConvo = true;
        
        ThisNpc = cityTabletData.todaysIDdict[NpcID];

        if (_chatStateMachine == null)
            _chatStateMachine = new ChatStateMachine();
        _chatStateMachine.SetState(new NpcInitState()); //back용
    }

    public void NpcCountUp()
    {
        npcNum++;
    }

    public void FirstNpc()
    {
        StartCoroutine(LateReply());
    }

    public string FindEval(int npcID) {
        return  newsSpawner.injector.allNews[npcID];
    }
    private IEnumerator LateReply()
    {
        yield return new WaitForSecondsRealtime(1f);
        string forFirstReply = "{ \"decision\" : \"yes\", \n\"yourReply\" : \"와! 물건 맞게 가져오셨네요. 거래 할게요~!\", \n\"persuasion\" : \"+3\", \n \"reason\" : \"물건이 마음에 듦\", \n \"summary\" : \"쿨거래 감사해요~\", \n\"emotion\" : \"best\"\n}";
        Reply.GptAnswer = forFirstReply;
    }

    private string _thisNpcSummary = "";
    private bool _isBuy;

    public void EndByUser()
    {
        _isBuy = false;
        TransitionToState(SendChatType.Endpoint);
    }

    public void updateThisSummary(string summary, bool isBuy) {
        _thisNpcSummary = summary;
        _isBuy = isBuy;
    }

    public void ActivatePanel(SendChatType chatState, object additionalData = null)
    {
        switch (chatState)
        {
            case SendChatType.ChatInit:
                if (additionalData is string ItemName)
                { 
                    playerItemName = ItemName;
                }

                cityTabletData.UpdateItemData(playerItemName, ThisNpc.NpcID); // show tablet: npc name ~ npc want item
                cityChattingUI.ShowPanel(chatState, playerItemName, ThisNpc); // show convo: npc name, npc item 룰렛
                break;

            case SendChatType.Chatting:
                if (additionalData is ChattingState.GptResult gptResult)
                {
                    cityChattingUI.ShowPanel(chatState, gptResult);// reply도 보여주고, persuasion에 따른 reason에 대한 ++, -- 보여주기
                }
                break;

            case SendChatType.Endpoint:
                isConvo = false;
                Debug.Log($"Endpoint isSuccess {ThisNpc.NpcID}");
                cityTabletData.UpdateEvaluationData(ThisNpc, _thisNpcSummary, _isBuy);
                cityChattingUI.ShowPanel(chatState, _isBuy); // convo가 끝나 카메라가 돌아가고, end Panel 하나만 띄우기
                break;

            default:
                throw new NotSupportedException($"The chat state '{chatState}' is not supported.");
        }
    }

    public void TransitionToState(SendChatType sendChatType)
    {
        _chatStateMachine.TransitionToState(sendChatType);
    }
}
