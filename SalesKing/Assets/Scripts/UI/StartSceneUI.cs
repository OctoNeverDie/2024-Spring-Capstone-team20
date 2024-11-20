using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class StartSceneUI : MonoBehaviour
{
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
        // 모든 하위 자식을 탐색하여 버튼-텍스트 짝 생성
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.name == "Story" || child.name == "Infinity" || child.name == "Option" || child.name == "Exit")
            {
                var text = child.GetComponentInChildren<TextMeshProUGUI>();
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

