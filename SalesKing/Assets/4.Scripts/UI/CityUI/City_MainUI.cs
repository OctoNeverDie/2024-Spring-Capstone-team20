using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class City_MainUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI time_text;
    [SerializeField] public GameObject NextNPCButton;
    [SerializeField] GameObject info_panel;
    [SerializeField] GameObject info_button;

    void Start()
    {
        time_text.text = "DAY " + (DataController.Instance.gameData.cur_day_ID+1);

        NextNPCButton.GetComponent<Button>().onClick.AddListener(onClickNextNPC);
        info_panel.GetComponent<Button>().onClick.AddListener(closeInfo);
        info_button.GetComponent<Button>().onClick.AddListener(openInfo);
    }

    public void openInfo()
    {
        info_panel.transform.DOLocalMove(new Vector3(-400, 418, 0), 0.5f).SetEase(Ease.OutQuad);
        info_button.SetActive(false);
    }

    public void closeInfo()
    {
        info_panel.transform.DOLocalMove(new Vector3(-400, 700, 0), 0.5f).SetEase(Ease.OutQuad);
        info_button.SetActive(true);
    }

    public void onClickNextNPC()
    {
        NextNPCButton.SetActive(false);
        NPCManager.Spawner.SpawnNextNPC();
    }
}
