using System;
using UnityEngine;
using static Define;


public class ChatSaleState : ChatBaseState
{
    private struct GptResult
    {
        public string _reaction;
        public string _thingToBuy;
        public bool _isYesNo;//yes나 no가 있나?
        public bool _yesIsTrue;//있다면, yes인가?
        public string _evaluation;
    }

    GptResult _gptResult = new GptResult();

    public override void Enter()
    {
        _sendChatType = SendChatType.ChatSale;
        //맨 처음 시작할 때, convo ui 나와야한다.
        Managers.Chat.ActivatePanel(_sendChatType);

        string user_input = "요즘 당신의 고민에 대해서 말씀해보세요.";
        ServerManager.Instance.GetGPTReply(user_input, _sendChatType);
        SubScribeAction();
    }

    public static Action popupBtnInventory;
    public void GptOutput(string type, string gpt_output)
    {
        if (type != nameof(Managers.Chat.ReplyManager.GptAnswer))
            return;

        //요즘 내 고민은 이거에요... 하고 답이 오면 inventory 보여주기
        popupBtnInventory?.Invoke();
    }

    public override void Exit()
    {
        UnSubScribeAction();
        _gptResult = new GptResult();
        //save evaluation
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
