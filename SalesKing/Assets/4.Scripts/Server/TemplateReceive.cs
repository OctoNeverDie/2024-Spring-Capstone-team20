using UnityEngine;

public class TemplateReceive : MonoBehaviour
{
    public void GetGptAnswer(string resultData, Define.SendChatType sendChatType)
    {
        if (sendChatType == Define.SendChatType.MuhanInit)
        {
            MuhanNpcDataManager.Instance.NpcsReceive(resultData);
        }
        else
        {
            ChatManager.Instance.Reply.GptAnswer = resultData;//이건 쌩
        }   
    }
}