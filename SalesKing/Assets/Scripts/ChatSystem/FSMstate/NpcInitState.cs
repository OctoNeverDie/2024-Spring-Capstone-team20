using System;
using System.Collections;
using UnityEngine;
using static Define;

public class NpcInitState : ChatBaseState
{
    //말을 건다 버튼 클릭하면 일어남.
    public override void Enter()
    {
        SubScribeAction();
        InitData();
    }

    public override void Exit()
    {
        UnSubScribeAction();
    }

    private void InitData()
    {
        Managers.Chat.Clear();

        Debug.Log($"{Managers.Chat.EvalManager.currentNpcId-1}. { Managers.Data.npcList[Managers.Chat.EvalManager.currentNpcId - 1].NpcName}");
        NpcInfo npc = Managers.Data.npcList[Managers.Chat.EvalManager.currentNpcId -1];
        //NpcInfo npc = Managers.Chat.npcSupplyManager.GetNextNpc();
        string _userSend = MakeAnswer(npc);
        Managers.Chat.EvalManager.InitNpcDict(npc.NpcID, npc.NpcName, npc.NpcAge, npc.NpcSex == "female", npc.KeyWord);
        _sendChatType = SendChatType.NpcInit;

        Debug.Log("For Test -------------");
        _userSend = @"
        ""NpcID"": 2,
        ""NpcName"": ""곽제리"",
        ""NpcSex"": ""female"",
        ""NpcAge"": 22,
        ""KeyWord"": ""#발랄한, #긍정적인"",
        ""당근에 올린 글"": ""시험을 망쳐서 요즘 기분이 꿀꿀해용 ㅠㅠ 달달한 거 먹고 싶은데 뭐 없을까요?"",
        ""Personality"": ""너는 긍정적이고 활발한 성격을 가지고 있어. 사람들과 소통하는 것을 좋아하고, 언제나 열린 마음으로 대화를 시도해. 다른 사람의 의견을 존중하며, 소통 중에 자연스럽게 자신의 생각을 공유하는 것을 선호해. 실수나 비판에 대해 크게 개의치 않으며, 유머 감각이 뛰어나 대화를 유쾌하게 이끌어 나가는 편이야. 새로운 경험에 대한 호기심이 많아, 상인과의 대화에서 흥미를 느끼고 쉽게 마음을 열어."",
        ""Dialogue Style"": ""일반적으로 친근하고 정중한 표현을 사용해. 모든 문장에 ~하네용. ~해용. 식으로 뒤에 ㅇ을 붙이는 편이야. 대화가 어색해지지 않도록 자주 유머를 섞고, 상대방의 이야기에 귀 기울이며 반응을 잘 보인다. 물건을 살 기회가 생기면, 기분이 좋고 흥미롭게 반응해볼 수 있어. 재밌네용! 같은 말을 자주 하며, 솔직하게 자신의 생각을 전달하는 것을 중요시해. 넌 발랄한 성격이기 때문에 감탄사도 많이 써."",
        ""Example"": ""앗! 그건 힘들 것 같네용. 그래도 응원은 할게용~!, 우와~! 너무너무 좋아용! 딱 제 맘에 드네용~!"",
        ""특이사항"": ""없음""
        ";

        Debug.Log($"NpcInitState에서 보냄 {_sendChatType}. {_userSend}");
        ServerManager.Instance.GetGPTReply("", _sendChatType, _userSend);
    }

    private void GptOutput(string type, string gpt_output)
    {
        if (type != nameof(Managers.Chat.ReplyManager.GptAnswer))
            return;

        //요즘 내 고민은 이거에요... 하고 답이 오면 inventory 보여주기, ConvoUI에 있다.
        Managers.Chat.ActivatePanel(_sendChatType);
    }

    protected string MakeAnswer(NpcInfo user_send)
    {
        string initData = $"\n Name: {user_send.NpcName}, Age : {user_send.NpcAge}, Sex : {user_send.NpcSex}, Keyword : {user_send.KeyWord}\n"
            + $"Situation_Description: {user_send.SituationDescription}\n"
            + $"Personality: {user_send.Personality}\n"
            + $"Dialogue_Style: {user_send.DialogueStyle}.\n"
            +$"Example : {user_send.Example}";

        return initData;
    }

    private void SubScribeAction()
    {
        ReplySubManager.OnReplyUpdated -= GptOutput;
        ReplySubManager.OnReplyUpdated += GptOutput;
    }
    private void UnSubScribeAction()
    {
        ReplySubManager.OnReplyUpdated -= GptOutput;
    }
}
