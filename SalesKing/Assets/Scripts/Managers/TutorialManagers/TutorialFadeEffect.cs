using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFadeEffect : TutorialBase
{
    
    [SerializeField]
    private TutorialFadeEffect fadeEffect;
    [SerializeField]
    private bool isFadeIn = false;
    private bool isCompleted = false;

    public override void Enter()
    {
        if (isFadeIn == true)
        {
            //fadeEffect.FadeIn(OnAfterFadeEffect);
        }
        else
        {
            //fadeEffect.FadeOut(onAfterFadeEffect);
        }
    }

    private void OnAfterFadeEffect() 
    { 
        isCompleted = true;
    }

    public override void Execute(TutorialController controller)
    {
        if (isCompleted == true) {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        //ì—ëŸ¬ ë°©ì§€ìš©
    }


}
