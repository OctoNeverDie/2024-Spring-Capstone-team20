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

    private void Init()
    {

        templateReceive = Util.GetOrAddComponent<TemplateReceive>(this.gameObject);

    }
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

    private string _userInput = "";
    public void GetGPTReply(string userInput) 
    {
        this._userInput = userInput;
        StartCoroutine(GetGPTCo());
    }
    private IEnumerator GetGPTCo()
    {
        yield return CoGetGPT();
    }

    public enum TestSendType
    { 
        Init,
        Chat,
        Clear
    }
    private Coroutine CoGetGPT( Action<ResultInfo> onSucceed = null,
                                Action<ResultInfo> onFailed = null,
                                Action<ResultInfo> onNetworkFailed = null)
    {
        string url = "http://127.0.0.1:8000/";

        JObject jobj = new JObject();
        jobj.Add("Request", _userInput);
        jobj.Add("SendType", $"{TestSendType.Init}");
        jobj.Add("Item", "@ObjName = Cup, @ObjectInfo = blah, @defaultPrice =10, @expensvie = 100, @tooExpensive =200");
        jobj.Add("Npc", "@NpcName = Jack, @NpcSex = female, @NpcAge = 17, @NpcPersonality = Bad, @NpcProplemType = relate, @NpcProblemInfo = blah");

        Action<ResultInfo> bringGPTReplay = (result) =>
        {
            Debug.Log($"+++++++++Reply");
            var resultData = JObject.Parse(result.Json)["reply"];
            Debug.Log($"Reply : {resultData}");
            templateReceive.StringConcat(resultData.ToString());
            /*
            JObject jsonObject = JObject.Parse(result.Json);

            JToken resultData1 = jsonObject["reply"];
            JToken resultData2 = jsonObject["thought"];
            JToken resultData3 = jsonObject["reason"];
            JToken resultData4 = jsonObject["emotion"];
            JToken resultData5 = jsonObject["suggestedPrice"];
            JToken resultData6 = jsonObject["reaction"];

            Debug.Log($"Reply: {(resultData1 != null ? resultData1.ToString() : "null")}");
            Debug.Log($"Thought: {(resultData2 != null ? resultData2.ToString() : "null")}");
            Debug.Log($"Reason: {(resultData3 != null ? resultData3.ToString() : "null")}");
            Debug.Log($"Emotion: {(resultData4 != null ? resultData4.ToString() : "null")}");
            Debug.Log($"Suggested Price: {(resultData5 != null ? resultData5.ToString() : "null")}");
            Debug.Log($"Reaction: {(resultData6 != null ? resultData6.ToString() : "null")}");

    thought: ƼŸ������ ��������ٴ� �� ���� ���� ������ �� ����. �׷��� Ȥ�� �𸣴ϱ� ��¥���� ������߰ھ�., 
    reason: ������ ���� ���� ���������. (usefulness: +1), ������ ��Ƽ� �ʱⰡ �ߵ� �� ����. (usefulness: +1), 
    emotion: �߸�, 
    suggestedPrice: ?, 
    reaction: ��! ƼŸ������ ��������ٴ°� ����̶�� ���� ���� �� ���ƿ�. �Դٰ� ������ ��Ƽ� �ʱ⵵ �� �� �� �����. ���� ƼŸ������ ������� �� �³���?
 */
            //templateReceive(resultData);
        };

        Action<ResultInfo> failTest = (result) =>
        {
            Debug.Log("+++++++++fail");
        };

        Action<ResultInfo> networkTest = (result) =>
        {
            //runserver �� ���� ��
            Debug.Log("+++++++++networkTest");
        };

        onSucceed += bringGPTReplay;
        onFailed += failTest;
        onNetworkFailed += networkTest;

        return StartCoroutine(SendRequest(url, SendType.POST, jobj, onSucceed, onFailed, onNetworkFailed));
    }
}
