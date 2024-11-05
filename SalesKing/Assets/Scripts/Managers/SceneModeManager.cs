using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneModeManager : MonoBehaviour
{
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
            case "OfficeMap": Awake_OfficeScene(); curScene = Define.SceneMode.OfficeMap; break;
            case "StageSelect": Awake_StageSelectScene(); curScene = Define.SceneMode.StageSelect; break;
            default: break;
        }
    }

    /*
    private void Awake_StartScene()
    {

    }
    */

    private void Awake_OfficeScene()
    {
        Managers.Instance.AddPlayerManager();
        Managers.Instance.AddOfficeManager();
        Managers.Instance.AddUserInputManager();
    }

    private void Awake_CityScene()
    {
        Managers.Instance.AddPlayerManager();
        Managers.Instance.AddNPCManager();
        Managers.Instance.AddUIManager();
        Managers.Instance.AddTimeManager();
        Managers.Instance.AddCameraManager();
        Managers.Instance.AddChatManager();
        Managers.Instance.AddConvoManager();
        Managers.Instance.AddCashManager();
        Managers.Instance.AddInventoryManager();
        Managers.Instance.AddTurnManager();
        Managers.Instance.AddUserInputManager();
        
    }

    private void Awake_StageSelectScene()
    {
        Managers.Instance.AddStageSelectManager();
    }

    public void LoadSceneByName(string name)
    {
        Managers.Instance.ClearChildManagers();
        SceneManager.LoadScene(name);
    }
}
