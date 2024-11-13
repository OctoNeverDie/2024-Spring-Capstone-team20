using UnityEngine;

public class TemplateReceive : MonoBehaviour
{
    public void GetGptAnswer(string resultData, Define.SendChatType sendChatType)
    {
        if (sendChatType == Define.SendChatType.ChatInit)
        {
            ChatManager.Instance.TransitionToState(Define.SendChatType.Chatting);
        }
        ChatManager.Instance.Reply.GptAnswer = resultData;//이건 쌩
    }
}