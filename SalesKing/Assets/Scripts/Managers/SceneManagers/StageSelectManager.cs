using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using OpenAI_API;

public class StageSelectManager : MonoBehaviour
{
    int curstage = 1;
    CinemachineVirtualCamera Camera; // 1인칭 시점 가상 카메라
    StageSelectUI UI;

    void Start()
    {
        Camera = FindObjectOfType<CinemachineVirtualCamera>();
        UI = GameObject.Find("Canvas").GetComponent<StageSelectUI>();
        Camera.Follow = UI.stagePanels[curstage-1].transform;
        CheckArrowBtns();
    }

    public void SwitchStages(bool isLeft)
    {
        if (isLeft)
        {
            curstage--;
            Camera.Follow = UI.stagePanels[curstage-1].transform;
        }
        else
        {
            curstage++;
            Camera.Follow = UI.stagePanels[curstage-1].transform;
        }
        CheckArrowBtns();
    }

    private void CheckArrowBtns()
    {
        // 첫 패널
        if (curstage == 1)
        {
            UI.LeftBtn.gameObject.SetActive(false);
        }
        // 막 패널
        else if (curstage == UI.stagePanels.Count)
        {
            UI.RightBtn.gameObject.SetActive(false);
        }
        else
        {
            UI.LeftBtn.gameObject.SetActive(true);
            UI.RightBtn.gameObject.SetActive(true);
        }
    }

    public void StartStage()
    {
        DataController.Instance.playData.curStageNum = curstage;
        Managers.Scene.LoadSceneByName("CityMap");
    }

    /*
    void Update()
    {
        if (Input.GetButtonDown("Interaction"))
        {
            DataController.Instance.playData.curStageNum++;

        }
    }
    */
}
