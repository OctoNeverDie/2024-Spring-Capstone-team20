using System;
using UnityEngine;
using static Define;

/// <summary>
/// Chatting State의 상태를 변경하기
/// Chatting State에게 필요한 object 제공
/// Chatting State 간 정보교환 제공
/// </summary>

public class ChatManager : Singleton<ChatManager> , ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => false;

    [SerializeField] StartChattingMockData mockData;
    private NpcInfo thisNpc;
    public NpcInfo ThisNpc { get; }

    public ReplySubManager Reply = new ReplySubManager();
    public EvalSubManager Eval = new EvalSubManager();
    public NpcSupplyManager npcSupplyManager = new NpcSupplyManager();

    public static event Action<SendChatType, EndType> OnPanelUpdated;
    private ChatStateMachine _chatStateMachine;

    public void Init()
    {
        _chatStateMachine = new ChatStateMachine();
        NpcInit();
    }

    private void NpcInit(int npcID = 0)
    {
        thisNpc = mockData.Encountered(npcID);

        _chatStateMachine.SetState(new NpcInitState()); //back용
        //ui용
    }

    public void Clear()
    {//TODO : eval도 지우고, reply도 지우고, UI도 지우고
        ReplyManager.ClearReplyData();
        OnNumberUpdated?.Invoke(0, 0, 0);
    }

    public void ActivatePanel(SendChatType chatState)
    {
        if (chatState == SendChatType.Endpoint)
        {
            //endtype따라 마지막 패널 달라진다!
            OnPanelUpdated?.Invoke(chatState, _endType);
        }
        else
        {
            OnPanelUpdated?.Invoke(chatState, EndType.None);
        }
    }

    public static event Action<int, float, float> OnNumberUpdated;


    public EndType _endType { get; set; }

    public void TransitionToState(SendChatType sendChatType)
    {
        _chatStateMachine.TransitionToState(sendChatType);
    }
}
