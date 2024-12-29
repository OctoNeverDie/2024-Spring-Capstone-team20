using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMain : MonoBehaviour
{
    public void BackToMainScene()
    {
        Debug.Log("back to main");
        SceneManager.LoadScene("Start");
    }
}
