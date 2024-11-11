using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileUI : MonoBehaviour
{
    public GameObject SaveFilePanelPrefab;
    public GameObject Panel;
    public List<GameObject> SaveFiles = new List<GameObject>();
    public List<Button> SaveFileButtons = new List<Button>();
    public List<Button> DeleteButtons = new List<Button>();

    //test
    private void Start()
    {
        SpawnSaveFilePanels();        
    }

    public void SpawnSaveFilePanels()
    {
        for (int i = 0; i < SaveFileButtons.Count; i++)
        {

        }
        /*
        // 버튼 클릭 이벤트 연결
        LeftBtn.onClick.AddListener(OnClickLeftBtn);
        RightBtn.onClick.AddListener(OnClickRightBtn);
        StartBtn.onClick.AddListener(OnClickStartBtn);
        */
    }

    private void OnClickSaveFileButton(int index)
    {

    }

    private void OnClickDeleteButton(int index)
    {

    }
}
