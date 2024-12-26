using System.Collections.Generic;
using UnityEngine;

public class NewsKkuangEffecter : MonoBehaviour
{
    [SerializeField] List<(float, float)> tweenFactors = new List<(float, float)>
    {
        (0.2f, 2f)
    };
    [SerializeField] float scaleOut = 5f;
    private Vector3 originalPos;
    private Vector3 originalScale;

    private void Awake() {
        originalPos = transform.position;
        originalScale = transform.localScale;
    }

    private void OnEnable() {
        transform.position = originalPos;
        transform.localScale = originalScale * scaleOut;
        KkuangAction();
    }

    private void KkuangAction() {
        Util.PopDotween(transform, tweenFactors);
    }
}
