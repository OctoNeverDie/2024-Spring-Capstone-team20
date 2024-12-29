using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class NewsKkuangEffecter : MonoBehaviour
{
    [SerializeField] List<(float, float)> tweenFactors = new List<(float, float)>
    {
        (3f, 0.001f),
        (1f, 0.5f)
    };
    [SerializeField] Ease ease = Ease.Linear;

    private void Start()
    {
        this.GetComponent<AudioSource>().Play();
        KkuangAction();
    }

    private void KkuangAction() {

        Sequence seq = Util.PopDotween(transform, tweenFactors, ease);
        seq.Play();
    }
}
