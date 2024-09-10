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

    thought: 티타늄으로 만들어졌다니 이 펜은 정말 유용할 것 같아. 그래도 혹시 모르니까 진짜인지 물어봐야겠어., 
    reason: 물건이 좋은 재료로 만들어졌음. (usefulness: +1), 펜촉이 얇아서 필기가 잘될 것 같음. (usefulness: +1), 
    emotion: 중립, 
    suggestedPrice: ?, 
    reaction: 와! 티타늄으로 만들어졌다는게 사실이라면 쓸모가 있을 것 같아요. 게다가 펜촉이 얇아서 필기도 잘 될 것 같고요. 정말 티타늄으로 만들어진 게 맞나요?
 */
            //templateReceive(resultData);
        };

        Action<ResultInfo> failTest = (result) =>
        {
            Debug.Log("+++++++++fail");
        };

        Action<ResultInfo> networkTest = (result) =>
        {
            //runserver 안 했을 때
            Debug.Log("+++++++++networkTest");
        };

        onSucceed += bringGPTReplay;
        onFailed += failTest;
        onNetworkFailed += networkTest;

        return StartCoroutine(SendRequest(url, SendType.POST, jobj, onSucceed, onFailed, onNetworkFailed));
    }
}
