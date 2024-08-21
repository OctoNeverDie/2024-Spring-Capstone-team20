using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneModeManager : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        Managers.Instance.ClearChildManagers();
        switch (scene.name)
        {
            case "Start": Awake_StartScene(); break;
            case "CityMap": Awake_CityScene(); break;
            case "OfficeMap": Awake_OfficeScene(); break;
            default: break;
        }
    }

    private void Awake_StartScene()
    {

    }

    private void Awake_OfficeScene()
    {
        Managers.Instance.AddPlayerManager();
    }

    private void Awake_CityScene()
    {
        Managers.Instance.AddPlayerManager();
        Managers.Instance.AddNPCManager();
        Managers.Instance.AddUIManager();
    }

    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }
}
