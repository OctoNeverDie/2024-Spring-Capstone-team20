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
