using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialNewsEffect : TutorialBase
{
    [SerializeField] private GameObject news_object;
    [SerializeField] private List<GameObject> news_list;
    private AudioSource source;
    private AudioClip sfx;

    private float big_scale = 3f;
    private float duration = 0.5f;
    private bool isEffectOver = false;

    public override void Enter()
    {
        isEffectOver = false;
        source = news_object.GetComponent<AudioSource>();

        for (int i = 0; i < news_list.Count; i++)
        {
            news_list[i].transform.localScale = new Vector3(0, 0, 0);
        }
        

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < news_list.Count; i++)
        {
            sequence.Append(news_list[i].transform.DOScale(new Vector3(big_scale, big_scale, big_scale), 0.01f).SetEase(Ease.OutBounce));
            sequence.Append(news_list[i].transform.DOScale(new Vector3(1, 1, 1), duration).SetEase(Ease.InOutQuad));
            sequence.AppendCallback(() =>
            {
                source.Play();
            });
        }

        sequence.AppendInterval(1f);

        sequence.AppendCallback(() => {
             news_object.transform.DOMoveY(news_object.transform.position.y + 1500f, 2f);
        });
        
        sequence.OnComplete(() =>
        {
            isEffectOver = true;
        });

        sequence.Play();
    }

    public override void Execute(TutorialController controller)
    {
        if(isEffectOver) controller.SetNextTutorial();
    }

    public override void Exit()
    {
        /*
        for (int i = 0; i < news_list.Count; i++)
        {
            news_list[i].transform.localScale = new Vector3(0, 0, 0);
        }
        */
    }
}
