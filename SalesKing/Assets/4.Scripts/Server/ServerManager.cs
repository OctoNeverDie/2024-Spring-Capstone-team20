using System;
using System.Collections;
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
    }
    #endregion

    //loading panel
    public static event Action<bool> OnSendReplyUpdate;

    private TemplateReceive templateReceive;
    private string _userInput = "";
    private string _initData = "";
    private SendChatType _sendChatType;
    private GameMode _gameMode;
    public void GetGPTReply(GameMode gameMode, string userInput, SendChatType sendChatTypeFrom, string initData = "")
    {
        _gameMode = gameMode;
        _sendChatType = sendChatTypeFrom;
        _userInput = userInput;
        if (sendChatTypeFrom == SendChatType.ChatInit)
        {
            _initData = initData;
        }

        Debug.Log($"User답++++++++++{_userInput}, {_sendChatType}, {_initData}");
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
        string url = "https://salesking-jbr.azurewebsites.net/"; //"http://127.0.0.1:8000/"; //"https://salesai-ljy.azurewebsites.net/"//"https://salesai-jsy333.azurewebsites.net/";//"https://salesai-jsy2.azurewebsites.net/";//

        JObject jobj = new JObject();
        jobj = AddJobjBySendType(jobj, _sendChatType);

        Action<ResultInfo> bringGPTReply = (result) =>
        {
            if (_gameMode == GameMode.Story)
            {
                string resultData = JObject.Parse(result.Json)["reply"].ToString();
                Debug.Log($"Gpt 답+++++++++++++++ {resultData}, {_sendChatType}");

                OnSendReplyUpdate?.Invoke(false);
                templateReceive.GetGptAnswer(resultData, _sendChatType);
            }
            else if (_gameMode == GameMode.Infinity)
            {
                string[] resultData = new string[3];
                resultData[0] = JObject.Parse(result.Json)["npc1"].ToString();
                resultData[1] = JObject.Parse(result.Json)["npc2"].ToString();
                resultData[2] = JObject.Parse(result.Json)["npc3"].ToString();

                OnSendReplyUpdate?.Invoke(false);
                templateReceive.GetGptAnswer(resultData, _sendChatType);
            }
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
