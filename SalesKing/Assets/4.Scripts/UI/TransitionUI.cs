using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // DOTween 사용

public class TransitionUI : MonoBehaviour
{
    // 페이드 인아웃 용도
    public Image FadeInFadeOut;
    public float FadeTime = 0.2f;

    public void StartFadeInFadeOut(float duration)
    {
        FadeInFadeOut.gameObject.SetActive(true);

        Sequence fadeSequence = DOTween.Sequence();
        // 페이드 인: 알파값을 0에서 1로 (0.2초)
        fadeSequence.Append(FadeInFadeOut.DOFade(1, FadeTime));
        // 유지 시간 (duration 동안 대기)
        fadeSequence.AppendInterval(duration);
        // 페이드 아웃: 알파값을 1에서 0으로 (0.2초)
        fadeSequence.Append(FadeInFadeOut.DOFade(0, FadeTime));

        fadeSequence.OnComplete(() => {
            FadeInFadeOut.gameObject.SetActive(false);
            FadeInFadeOut.color = new Color(FadeInFadeOut.color.r, FadeInFadeOut.color.g, FadeInFadeOut.color.b, 0f);
        });

        // 시퀀스 실행
        fadeSequence.Play();
    }
}
