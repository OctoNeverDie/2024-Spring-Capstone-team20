using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ServerManager : ServerBase
{
    private static ServerManager instance = null;
    public static ServerManager Instance => instance;

    private TemplateReceive templateReceive;
    private string _userInput = "";
    private TemplateSend.SendChatType sendChatType;
    private void Init()
    { templateReceive = Util.GetOrAddComponent<TemplateReceive>(this.gameObject);  }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            Init();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void GetGPTReply(string userInput, TemplateSend.SendChatType sendChatTypeFrom)
    {
        this.sendChatType = sendChatTypeFrom;
        this._userInput = userInput;

        StartCoroutine(GetGPTCo());
    }

    private IEnumerator GetGPTCo()
    {
        yield return CoGetGPT();
    }

    private JObject AddJobjBySendType(JObject jobj, TemplateSend.SendChatType sendChatType)
    {
        jobj.Add("Request", _userInput);
        jobj.Add("SendType", $"{sendChatType}");
        
        return jobj;
    }
    private Coroutine CoGetGPT( Action<ResultInfo> onSucceed = null,
                                Action<ResultInfo> onFailed = null,
                                Action<ResultInfo> onNetworkFailed = null)
    {
        string url = "https://salesai-jsy2.azurewebsites.net/"; //"http://127.0.0.1:8000/";

        JObject jobj = new JObject();
        jobj = AddJobjBySendType(jobj, sendChatType);

        Action<ResultInfo> bringGPTReplay = (result) =>
        {
            var resultData = JObject.Parse(result.Json)["reply"];
            Debug.Log($"Reply : {resultData}");
            templateReceive.StringConcat(resultData.ToString());
        };

        Action<ResultInfo> failTest = (result) =>
        {
            Debug.Log("+++++++++fail");
        };

        Action<ResultInfo> networkTest = (result) =>//runserver!
        {
            Debug.Log("+++++++++networkTest");
        };

        onSucceed += bringGPTReplay;
        onFailed += failTest;
        onNetworkFailed += networkTest;

        return StartCoroutine(SendRequest(url, SendType.POST, jobj, onSucceed, onFailed, onNetworkFailed));
    }
}
