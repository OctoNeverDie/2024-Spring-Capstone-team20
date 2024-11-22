using System;
using UnityEngine;
using static Define;

/// <summary>
/// Scene load하면 사라짐.
/// Chatting State의 상태를 변경하기
/// Chatting State에게 필요한 object 제공
/// Chatting State 간 정보교환 제공
/// </summary>

public class ChatManager : Singleton<ChatManager> , ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => true;

    [SerializeField] City_ChattingUI cityChattingUI;
    [SerializeField] City_TabletDataManager cityTabletData;

    public NpcInfo ThisNpc { get; private set; }
    public int npcNum { get; private set; } = 0;
    public bool isEndByUser { get; private set; } = false;

    public ReplySubManager Reply = new ReplySubManager();
    public EvalSubManager Eval = new EvalSubManager();

    private ChatStateMachine _chatStateMachine;

    public void Init(int NpcID=0)
    {
        ThisNpc = cityTabletData.todaysIDdict[NpcID];
        _chatStateMachine = new ChatStateMachine();
        _chatStateMachine.SetState(new NpcInitState()); //back용
    }

    public void NpcCountUp()
    {
        npcNum++;
    }

    public void EndByUser()
    {
        isEndByUser = true;
        TransitionToState(SendChatType.Endpoint);
    }

    public void FirstNpc()
    {
        TransitionToState(SendChatType.Chatting);

        string forFirstReply = "{ \"decision\" : \"yes\", \n\"yourReply\" : \"와! 물건 맞게 가져오셨네요. 거래 할게요~!\", \n\"persuasion\" : \"+3\", \n \"reason\" : \"물건이 마음에 듦\", \n\"emotion\" : \"best\"\n}";
        Reply.GptAnswer = forFirstReply;
    }

    public void ActivatePanel(SendChatType chatState, object additionalData = null, string name =null)
    {
        switch (chatState)
        {
            case SendChatType.ChatInit:
                if (additionalData is ItemInfo randItem)
                {
                    cityChattingUI.ShowPanel(chatState, randItem, name, isEndByUser); // show convo: npc name, npc item 룰렛
                    cityTabletData.UpdateItemData(randItem, ThisNpc.NpcID); // show tablet: npc name ~ npc want item
                }
                break;

            case SendChatType.Chatting:
                if (additionalData is ChattingState.GptResult gptResult)
                {
                    if(ThisNpc.NpcID!=0) NPCManager.Instance.curTalkingNPC.GetComponent<NPC>().PlayNPCAnimByEmotion(gptResult.emotion);
                    cityChattingUI.ShowPanel(chatState, gptResult);// reply도 보여주고, persuasion에 따른 reason에 대한 ++, -- 보여주기
                }
                break;

            case SendChatType.Endpoint:
                cityChattingUI.ShowPanel(chatState); // convo가 끝나 카메라가 돌아가고, end Panel 하나만 띄우기
                cityTabletData.UpdateEvaluationData(Eval.NpcEvalDict[ThisNpc.NpcID].summary, ThisNpc.NpcID, Eval.NpcEvalDict[ThisNpc.NpcID].isSuccess);
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
