using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
using static Define;

public class ServerManager : ServerBase
{
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

    //loading panel
    public static event Action<bool> OnSendReplyUpdate;

    private TemplateReceive templateReceive;
    private string _userInput = "";
    private string _initData = "";
    private SendChatType _sendChatType;
    
    public void GetGPTReply(string userInput, SendChatType sendChatTypeFrom, string initData = "")
    {
        SaveToJson("UserInput", userInput);
        SaveToJson("ChatType", sendChatTypeFrom.ToString());

        this._sendChatType = sendChatTypeFrom;
        this._userInput = userInput;
        this._initData = initData;

        Debug.Log($"User답++++++++++{_userInput}, {_sendChatType}, {_initData}");
        ServerManager.OnSendReplyUpdate?.Invoke(true);
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
        jobj.Add("DataInit", _initData);
        
        return jobj;
    }
    private Coroutine CoGetGPT( Action<ResultInfo> onSucceed = null,
                                Action<ResultInfo> onFailed = null,
                                Action<ResultInfo> onNetworkFailed = null)
    {
        string url = "http://127.0.0.1:8000/";  //"https://salesai-khm.azurewebsites.net/"; //"https://salesai-ljy.azurewebsites.net/"//"https://salesai-jsy333.azurewebsites.net/";//"https://salesai-jsy2.azurewebsites.net/";

        JObject jobj = new JObject();
        jobj = AddJobjBySendType(jobj, _sendChatType);

        Action<ResultInfo> bringGPTReply = (result) =>
        {
            var resultData = JObject.Parse(result.Json)["reply"].ToString();
            Debug.Log($"Gpt 답+++++++++++++++ {resultData}, {_sendChatType}");

             // 공통 함수 호출하여 GPT 답변 저장
            SaveToJson("GptAnswer", resultData);
            SaveToJson("ChatType", _sendChatType.ToString());

            ServerManager.OnSendReplyUpdate?.Invoke(false);
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

    private void SaveToJson(string key, string value)
    {
        // 파일 경로 설정 (예: Application.persistentDataPath)
        string filePath = Path.Combine(Application.persistentDataPath, "ChatLog.json");


        // 기존 JSON 파일이 있으면 불러옴
        JObject logData = new JObject();
        if (System.IO.File.Exists(filePath))
        {
            var existingData = System.IO.File.ReadAllText(filePath);
            logData = JObject.Parse(existingData);
        }

        // 새로운 데이터를 추가
        logData[key] = value;

        // 파일에 쓰기
        System.IO.File.WriteAllText(filePath, logData.ToString());
    }
}
