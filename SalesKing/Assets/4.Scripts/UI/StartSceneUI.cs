using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class StartSceneUI : MonoBehaviour
{
    public void LoadSaveFileScene_StoryMode()
    {
        DataController.Instance.gameData.this_game_mode = Define.GameMode.Story;
        //SceneManager.LoadScene("SaveFile");
        SceneManager.LoadScene("CityMap");
    }

    public void LoadSaveFileScene_InfinityMode()
    {
        DataController.Instance.gameData.this_game_mode = Define.GameMode.Infinity;
        //SceneManager.LoadScene("SaveFile");
        SceneManager.LoadScene("InfiniteNPCs");
    }

    public void GameExit()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        DataController.Instance.SaveGameData();
        string file_ID = DataController.Instance.gameData.cur_save_file_ID;
        DataController.Instance.SavePlayData(file_ID);
        Application.Quit();
        #endif
    }

    private struct ButtonTextPair
    {
        public GameObject button;
        public TextMeshProUGUI text;
    }

    [SerializeField] private Color hoverColor = Color.yellow; // 마우스 오버 시 색상
    [SerializeField] private Color defaultColor = Color.white; // 기본 색상

    private List<ButtonTextPair> buttonTextPairs = new List<ButtonTextPair>();

    private void Start()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            // 모든 하위 자식을 탐색하여 버튼-텍스트 짝 생성
            if (child.name == "Story" || child.name == "Infinity" || child.name == "Option" || child.name == "Exit")
            {
                var text = child.GetComponentInChildren<TextMeshProUGUI>();
                var button = child.GetComponent<Button>();

                // Button 컴포넌트가 없거나 interactable이 false인 경우 건너뜀
                if (button == null || !button.interactable)
                {
                    continue;
                }

                if (text != null)
                {
                    var eventTrigger = child.gameObject.AddComponent<EventTrigger>();
                    AddEvent(eventTrigger, EventTriggerType.PointerEnter, () => OnHover(text, hoverColor));
                    AddEvent(eventTrigger, EventTriggerType.PointerExit, () => OnHover(text, defaultColor));

                    buttonTextPairs.Add(new ButtonTextPair { button = child.gameObject, text = text });
                }
            }
        }
    }

    private void AddEvent(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(_ => action());
        trigger.triggers.Add(entry);
    }

    private void OnHover(TextMeshProUGUI text, Color color)
    {
        if (text != null)
        {
            text.color = color;
        }
    }
}

