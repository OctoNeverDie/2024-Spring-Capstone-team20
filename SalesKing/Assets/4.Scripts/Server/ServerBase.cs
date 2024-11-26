using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class ServerBase : MonoBehaviour
{
    public enum SendType { GET, POST, PUT ,DELETE }

    //sent to the server
    protected virtual async Task<ResultInfo> SendRequest(string url, SendType sendType, JObject jobj)
    {
        //check network connection
        await CheckNetwork();

        using (var req = new UnityWebRequest(url, sendType.ToString()))
        {
            //make request body and header
            byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jobj));
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            var operation = req.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            //check result success/fail
            var result = ResultCheck(req);

            return result;
        }
    }

    protected virtual async Task CheckNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //TODO : pop up - network error
            Debug.LogError("Network not working!");

            while (Application.internetReachability == NetworkReachability.NotReachable)
            {
                await Task.Delay(500); // 0.5초마다 체크
            }

            Debug.Log("Network reconnected");
        }
    }

    protected virtual ResultInfo ResultCheck(UnityWebRequest req)
    {
        ResultInfo res;
        switch (req.result)
        {
            case UnityWebRequest.Result.Success:
                res = new ResultInfo(req.downloadHandler.text, true, false, string.Empty);
                return res;

            case UnityWebRequest.Result.InProgress: // 네트워크 오류 가능성
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
