using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
	[SerializeField]
	private	List<TutorialBase>	tutorials;
    private TutorialBase		currentTutorial = null;
	private	int					currentIndex = -1;

	[SerializeField]
	private GameObject endingCanvas = null;

	public bool isComplete = false;

    private void Start()
	{
		if(endingCanvas != null)
			endingCanvas.SetActive(true);

        if (PlayerManager.Instance != null && PlayerManager.Instance.player != null) PlayerManager.Instance.player.FreezeAndUnFreezePlayer(true);
		if(UserInputManager.Instance != null) UserInputManager.Instance.isKeyInputLocked = true;
		SetNextTutorial();
	}

	private void Update()
	{
		if ( currentTutorial != null )
		{
			currentTutorial.Execute(this);
		}
	}

	public void SetNextTutorial()
	{
		// 현재 튜토리얼의 Exit() 메소드 호출
		if ( currentTutorial != null )
		{
            currentTutorial.Exit();
		}

		// 마지막 튜토리얼을 진행했다면 CompletedAllTutorials() 메소드 호출
		if(currentIndex >= tutorials.Count -1)
		{
			Debug.Log("마지막 튜토리얼 끝");
			CompletedAllTutorials();
			return;
		}

		// 다음 튜토리얼 과정을 currentTutorial로 등록
		currentIndex ++;
		currentTutorial = tutorials[currentIndex];

		// 새로 바뀐 튜토리얼의 Enter() 메소드 호출
		currentTutorial.Enter();
	}

	public void CompletedAllTutorials()
	{
		currentTutorial = null;
        isComplete = true;
        Debug.Log("Complete All");
        if (PlayerManager.Instance != null && PlayerManager.Instance.player != null) PlayerManager.Instance.player.FreezeAndUnFreezePlayer(false);
        if (UserInputManager.Instance != null) UserInputManager.Instance.isKeyInputLocked = false;
    }
}

