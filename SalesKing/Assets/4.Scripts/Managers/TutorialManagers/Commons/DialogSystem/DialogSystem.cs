using System.Collections;
using UnityEngine;
using TMPro;

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject			cur_panel;                      // 현재 분기의 대화 패널
    [SerializeField]
	private	Dialog[]			dialogs;						// 현재 분기의 대사 목록
	[SerializeField]
	private	TextMeshProUGUI 	textDialogue;					// 현재 대사 출력 Text UI
	[SerializeField]
	private	GameObject			objectArrow;					// 대사가 완료되었을 때 출력되는 커서 오브젝트
	[SerializeField]
	private	float				typingSpeed;					// 텍스트 타이핑 효과의 재생 속도
	[SerializeField]
	private	KeyCode				keyCodeSkip = KeyCode.Return;	// 타이핑 효과를 스킵하는 키

	private	int					currentIndex = -1;
	private	bool				isTypingEffect = false;			// 텍스트 타이핑 효과를 재생중인지

	public void Setup()
	{
		cur_panel.SetActive(true);
		SetNextDialog();
	}

	public bool UpdateDialog()
	{
		if ( Input.GetKeyDown(keyCodeSkip) || Input.GetMouseButtonDown(0) )
		{
			// 텍스트 타이핑 효과를 재생중일때 마우스 왼쪽 클릭하면 타이핑 효과 종료
			if ( isTypingEffect == true )
			{
				// 타이핑 효과를 중지하고, 현재 대사 전체를 출력한다
				StopCoroutine("TypingText");
				isTypingEffect = false;
                textDialogue.text = dialogs[currentIndex].dialogue + "";
				// 대사가 완료되었을 때 출력되는 커서 활성화
				objectArrow.SetActive(true);

				return false;
			}

			// 다음 대사 진행
			if ( dialogs.Length > currentIndex + 1 )
			{
				SetNextDialog();
			}
			// 대사가 더 이상 없을 경우 true 반환
			else
			{
				// 모든 캐릭터 이미지를 어둡게 설정
				for ( int i = 0; i < 2; ++ i )
				{
					// 모든 대화 관련 게임오브젝트 비활성화
					InActiveObjects(i);
				}
				cur_panel.SetActive(false);
				return true;
			}
		}

		return false;
	}

	private void SetNextDialog()
	{
		currentIndex ++;

		// 화자의 대사 텍스트 활성화 및 설정 (Typing Effect)
		textDialogue.gameObject.SetActive(true);
		StartCoroutine(nameof(TypingText));
	}

	private void InActiveObjects(int index)
	{
		textDialogue.gameObject.SetActive(false);
		objectArrow.SetActive(false);
	}

	private IEnumerator TypingText()
	{
		int index = 0;
		
		isTypingEffect = true;

		// 텍스트를 한글자씩 타이핑치듯 재생
		while ( index < dialogs[currentIndex].dialogue.Length )
		{
			textDialogue.text = dialogs[currentIndex].dialogue.Substring(0, index);

			index ++;

			yield return new WaitForSeconds(typingSpeed);
		}

		isTypingEffect = false;

		// 대사가 완료되었을 때 출력되는 커서 활성화
		objectArrow.SetActive(true);
	}
}

[System.Serializable]
public struct Dialog
{
	public	string		dialogue;	// 대사
}

