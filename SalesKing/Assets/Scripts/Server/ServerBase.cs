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
    public delegate void RequestCallback(ResultInfo result);

    //sent to the server
    protected virtual IEnumerator SendRequest(string url, SendType sendType, JObject jobj, Action<ResultInfo> onSucceed, Action<ResultInfo> onFailed, Action<ResultInfo> onNetworkFailed)
    {
        //check network connection
        yield return StartCoroutine(CheckNextwork());

        using (var req = new UnityWebRequest(url, sendType.ToString()))
        {
            //check sending information
            Debug.LogFormat("url: {0} \n" +
            "보낸데이터: {1}",
            url,
            JsonConvert.SerializeObject(jobj, Formatting.Indented));

            //make request body
            byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jobj));
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            //make request header
            req.SetRequestHeader("Content-Type", "application/json");

            //sending
            yield return req.SendWebRequest();

            //check success/fail
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
            case UnityWebRequest.Result.InProgress://possibly network error
                res = new ResultInfo(req.downloadHandler.text, false, true, "InProgress");
                return res;
            case UnityWebRequest.Result.Success:
                JObject jobj = JObject.Parse(req.downloadHandler.text);

                //if server's "meta code" data is not 0 -> fail
                //bool isSuccess = int.Parse(jobj["meta"]["code"].ToString()) == 0 ? true : false;
                bool isSuccess = true;

                if (isSuccess)
                {
                    res = new ResultInfo(req.downloadHandler.text, true, false, string.Empty);
                    return res;
                }
                else //fail
                {
                    Debug.LogErrorFormat("요청 실패: {0}", jobj["message"].ToString());
                    res = new ResultInfo(req.downloadHandler.text, false, false,
                        string.Format("Code: {0} - {1}", jobj["code"].ToString(), jobj["message"].ToString()));
                    return res;
                }
            #region Connection Error Handle
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.ProtocolError:
            case UnityWebRequest.Result.DataProcessingError:
                // Connection error, Runserver 안 함
                Debug.LogError(req.error);
                Debug.Log(req.downloadHandler.text);
                res = new ResultInfo(req.downloadHandler.text, false, true, req.error);
                return res;
            default:
                Debug.LogError(req.error);
                Debug.Log(req.downloadHandler.text);
                res = new ResultInfo(req.downloadHandler.text, false, true, "Unknown");
                return res;
             #endregion
        }
    }

    //Hold the result from web
    public class ResultInfo
    {
        private string json; //hold return value from web
        private bool isSuccess;
        private bool isNetworkError;
        private string error;

        public string Json => json;
        public bool IsSuccess => isSuccess;
        public bool IsNetworkError => isNetworkError;
        public string Error => error;

        public ResultInfo(string json, bool isSuccess, bool isNetworkError, string error)
        {
            this.json = json;
            this.isSuccess = isSuccess;
            this.isNetworkError = isNetworkError;
            this.error = error;
        }
    }

}
