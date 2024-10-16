using UnityEngine;
using static Define;

public class ItemInitState : ChatBaseState
{
    public override void Enter()
    {
        SubScribeAction();

        _sendChatType = SendChatType.ItemInit;
        Managers.Chat.ActivatePanel(_sendChatType);
    }

    public override void Exit()
    {
        UnSubScribeAction();
    }
    private void MakeAnswer(float userSuggest, ItemInfo itemInfo)
    {
        string _userSend = $"여기, {itemInfo.ObjName}입니다. {userSuggest}크레딧이에요.";

        string expensiveRate = Managers.Chat.RatePrice(userSuggest, itemInfo);
        string _initData = $"\n#Initial Values" 
         +$"\nThe thing vendor is selling to you: {itemInfo.ObjName}"
        + $"\nvendor First Suggest: {userSuggest} credit,"
        + $"Your First Suggest: {itemInfo.defaultPrice} credit"
        + $"yourOpinion About Vendor's First Suggest: {expensiveRate}";

        SendAnswer(_userSend, _initData);
    }

    private void SendAnswer(string _userSend, string _initData)
    {
        Managers.Chat.TransitionToState(SendChatType.ChatBargain);

        Debug.Log($"ItemInitState에서 보냄 {_sendChatType}");
        ServerManager.Instance.GetGPTReply(_userSend, SendChatType.ItemInit, _initData);
    }


    private void SubScribeAction()
    {
        EvalSubManager.OnItemInit -= MakeAnswer;
        EvalSubManager.OnItemInit += MakeAnswer;
    }
    private void UnSubScribeAction()
    {
        EvalSubManager.OnItemInit -= MakeAnswer;
    }
}
