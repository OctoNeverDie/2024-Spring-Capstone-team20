using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneTest : MonoBehaviour
{
    public void LoadCityScene()
    {
        SceneManager.LoadScene("CityMap");
    }

    public void LoadOfficeScene()
    {
        SceneManager.LoadScene("OfficeMap");
    }

    public void GameExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
