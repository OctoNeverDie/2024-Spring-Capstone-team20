using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialNewsEffect : TutorialBase
{
    [SerializeField] private List<GameObject> news_list;
    private float big_scale = 3f;
    private float duration = 2f;

    public override void Enter()
    {
        for (int i = 0; i < news_list.Count; i++)
        {
            news_list[i].transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public override void Execute(TutorialController controller)
    {
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < news_list.Count; i++)
        {
            sequence.Append(news_list[i].transform.DOScale(new Vector3(big_scale, big_scale, big_scale), 0.1f)); 
            sequence.Append(news_list[i].transform.DOScale(new Vector3(1, 1, 1), duration));
        }
        /*
        sequence.AppendCallback(() => {

            Sequence moveSequence = DOTween.Sequence();

            for (int i = 0; i < news_list.Count; i++)
            {
                
                moveSequence.Append(transform.DOMoveY(transform.position.y + moveDistance, moveDuration)); // 위로 이동
                moveSequence.Append(transform.DOFade(0, moveDuration)); // 점차적으로 사라짐
                
            }

            moveSequence.Play(); // 이동 애니메이션 실행
        });
        */
        sequence.OnComplete(() =>
        {
            controller.SetNextTutorial();
        });

        sequence.Play();
    }

    public override void Exit()
    {
        for (int i = 0; i < news_list.Count; i++)
        {
            news_list[i].transform.localScale = new Vector3(0, 0, 0);
        }
    }
}
