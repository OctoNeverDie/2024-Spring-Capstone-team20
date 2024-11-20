using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileMainUI : MonoBehaviour
{
    public GameObject SaveFilePanelPrefab;
    public GameObject VerticalLayoutPanel;

    [SerializeField] GameObject StoryModeText;
    [SerializeField] GameObject InfinityModeText;

    void Start()
    {
        Define.GameMode mode = DataController.Instance.gameData.this_game_mode;
        switch (mode)
        {
            case Define.GameMode.Story: StoryModeText.SetActive(true); InfinityModeText.SetActive(false); break;
            case Define.GameMode.Infinity: StoryModeText.SetActive(false); InfinityModeText.SetActive(true); break;
            default: break;
        }
    }
}
