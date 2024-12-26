using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NewsKkuangEffecter : MonoBehaviour
{
    [SerializeField]
    List<(float, float)> tweenFactors = new List<(float, float)>
    {
        (2f, 0.01f),
        (1f, 0.7f)
    };
    [SerializeField] float scaleOut = 5f;
    [SerializeField] Ease ease = Ease.Linear;

    private void OnEnable() {
        KkuangAction();
    }

    private void KkuangAction() {
        Debug.Log("????????????????????");
        Util.PopDotween(transform, tweenFactors, ease);
    }
}
