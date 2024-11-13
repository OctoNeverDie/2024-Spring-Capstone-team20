using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        // 중복 방지 코드
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    SaveFileUI UI;
    List<SaveFilePanelUI> panel_list = new List<SaveFilePanelUI>();

    private void Start()
    {
        UI = GameObject.Find("Canvas").GetComponent<SaveFileUI>();
        UpdateSaveFilePanels();
    }

    public void UpdateSaveFilePanels()
    {
        int save_file_count = DataController.Instance.gameData.save_files_IDs.Count;
        foreach (Transform child in UI.VerticalLayoutPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < save_file_count; i++)
        {
            GameObject panel = Instantiate(UI.SaveFilePanelPrefab); // 프리팹 인스턴스화
            panel.transform.SetParent(UI.VerticalLayoutPanel.transform, false); // VerticalLayoutPanel을 부모로 설정
            panel_list.Add(panel.GetComponent<SaveFilePanelUI>());
        }
    }

    public void LoadCitySceneBySaveFile(string file_id)
    {
        Debug.Log("load save file name : " + file_id);
    }

    public void DeleteSaveFile(string file_id, GameObject panel)
    {
        Destroy(panel);
        Debug.Log("delete save file name : " + file_id);
    }
}
