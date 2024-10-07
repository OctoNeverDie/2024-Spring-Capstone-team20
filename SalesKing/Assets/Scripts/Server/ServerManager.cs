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
        //this.sendChatType = sendChatTypeFrom;
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

        Action<ResultInfo> bringGPTReplay = (result) =>
        {
            var resultData = JObject.Parse(result.Json)["reply"];
            var sendTypeData = JObject.Parse(result.Json)["sendType"].ToString();  // JSON에서 string 값 가져옴

            if (Enum.TryParse(sendTypeData, out SendChatType sendChatType))
            {
                Managers.Chat.TransitionToState(sendChatType);
            }
            else
            {
                Debug.Log("Failed to parse enum");
            }

            // 추가 코드
            Managers.Convo.ParseNPCAnswer($"{resultData}");
            VariableList.S_GptAnswer = resultData.ToString();
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
