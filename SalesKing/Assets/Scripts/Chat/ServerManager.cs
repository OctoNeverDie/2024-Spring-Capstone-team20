using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class ServerManager : MonoBehaviour
{
    private static ServerManager _instance;

    public static ServerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("ServerManager");
                _instance = go.AddComponent<ServerManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private string csrfToken;

    private IEnumerator GetCsrfToken()
    {
        string url = "http://127.0.0.1:8000/csrf_token/";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var json = www.downloadHandler.text;
                csrfToken = JObject.Parse(json)["csrfToken"].ToString();
                Debug.Log("CSRF Token: " + csrfToken);
            }
            else
            {
                Debug.Log("Error getting CSRF token: " + www.error);
            }
        }
    }

    public IEnumerator GetCompletionCoroutine(string prompt)
    {
        yield return GetCsrfToken();

        string url = "http://127.0.0.1:8000/get-completion/";

        JObject jobj = new JObject { { "prompt", prompt } };
        string jsonData = jobj.ToString();

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("X-CSRFToken", csrfToken);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("에러 났대.");
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("이게 맞아?");
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}