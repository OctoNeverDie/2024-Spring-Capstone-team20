using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileManager : MonoBehaviour
{
    SaveFileUI UI;
    int save_file_count = 5;

    private void Start()
    {
        UI = GameObject.Find("Canvas").GetComponent<SaveFileUI>();
        SpawnSaveFilePanels();
    }

    public void SpawnSaveFilePanels()
    {
        for (int i = 0; i < save_file_count; i++)
        {
            GameObject panel = Instantiate(UI.SaveFilePanelPrefab); // 프리팹 인스턴스화
            panel.transform.SetParent(UI.VerticalLayoutPanel.transform, false); // VerticalLayoutPanel을 부모로 설정

        }
        /*
        // 버튼 클릭 이벤트 연결
        LeftBtn.onClick.AddListener(OnClickLeftBtn);
        RightBtn.onClick.AddListener(OnClickRightBtn);
        StartBtn.onClick.AddListener(OnClickStartBtn);
        */
    }
}
