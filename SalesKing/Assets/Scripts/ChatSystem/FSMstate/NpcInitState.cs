using UnityEngine;
using static Define;

public class NpcInitState : ChatBaseState
{
    //말을 건다 버튼 클릭하면 일어남.
    public override void Enter()
    {
        InitData();
    }

    public override void Exit()
    {
    }

    private void InitData()
    {
        _sendChatType = SendChatType.ChatInit;

        Managers.Chat.Clear();
        InitData initData = Managers.Chat.dailyInitData.getInitData();
        NpcInfo npc = Managers.Data.npcList[initData.npcID];
        Managers.Chat.EvalManager.InitNpcDict(initData, npc);

        string _userSend = MakeUserSend(npc);
        string[] _mbtis = MakeMbtiSend(initData.mbtiPrefers);

        Debug.Log($"NpcInitState에서 보냄 {_sendChatType}. {_userSend}, {_mbtis}");
        ServerManager.Instance.GetGPTReply("", _sendChatType, _userSend, _mbtis);
    }

    private string[] MakeMbtiSend(Define.Prefer[] mbtiPrefers)
    {
        string[] mbtis = new string[4];
        for (int i = 0; i < mbtiPrefers.Length; i++)
        {
            mbtis[i] = mbtiPrefers[i].ToString();
        }
        return mbtis;
    }

    private string MakeUserSend(NpcInfo npc)
    {
        var eval = Managers.Chat.EvalManager._npcEvaluation;

        string user_send = $"\n NpcName : {eval.npcName}, NpcSex : {eval.npcSex}, NpcAge : {eval.npcAge} "
            + $" KeyWord : {eval.npcKeyword}, \nPersonailty : {npc.Personality}\nDialogue Style: {npc.DialogueStyle}\nExample: {npc.Example}"
            + $"당근에 올린 글: {eval.concern}\n네가 사려고 한 물건: {eval.wantItemName}, 판매자가 가져온 물건: {eval.boughtItemName} ";

        return user_send;
    }
}
