using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NpcInitState : ChatBaseState
{
    int NpcID;

    //말을 거시겠습니까? 클릭하면 일어남.
    public override void Enter()
    {
        Managers.Chat.Clear();

        NpcInfo npc= Managers.Chat.npcSupplyManager.GetNextNpc();

        string _userSend= MakeAnswer(npc);

        Debug.Log($"NPC"+
          $"ID = {npc.NpcID}, " +
          $"Name = {npc.NpcName}, " +
          $"Sex = {npc.NpcSex}, " +
          $"Age = {npc.NpcAge}, " +
          $"Situation = {npc.Situation_Description}, " +
          $"Personality = {npc.Personality}, " +
          $"Dialogue Style = {npc.Dialogue_Style}");
        Debug.Log($"{npc.NpcSex == "female"}");
        Managers.Chat.EvalManager.InitNpcDict(npc.NpcID, npc.NpcName, npc.NpcAge, npc.NpcSex == "female");

        _sendChatType = SendChatType.NpcInit;

        Debug.Log($"NpcInitState에서 보냄 {_sendChatType}");
        ServerManager.Instance.GetGPTReply(_userSend, _sendChatType);
    }

    protected string MakeAnswer(NpcInfo user_send)
    {
        string initData = $"\n Name: {user_send.NpcName}, Age : {user_send.NpcAge}, Sex : {user_send.NpcSex}\n"
            + $"Situation_Description: {user_send.Situation_Description}\n"
            + $"Personality: {user_send.Personality}\n"
            + $"Dialogue_Style: {user_send.Dialogue_Style}.\n";

        return initData;
    }
}
