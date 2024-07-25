using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ServerManager : ServerBase
{
    private static ServerManager instance = null;
    public static ServerManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
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

    private Coroutine CoGetGPT( Action<ResultInfo> onSucceed = null,
                                Action<ResultInfo> onFailed = null,
                                Action<ResultInfo> onNetworkFailed = null)
    {
        string url = "http://127.0.0.1:8000/";

        JObject jobj = new JObject();
        jobj.Add("request", _userInput);

        Action<ResultInfo> bringGPTReplay = (result) =>
        {
            Debug.Log($"+++++++++Reply");
            var resultData = JObject.Parse(result.Json)["reply"];
            Debug.Log($"Reply : {resultData}");
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

        return StartCoroutine(SendRequest(url, SendType.GET, jobj, onSucceed, onFailed, onNetworkFailed));
    }
}
