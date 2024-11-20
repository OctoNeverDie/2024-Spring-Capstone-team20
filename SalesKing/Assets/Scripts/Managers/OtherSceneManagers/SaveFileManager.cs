using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveFileManager : MonoBehaviour
{
    private static SaveFileManager instance;
    public static SaveFileManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SaveFileManager>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject();
                    instance = singleton.AddComponent<SaveFileManager>();
                    singleton.name = typeof(SaveFileManager).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singleton);
                }
            }
            return instance;
        }
    }

    SaveFileMainUI UI;
    List<SaveFilePanelUI> panel_list = new List<SaveFilePanelUI>();
    int slot_count = 3;

    private void Start()
    {
        UI = GameObject.Find("Canvas").GetComponent<SaveFileMainUI>();
        UpdateSaveFilePanels();
    }

    public void UpdateSaveFilePanels()
    {
        // save file이 없을 때 : create 1개, ??? 2개 
        // save file이 1개일때 : 파일 1개, create 1개, ??? 1개
        // save file이 2개일때 : 파일 2개, create 1개
        // save file이 3개일때 : 파일별로 슬롯 총 3개
        
        int save_file_count = DataController.Instance.gameData.save_files_IDs.Count;
        foreach (Transform child in UI.VerticalLayoutPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < slot_count; i++)
        {
            GameObject panel = Instantiate(UI.SaveFilePanelPrefab); // 프리팹 인스턴스화
            panel.transform.SetParent(UI.VerticalLayoutPanel.transform, false); // VerticalLayoutPanel을 부모로 설정
            SaveFilePanelUI panel_UI = panel.GetComponent<SaveFilePanelUI>();
            panel_list.Add(panel_UI);

            // file panel
            if(i < save_file_count)
            {
                panel_UI.fileID = DataController.Instance.gameData.save_files_IDs[i];
            }
            // create file panel
            else if(i == save_file_count)
            {
                panel_UI.isCreateFile = true;
            }
            // ???
            else
            {
                panel_UI.isNullPanel = true;
            }
        }
        
    }

    public void LoadCitySceneBySaveFile(string file_id)
    {
        DataController.Instance.gameData.cur_save_file_ID = file_id;
        Debug.Log("load save file name : " + file_id);
        SceneManager.LoadScene("CityMap");
    }

    public void DeleteSaveFile(string file_id)
    {
        DataController.Instance.gameData.save_files_IDs.Remove(file_id);
        DataController.Instance.DeletePlayData(file_id);
        Debug.Log("delete save file name : " + file_id);
        UpdateSaveFilePanels();
    }

    public void CreateSaveFile()
    {
        DataController.Instance.LoadPlayData("playData" + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss"));
        UpdateSaveFilePanels();
    }
}
