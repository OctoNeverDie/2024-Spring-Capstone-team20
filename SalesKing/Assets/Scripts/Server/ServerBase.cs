using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class ServerBase : MonoBehaviour
{
    public enum SendType { GET, POST, PUT ,DELETE }
    //public delegate void RequestCallback(ResultInfo result);

    //sent to the server
    protected virtual IEnumerator SendRequest(string url, SendType sendType, JObject jobj, Action<ResultInfo> onSucceed, Action<ResultInfo> onFailed, Action<ResultInfo> onNetworkFailed)
    {
        //check network connection
        yield return StartCoroutine(CheckNextwork());

        using (var req = new UnityWebRequest(url, sendType.ToString()))
        {
            //check sending information
            //Debug.LogFormat("Sended Data: {1}", url, JsonConvert.SerializeObject(jobj, Formatting.Indented));

            //make request body and header
            byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jobj));
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            //check result success/fail
            var result = ResultCheck(req);

            //if network error
            if (result.IsNetworkError)
            {
                onNetworkFailed?.Invoke(result);

                // TODO: 네트워크 재시도 팝업 호출.

                yield return new WaitForSeconds(1f);
                Debug.LogError("재시도");
                yield return StartCoroutine(SendRequest(url, sendType, jobj, onSucceed, onFailed, onNetworkFailed));
            }
            else
            {
                if (result.IsSuccess)
                {
                    onSucceed?.Invoke(result);
                }
                else //fail from server
                { 
                    onFailed?.Invoke(result);
                }
            }
        }
    }

    protected virtual IEnumerator CheckNextwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //TODO : pop up - network error
            Debug.LogError("Network not working!");

            yield return new WaitUntil(() => Application.internetReachability != NetworkReachability.NotReachable);
            Debug.Log("Network reconnected");
        }
    }

    protected virtual ResultInfo ResultCheck(UnityWebRequest req)
    {
        ResultInfo res;
        switch (req.result)
        {
            case UnityWebRequest.Result.Success:
                JObject jobj = JObject.Parse(req.downloadHandler.text);

                res = new ResultInfo(req.downloadHandler.text, true, false, string.Empty);
                return res;

            case UnityWebRequest.Result.InProgress://possibly network error
                res = new ResultInfo(req.downloadHandler.text, false, true, "InProgress");
                return res;
            #region Connection Error Handle
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.ProtocolError:
            case UnityWebRequest.Result.DataProcessingError:// Connection error, Runserver 안 함
            default:
                Debug.Log(req.downloadHandler.text);
                res = new ResultInfo(req.downloadHandler.text, false, true, req.error);
                return res;
             #endregion
        }
    }

    public class ResultInfo //Hold the result from web
    {
        public string Json { get; private set; }//hold return value from web
        public bool IsSuccess { get; private set; } 
        public bool IsNetworkError { get; private set; }
        public string Error { get; private set; } 

        public ResultInfo(string json, bool isSuccess, bool isNetworkError, string error)
        {
            this.Json = json;
            this.IsSuccess = isSuccess;
            this.IsNetworkError = isNetworkError;
            this.Error = error;
        }
    }
}
