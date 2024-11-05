using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUI : MonoBehaviour
{
    public GameObject StagePanelsHolder;
    public List<GameObject> stagePanels = new List<GameObject>();
    public Button LeftBtn;
    public Button RightBtn;
    public Button StartBtn;

    void Start()
    {
        for(int i=0; i<StagePanelsHolder.transform.childCount; i++)
        {
            stagePanels.Add(StagePanelsHolder.transform.GetChild(i).gameObject);
        }

        // 버튼 클릭 이벤트 연결
        LeftBtn.onClick.AddListener(OnClickLeftBtn);
        RightBtn.onClick.AddListener(OnClickRightBtn);
        StartBtn.onClick.AddListener(OnClickStartBtn);
    }

    private void OnClickLeftBtn()
    {
        Managers.Stage.SwitchStages(true);
    }

    private void OnClickRightBtn()
    {
        Managers.Stage.SwitchStages(false);
    }

    private void OnClickStartBtn()
    {
        Managers.Stage.StartStage();
    }
}
