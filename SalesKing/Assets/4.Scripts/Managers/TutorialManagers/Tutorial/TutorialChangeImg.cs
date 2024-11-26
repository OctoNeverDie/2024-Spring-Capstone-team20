using UnityEngine;
using UnityEngine.UI; // UI 사용 시 필요

public class TutorialChangeImg : TutorialBase
{
    // 변경할 이미지와 새 스프라이트를 설정할 변수
    [SerializeField] private Image uiImage; // UI Image 컴포넌트
    [SerializeField] private Sprite newSprite; // 새로 설정할 Sprite

    public override void Enter()
    {
        // 예제: Enter 단계에서 이미지를 변경할 수도 있음
        ChangeSprite();
    }

    public override void Execute(TutorialController controller)
    {
        controller.SetNextTutorial();
    }

    public override void Exit()
    {
    }

    // 스프라이트를 변경하는 함수
    public void ChangeSprite()
    {
        if (uiImage != null && newSprite != null)
        {
            uiImage.sprite = newSprite;
            Debug.Log("Sprite has been changed.");
        }
        else
        {
            Debug.LogWarning("UI Image or New Sprite is not assigned.");
        }
    }
}
