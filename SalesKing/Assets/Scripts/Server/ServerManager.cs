using System;
using System.Collections;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using static Define;

public class ServerManager : ServerBase
{
    private static ServerManager instance = null;
    public static ServerManager Instance => instance;

    private TemplateReceive templateReceive;
    private string _userInput = "";
    private SendChatType _sendChatType;
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

    public void GetGPTReply(string userInput, SendChatType sendChatTypeFrom)
    {
        if (_sendChatType == SendChatType.Endpoint && sendChatTypeFrom != SendChatType.NpcInit)
            return;

        Debug.Log($"User답++++++++++{userInput}, {sendChatTypeFrom.ToString()}");
        this._sendChatType = sendChatTypeFrom;
        this._userInput = userInput;


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
        
        return jobj;
    }
    private Coroutine CoGetGPT( Action<ResultInfo> onSucceed = null,
                                Action<ResultInfo> onFailed = null,
                                Action<ResultInfo> onNetworkFailed = null)
    {
        string url = "https://salesai-jsy2.azurewebsites.net/"; //"http://127.0.0.1:8000/";

        JObject jobj = new JObject();
        jobj = AddJobjBySendType(jobj, _sendChatType);

        Action<ResultInfo> bringGPTReply = (result) =>
        {
            var resultData = JObject.Parse(result.Json)["reply"].ToString();
            //var sendTypeData = JObject.Parse(result.Json)["sendType"].ToString();  // JSON에서 string 값 가져옴
            Debug.Log($"Gpt 답+++++++++++++++ {resultData}, {_sendChatType.ToString()}");
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
}
