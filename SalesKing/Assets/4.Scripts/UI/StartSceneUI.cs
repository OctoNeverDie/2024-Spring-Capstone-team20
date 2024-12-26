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
        SceneManager.LoadScene("OfficeMap");
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
    [SerializeField] private Color disabledColor = Color.grey; // 상호작용 꺼졌을 때 색상

    private List<ButtonTextPair> buttonTextPairs = new List<ButtonTextPair>();

    private void Start()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            // 모든 하위 자식을 탐색하여 버튼-텍스트 짝 생성
            if (child.name == "StartNew" || child.name == "Continue" || child.name == "Option" || child.name == "Exit")
            {
                var texts = child.GetComponentsInChildren<TextMeshProUGUI>(); // 모든 TextMeshProUGUI 컴포넌트를 가져옴
                var button = child.GetComponent<Button>();

                // Button 컴포넌트가 없거나 interactable이 false인 경우 건너뜀
                if (button == null || !button.interactable)
                {
                    OnHover(texts, disabledColor);
                    continue;
                }

                if (texts.Length > 0)
                {
                    var eventTrigger = child.gameObject.AddComponent<EventTrigger>();
                    AddEvent(eventTrigger, EventTriggerType.PointerEnter, () => OnHover(texts, hoverColor));
                    AddEvent(eventTrigger, EventTriggerType.PointerExit, () => OnHover(texts, defaultColor));

                    buttonTextPairs.Add(new ButtonTextPair { button = child.gameObject, text = texts[0] });
                }
            }
        }
    }

    private void OnHover(TextMeshProUGUI[] texts, Color color)
    {
        foreach (var text in texts)
        {
            if (text != null)
            {
                text.color = color;
            }
        }
    }

    private void AddEvent(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(_ => action());
        trigger.triggers.Add(entry);
    }
}

