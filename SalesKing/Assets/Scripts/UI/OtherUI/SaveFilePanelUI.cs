using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class SaveFilePanelUI : MonoBehaviour
{
    public string fileID;
    public bool isCreateFile = false;
    public bool isNullPanel = false;

    public Button file_button;
    public Button delete_button;
    public TextMeshProUGUI file_button_text;

    void Start()
    {
        file_button.onClick.AddListener(OnClickFileButton);
        delete_button.onClick.AddListener(OnClickDeleteButton);
        SetSaveFilePanel();
    }

    public void SetSaveFilePanel()
    {
        if (isCreateFile)
        {
            file_button_text.text = "Create New Save File";
            delete_button.interactable = false;
        }
        else if (isNullPanel)
        {
            file_button_text.text = "???";
            file_button.interactable = false;
            delete_button.interactable = false;
        }
        else
        {
            DataController.Instance.LoadPlayData(fileID);
            int saved_day = DataController.Instance.playData.cur_day_ID;
            file_button_text.text = (saved_day+1)+"일부터 계속하기\n"+fileID;
        }
            
    }

    public void OnClickFileButton()
    {
        if (isCreateFile)
        {
            // 이 패널이 들고 있는 file ID
            SaveFileManager.Instance.CreateSaveFile();
            SaveFileManager.Instance.LoadCitySceneBySaveFile(fileID);
        }
        else if (!isNullPanel)
        {
            SaveFileManager.Instance.LoadCitySceneBySaveFile(fileID);
        }
            
    }

    public void OnClickDeleteButton()
    {
        if(!isCreateFile && !isNullPanel)
        {
            // 이 패널이 들고 있는 file ID
            SaveFileManager.Instance.DeleteSaveFile(fileID);
        }

    }
}
