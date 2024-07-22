using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SendTextToDjango : MonoBehaviour
{
    private string djangoUrl = "http://127.0.0.1:8000/";  // Django 서버 URL

    void Start()
    {
        StartCoroutine(SendPostRequest("안녕"));
    }

    IEnumerator SendPostRequest(string text)
    {
        WWWForm form = new WWWForm();
        form.AddField("prompt", text);

        using (UnityWebRequest www = UnityWebRequest.Post(djangoUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("에러 났대.");
                Debug.Log(www.error);
            }
            else
            {
                // 응답을 성공적으로 받았을 때
                Debug.Log("이게 맞아?");
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
