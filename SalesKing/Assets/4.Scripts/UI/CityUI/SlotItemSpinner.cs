using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using DG.Tweening;
using System.Collections;

public class SlotItem : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI ItemSlot;

    float startInterval = 0.005f;
    float minimumInterval = 0.01f;
    float spinDuration = 1.5f;
    Ease tweening = Ease.InOutQuad;

    List<string> items;
    string selectedItem;
    bool onSpinning = false;
    Coroutine spinCoroutine;
    float defaultInterval;

    private void Start()
    {
        items = DataGetter.Instance.ItemList
                .Select(item => item.ObjName)
                .ToList();

        StartSpinning("사탕");
    }

    public void StartSpinning(string selected)
    {
        if (!onSpinning)
        {
            onSpinning = true;
            selectedItem = selected;
            defaultInterval = startInterval;
            spinCoroutine = StartCoroutine(SpinText());
        }
    }

    public void StopSpinning()
    {
        if (onSpinning)
        {
            if (spinCoroutine != null)
            {
                StopCoroutine(spinCoroutine);
            }
            spinCoroutine = StartCoroutine(SlowDownAndStop());
        }
    }

    private IEnumerator SpinText()
    {
        float elapsedTime = 0f;
        float spinDuration = 0.2f;

        while (elapsedTime < spinDuration)
        {
            ItemSlot.text = items[Random.Range(0, items.Count)];
            yield return new WaitForSecondsRealtime(defaultInterval);
            elapsedTime += defaultInterval;
        }

        StopSpinning();
    }

    private IEnumerator SlowDownAndStop()
    {
        Sequence slowDownSequence = DOTween.Sequence();

        float startInterval = defaultInterval;
        float endInterval = minimumInterval;

        Tween intervalTween = DOTween.To(() => startInterval, x => startInterval = x, endInterval, spinDuration)
                                        .SetEase(tweening);

        slowDownSequence.Append(intervalTween);

        spinCoroutine = StartCoroutine(SpinWithInterval(startInterval));

        slowDownSequence.OnComplete(() =>
        {
            // 최종적으로 스피닝을 멈추고 팝업 이펙트 적용
            if (spinCoroutine != null)
            {
                StopCoroutine(spinCoroutine);
            }
            else { Debug.Log("OnComplete"); }
            ItemSlot.text = selectedItem;
            onSpinning = false;
            PlayPopEffect();
        });

        
        yield return slowDownSequence.WaitForCompletion();
    }

    // 스피닝 간격을 동적으로 변경하는 Coroutine
    private IEnumerator SpinWithInterval(float interval)
    {
        DOTween.To(() => defaultInterval, x => defaultInterval = x, minimumInterval, spinDuration)
           .SetEase(tweening)
           .SetUpdate(true); // Time Scale에 영향을 받지 않도록 설정 (필요 시)

        // defaultInterval이 minimumInterval에 도달할 때까지 텍스트를 업데이트
        while (defaultInterval < minimumInterval)
        {
            int idx = Random.Range(0, items.Count);
            ItemSlot.text = items[idx];
            Debug.Log($"들어오긴 하나요?{onSpinning}, {defaultInterval}, {minimumInterval}. {idx}");
            yield return new WaitForSeconds(defaultInterval);
        }
    }

    private void PlayPopEffect()
    {
        // 초기 스케일 저장
        Vector3 originalScale = ItemSlot.transform.localScale;

        // 팝업 이펙트 Sequence 생성
        Sequence popSequence = DOTween.Sequence();

        popSequence.Append(ItemSlot.transform.DOScale(originalScale * 2f, 0.8f)) // 1.2배로 확대
                   .Append(ItemSlot.transform.DOScale(originalScale*1.2f, 0.4f))
                   .Append(ItemSlot.transform.DOScale(originalScale * 1.1f, 0.2f))
                   .Append(ItemSlot.transform.DOScale(originalScale, 0.1f))// 원래 크기로 축소
                   .SetEase(Ease.OutBounce);

        popSequence.Play();
    }
}
