using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialFadeEffect : TutorialBase
{
	[SerializeField]
	private	Image	fade_image;
	[SerializeField]
	private	bool		isFadeIn = false;
	private	bool		isCompleted = false;

	private float		duration = 1f;

	public override void Enter()
	{
		if ( isFadeIn == true )
		{
            fade_image.DOFade(0, duration).OnComplete(() =>
            {
				OnAfterFadeEffect();
            });
        }
		else
		{
            fade_image.DOFade(1, duration).OnComplete(() =>
            {
                OnAfterFadeEffect();
            });
        }
	}

	private void OnAfterFadeEffect()
	{
		isCompleted = true;
	}

	public override void Execute(TutorialController controller)
	{
		if ( isCompleted == true )
		{
			controller.SetNextTutorial();
		}
	}

	public override void Exit()
	{
	}
}

