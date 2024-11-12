using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위해 추가

public class EventManager : MonoBehaviour
{
    public GameObject youDiedPanel;

    // Start is called before the first frame update
    void OnEnable()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(2f);
        sequence.Append(Camera.main.DOShakePosition(10f, 0.2f));

        sequence.Play();

        CanvasGroup canvasGroup = youDiedPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0; // 초기 투명도 설정
        }

        StartCoroutine(ShowPanelWithDelay(2f));
    }

    private IEnumerator ShowPanelWithDelay(float delay)
    {
        // 2초 대기
        yield return new WaitForSeconds(delay);
        youDiedPanel.SetActive(true);

        // 패널을 서서히 보이게 함
        CanvasGroup canvasGroup = youDiedPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            float fadeDuration = 1f; // 페이드 인 지속 시간
            float startAlpha = 0f;
            float endAlpha = 1f;

            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = endAlpha; // 마지막 투명도 설정
        }

        // 5초 후에 씬 로드
        yield return new WaitForSeconds(5f);
        LoadScene("Start");
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // 씬 로드
    }
}
