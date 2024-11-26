using UnityEngine;

public class TutorialKeyTrigger : TutorialBase
{
    [SerializeField]
    private KeyCode keycode_trigger;   // 타이핑 효과를 스킵하는 키
    public	bool isTrigger { set; get; } = false;

	public override void Enter()
	{
        UserInputManager.Instance.isKeyInputLocked = false;
    }

	public override void Execute(TutorialController controller)
	{
        if (Input.GetKeyDown(keycode_trigger))
        {
			isTrigger = true;
            Debug.Log("이번 키코드는 " + keycode_trigger);
            UserInputManager.Instance.isKeyInputLocked = true;
        }

        if ( isTrigger == true )
		{
			controller.SetNextTutorial();
		}
	}

	public override void Exit()
	{
        PlayerManager.Instance.player.FreezeAndUnFreezePlayer(true);
        UserInputManager.Instance.isKeyInputLocked = true;
    }

}

