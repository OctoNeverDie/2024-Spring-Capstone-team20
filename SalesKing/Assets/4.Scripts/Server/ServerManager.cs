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
    #endregion

    //loading panel
    public static event Action<bool> OnSendReplyUpdate;

    private TemplateReceive templateReceive;
    private string _userInput = "";
    private string _initData = "";
    private SendChatType _sendChatType;
    public void GetGPTReply(string userInput, SendChatType sendChatTypeFrom, string initData="")
    { 
        this._sendChatType = sendChatTypeFrom;
        this._userInput = userInput;
        if (sendChatTypeFrom == SendChatType.ChatInit)
        {
            this._initData = initData;
        }

        Debug.Log($"User답++++++++++{_userInput}, {_sendChatType}, {_initData}");
    //    string str = "{\n" +
    //"  \"decision\": \"wait\",\n" +
    //"  \"yourReply\": \"판매자의 설명에 따라 더 질문하거나 반응하세요. 제시된 성격을 반영하여 반응하세요. 아직 확신이 없는 상태입니다.\",\n" +
    //"  \"persuasion\": -3,\n" +
    //"  \"reason\": \"왜 해당 persuasion 점수를 출력했는지 설명해줘. 키워드식으로 짧게 써줘. ex. 연민을 느낌, 불확실한 설명, ~~한 키워드가 마음에 듦 등등\",\n" +
    //"  \"emotion\": \"worst\"\n" +
    //"}";

    //    templateReceive.GetGptAnswer(str, _sendChatType);
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
    private Coroutine CoGetGPT( Action<ResultInfo> onSucceed = null,
                                Action<ResultInfo> onFailed = null,
                                Action<ResultInfo> onNetworkFailed = null)
    {
        string url = "http://127.0.0.1:8000/"; //"https://salesai-khm.azurewebsites.net/"; //"https://salesai-ljy.azurewebsites.net/"//"https://salesai-jsy333.azurewebsites.net/";//"https://salesai-jsy2.azurewebsites.net/";//

        JObject jobj = new JObject();
        jobj = AddJobjBySendType(jobj, _sendChatType);

        Action<ResultInfo> bringGPTReply = (result) =>
        {
            var resultData = JObject.Parse(result.Json)["reply"].ToString();
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
}
