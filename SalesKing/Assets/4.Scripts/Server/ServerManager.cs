using System;
using System.Threading.Tasks;
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

    public async void GetGPTReply(GameMode gameMode, string userInput, SendChatType sendChatTypeFrom, string initData="")
    { 
        _gameMode = gameMode;
        _sendChatType = sendChatTypeFrom;
        _userInput = userInput;
        if (sendChatTypeFrom == SendChatType.ChatInit)
        {
            _initData = initData;
        }
        else 
        {
            _initData = "";
        }

        Debug.Log($"User답++++++++++{_userInput}, {_sendChatType}, {_initData}");
    
        if(gameMode == GameMode.Story) 
            OnSendReplyUpdate?.Invoke(true);

        await GetGPTAsync();
    }

    private async Task GetGPTAsync()
    {
        await CoGetGPT();
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
    private async Task CoGetGPT()
    {
        string url = "http://127.0.0.1:8000/"; //"https://salesai-khm.azurewebsites.net/"; //"https://salesai-ljy.azurewebsites.net/"//"https://salesai-jsy333.azurewebsites.net/";//"https://salesai-jsy2.azurewebsites.net/";//

        JObject jobj = new JObject();
        jobj = AddJobjBySendType(jobj, _sendChatType);

        while (true) 
        {
            ResultInfo result = await SendRequest(url, SendType.POST, jobj);

            if (result.IsNetworkError)
            {
                Debug.Log("+++++++++networkTest");
                await Task.Delay(120000); // 재시도 간격 조절
                continue; // 재시도
            }
            else if (result.IsSuccess)
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
                    Debug.Log($"Gpt 답+++++++++++++++ {resultData[0]}, {_sendChatType}");

                    templateReceive.GetGptAnswer(resultData, _sendChatType);
                }
                break; // 성공 시 루프 종료
            }
            else
            {
                Debug.Log("+++++++++fail");
                break; // 실패 시 루프 종료
            }
        }
    }
}
