
using UnityEngine;

public class TestAPI : MonoBehaviour
{
    private void Start()
    {
        string prompt = "¾È³ç";
        StartCoroutine(ServerManager.Instance.GetCompletionCoroutine(prompt));
    }
}