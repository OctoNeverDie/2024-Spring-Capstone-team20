using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
using static Define;

public class ServerManager : ServerBase
{
    #region singleton
    private static ServerManager instance = null;
    public static ServerManager Instance => instance;

    private void Init()
    { templateReceive = Util.GetOrAddComponent<TemplateReceive>(this.gameObject); }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            Init();
        }
        else
        {
            Destroy(this.gameObject);
        }
        filePath = Path.Combine(Application.persistentDataPath, "chat_log.txt");

    }
    #endregion

    //loading panel
    public static event Action<bool> OnSendReplyUpdate;

    private TemplateReceive templateReceive;
    private string _userInput = "";
    private string _initData = "";
    private SendChatType _sendChatType;
    private string filePath;

    public void ShowPanel(bool isShow)
    {
        OnSendReplyUpdate?.Invoke(isShow);
    }
    public void GetGPTReply(string userInput, SendChatType sendChatTypeFrom, string initData = "")
    {
        _sendChatType = sendChatTypeFrom;
        _userInput = userInput;
        if (sendChatTypeFrom == SendChatType.ChatInit)
        {
            _initData = initData;
        }

        Debug.Log($"User답++++++++++{_userInput}, {_sendChatType}, {_initData}");
        ServerManager.Instance.SaveChat(_userInput);

        OnSendReplyUpdate?.Invoke(true);
        StartCoroutine(GetGPTCo());
    }

    private IEnumerator GetGPTCo()
    {
        yield return CoGetGPT();
    }

    private JObject AddJobjBySendType(JObject jobj, SendChatType sendChatType)
    {
        jobj.Add("Request", _userInput);
        jobj.Add("SendType", sendChatType.ToString());
        if (sendChatType == Define.SendChatType.ChatInit)
        {
            jobj.Add("NpcInit", _initData);
        }

        return jobj;
    }
    private Coroutine CoGetGPT(Action<ResultInfo> onSucceed = null,
                                Action<ResultInfo> onFailed = null,
                                Action<ResultInfo> onNetworkFailed = null)
    {
        string url = "http://127.0.0.1:8000/"; //"https://salesking-finalreal.azurewebsites.net/";//"https://salesking-final.azurewebsites.net/"; //"https://salesking-jbr.azurewebsites.net/"; //"https://salesai-ljy.azurewebsites.net/"//

        JObject jobj = new JObject();
        jobj = AddJobjBySendType(jobj, _sendChatType);

        Action<ResultInfo> bringGPTReply = (result) =>
        {
            string resultData = JObject.Parse(result.Json)["reply"].ToString();
            Debug.Log($"Gpt 답+++++++++++++++ {resultData}, {_sendChatType}");

            OnSendReplyUpdate?.Invoke(false);
            templateReceive.GetGptAnswer(resultData, _sendChatType);
            
        };

        Action<ResultInfo> failTest = (result) =>
        {
            Debug.Log("+++++++++fail");
        };

        Action<ResultInfo> networkTest = (result) =>//runserver!
        {
            Debug.Log("+++++++++networkTest");
        };

        onSucceed += bringGPTReply;
        onFailed += failTest;
        onNetworkFailed += networkTest;

        return StartCoroutine(SendRequest(url, SendType.POST, jobj, onSucceed, onFailed, onNetworkFailed));
    }

    public void SaveChat(string str)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true)) // Append 모드
            {
                writer.WriteLine(str);
            }
            Debug.Log($"'{str}'이(가) {filePath}에 저장되었습니다.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"파일 저장 중 오류 발생: {ex.Message}");
        }
    }
    

}
