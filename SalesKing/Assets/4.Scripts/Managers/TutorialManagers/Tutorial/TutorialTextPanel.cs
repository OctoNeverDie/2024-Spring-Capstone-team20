using UnityEngine;

public class TutorialTextPanel : TutorialBase
{
	[SerializeField] private GameObject tuto_panel;
	[SerializeField] private GameObject text_panel;
    [SerializeField]
    private KeyCode keyCodeSkip = KeyCode.Return;   // 타이핑 효과를 스킵하는 키

    public override void Enter()
	{
		tuto_panel.SetActive(true);
		text_panel.SetActive(true);
	}

	public override void Execute(TutorialController controller)
	{
        if (Input.GetKeyDown(keyCodeSkip) || Input.GetMouseButtonDown(0))
		{
            controller.SetNextTutorial();
        }

	}

	public override void Exit()
	{
		tuto_panel.SetActive(false);
        text_panel.SetActive(false);
    }
}

