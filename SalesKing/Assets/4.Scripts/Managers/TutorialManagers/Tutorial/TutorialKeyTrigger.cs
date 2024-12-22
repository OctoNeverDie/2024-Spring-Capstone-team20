using UnityEngine;
using DG.Tweening;
using TMPro;

public class TutorialKeyTrigger : TutorialBase
{
    [SerializeField]
    private KeyCode keycode_trigger;   // 타이핑 효과를 스킵하는 키
    public	bool isTrigger { set; get; } = false;
    [SerializeField]
    private bool isOpenTablet;
    [SerializeField]
    private bool isAnyKey=false;
    [SerializeField]
    private GameObject Tablet;
    [SerializeField]
    private GameObject DoWhatText;
    [SerializeField]
    private string do_what_to_continue;

    public override void Enter()
	{
        DoWhatText.SetActive(true);
        //UserInputManager.Instance.isKeyInputLocked = false;
        DoWhatText.GetComponent<TextMeshProUGUI>().text = do_what_to_continue;
    }

	public override void Execute(TutorialController controller)
	{
        if(isAnyKey && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
        {
            isTrigger = true;
        }

        if (Input.GetKeyDown(keycode_trigger))
        {
            if (Input.GetButtonDown("Tab"))
            {
                if (isOpenTablet)
                {
                    OnClickShowTablet();
                }
                else
                {
                    OnClickHideTablet();
                }
            }
			isTrigger = true;
            Debug.Log("이번 키코드는 " + keycode_trigger);
            //UserInputManager.Instance.isKeyInputLocked = true;
        }

        if ( isTrigger == true )
		{
			controller.SetNextTutorial();
		}
	}

	public override void Exit()
	{
        //PlayerManager.Instance.player.FreezeAndUnFreezePlayer(true);
        //UserInputManager.Instance.isKeyInputLocked = true;
        DoWhatText.SetActive(false);
    }

    public void OnClickShowTablet()
    {
        // Tablet을 -2000,0,0에서 0,0,0으로 이동
        Tablet.transform.localPosition = new Vector3(-2000, 0, 0);
        Tablet.transform.DOLocalMove(Vector3.zero, 1f); // 1초 동안 이동
    }

    public void OnClickHideTablet()
    {
        // Tablet을 0,0,0에서 -2000,0,0으로 이동
        Tablet.transform.localPosition = new Vector3(0, 0, 0);
        Tablet.transform.DOLocalMove(new Vector3(-2000, 0, 0), 1f); // 1초 동안 이동
    }


}

