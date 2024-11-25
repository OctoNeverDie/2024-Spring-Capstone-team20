using UnityEngine;

public class TemplateReceive : MonoBehaviour
{
    public void GetGptAnswer(string resultData, Define.SendChatType sendChatType)
    {
        ChatManager.Instance.Reply.GptAnswer = resultData;//이건 쌩
    }
}