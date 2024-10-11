using UnityEngine;
using static Define;

public class DemoState : ChatBaseState
{
    public override void Enter()
    {
        NpcInfo npc = Managers.Chat.npcSupplyManager.GetNextNpc();
        string _userSend = MakeAnswer(npc);
        Managers.Chat.EvalManager.InitNpcDict(npc.NpcID, npc.NpcName, npc.NpcAge, npc.NpcSex == "female");

        EvalSubManager.OnItemInit -= MakeAnswer;
        EvalSubManager.OnItemInit += MakeAnswer;

        _sendChatType = SendChatType.ItemInit;
        Managers.Chat.ActivatePanel(_sendChatType);
    }

    protected string MakeAnswer(NpcInfo user_send)
    {
        string initData = $"\n Name: {user_send.NpcName}, Age : {user_send.NpcAge}, Sex : {user_send.NpcSex}\n"
            + $"Situation_Description: {user_send.Situation_Description}\n"
            + $"Personality: {user_send.Personality}\n"
            + $"Dialogue_Style: {user_send.Dialogue_Style}." 
            +$"System : You first have to talk about your situation in korean\n";

        return initData;
    }

    public override void Exit()
    {
        EvalSubManager.OnItemInit -= MakeAnswer;
    }

    private void SendAnswer(string _userSend)
    {
        Debug.Log($"ItemInitState에서 보냄 {_sendChatType}");
        ServerManager.Instance.GetGPTReply(_userSend, _sendChatType);
    }

    private void MakeAnswer(float userSuggest, ItemInfo itemInfo)
    {
        string expensiveRate = Managers.Chat.RatePrice(userSuggest, itemInfo);

        string _userSend = $"\nThe thing you want to buy: {Managers.Chat.EvalManager.ThingToBuy}"
        + $"\nThe thing vendor is selling to you: {itemInfo.ObjName}"
        + $"\nvendor First Suggest: {userSuggest} credit,"
        + $"Your First Suggest: {itemInfo.defaultPrice} credit"
        + $"yourOpinion: {expensiveRate}";

        SendAnswer(_userSend);
    }
}
