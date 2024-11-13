using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class SaveFilePanelUI : MonoBehaviour
{
    public string fileID;
    public Button file_button;
    public Button delete_button;
    public TextMeshProUGUI file_button_text;

    void Start()
    {
        file_button.onClick.AddListener(OnClickSaveFileButton);
        delete_button.onClick.AddListener(OnClickDeleteButton);
    }

    public void OnClickSaveFileButton()
    {
        SaveFileManager.Instance.LoadCitySceneBySaveFile(fileID);
    }

    public void OnClickDeleteButton()
    {
        SaveFileManager.Instance.DeleteSaveFile(fileID, this.gameObject);
    }
}
