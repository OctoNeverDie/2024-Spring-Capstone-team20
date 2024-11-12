using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneModeManager : MonoBehaviour
{
    /*
    public Define.SceneMode curScene;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Scene Loaded: " + scene.name);
        Managers.Instance.ClearChildManagers();
        switch (scene.name)
        {
            //case "Start": Awake_StartScene(); break;
            case "CityMap": Awake_CityScene(); curScene = Define.SceneMode.CityMap; break;
            case "CityMap_juyeon": Awake_CityScene(); curScene = Define.SceneMode.CityMap; break;
            default: break;
        }
    }

    private void Awake_CityScene()
    {
        Managers.Instance.AddChatManager();
        Managers.Instance.AddConvoManager();
    }

    public void LoadSceneByName(string name)
    {
        Managers.Instance.ClearChildManagers();
        SceneManager.LoadScene(name);
    }*/
}
