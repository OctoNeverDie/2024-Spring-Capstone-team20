using UnityEngine;
using UnityEngine.UI;

public class TutorialButtonTrigger : TutorialBase
{
    [SerializeField]
    private Button button_trigger;
    public	bool isTrigger { set; get; } = false;
    

	public override void Enter()
	{
        button_trigger.onClick.AddListener(OnClickTriggerButton);
    }

	public override void Execute(TutorialController controller)
	{
        if ( isTrigger == true )
		{
			controller.SetNextTutorial();
		}
	}

	public override void Exit()
	{

    }

	private void OnClickTriggerButton()
	{
		isTrigger = true;
    }
}

