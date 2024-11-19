using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HoverButtonTextColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI childText;
    private Color originalColor;

    public Color hoverColor = Color.yellow; // 마우스 오버 시 변경할 색상

    private void Awake()
    {
        // 자식 객체 중 TextMeshProUGUI를 찾아 저장
        childText = GetComponentInChildren<TextMeshProUGUI>();
        if (childText != null)
        {
            originalColor = childText.color; // 기존 색상을 저장
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (childText != null)
        {
            childText.color = hoverColor; // 색상 변경
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (childText != null)
        {
            childText.color = originalColor; // 원래 색상으로 복구
        }
    }
}