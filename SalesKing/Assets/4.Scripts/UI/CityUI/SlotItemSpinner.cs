using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI ItemSlot;
    [SerializeField]
    Image bg;
    [SerializeField]
    Button okayBtn;

    float startInterval = 0.005f;
    float minimumInterval = 0.01f;
    float spinDuration = 1f;
    Ease tweening = Ease.InOutQuad;

    List<string> items;
    bool onSpinning = false;
    Coroutine spinCoroutine;
    float defaultInterval;

    private void OnEnable()
    {
        if (items == null)
        {
            items = DataGetter.Instance.ItemList
                .Select(item => item.ObjName)
                .ToList();
        }
        bg.color = Color.white;
        okayBtn.gameObject.SetActive(false);
        StartSpinning();
    }

    public void StartSpinning()
    {
        if (!onSpinning)
        {
            onSpinning = true;
            defaultInterval = startInterval;
            spinCoroutine = StartCoroutine(SlowDownAndStop());
        }
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
            onSpinning = false;
            bg.color = Color.green;
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
            yield return new WaitForSeconds(defaultInterval);
        }

        ItemSlot.text = ChatManager.Instance.playerItemName;
    }

    private void PlayPopEffect()
    {
        List<(float scale, float duration)> tweenFactors = new List<(float, float)>
        {
            (1.8f, 0.8f),
            (1.2f, 0.4f),
            (1.1f, 0.2f),
            (1f, 0.1f)
        };
        AudioManager.Instance.PlaySFX("SlotEnd");
        okayBtn.gameObject.SetActive(true);
        Debug.Log("냐냐냐냐");
        Util.PopDotween(ItemSlot.transform, tweenFactors);
    }
}
