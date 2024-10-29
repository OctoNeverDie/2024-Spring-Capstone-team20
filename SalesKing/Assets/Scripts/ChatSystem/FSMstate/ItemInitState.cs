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
        string _userSend = $"여기, {itemInfo.ObjName}입니다. {userSuggest}크레딧이에요. 관련해서, 당신의 고민을 더 자세히 들을 수 있을까요? 이 물건이 당신의 고민을 해결해줄지도 모르거든요!";

        string expensiveRate = Managers.Chat.RatePrice(userSuggest, itemInfo);
        string _initData = $"\n#Initial Values" 
         +$"\nThe thing vendor is selling to you: {itemInfo.ObjName}"
        + $"\nvendor First Suggest: {userSuggest} credit,"
        + $"Your First Suggest: {itemInfo.defaultPrice} credit";

        Debug.Log("For Test -------------");
        _userSend = "$start";
        _initData = "네가 사려고 한 물건: 케이크, 판매자가 가져온 물건: 캣타워";

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
