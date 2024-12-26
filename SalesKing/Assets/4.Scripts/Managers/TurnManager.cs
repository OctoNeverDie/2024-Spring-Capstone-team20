using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class TurnManager : Singleton<TurnManager>, ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => true;

    [SerializeField] private GameObject[] EndDaypanels;
    [SerializeField] private Image FirstFadeInPanel;
    [SerializeField] private GameObject CustomerReviewPanel;
    [SerializeField] private Image FinalFadeOutPanel;
    [SerializeField] private GameObject DontDestroyOnCityMapReload;
    [SerializeField] private City_TabletDataManager Tablet;

    [SerializeField] private GameObject StartDayPanel;
    [SerializeField] private Image StartFadeInPanel;
    [SerializeField] private TextMeshProUGUI NextDayText;

    [SerializeField] private GameObject myTutoManager;

    private bool isMouseInputChecking = false;
    private float duration = 1.0f;

    private int stage_count = 4;

    protected override void Awake()
    {
        base.Awake();
        foreach (var endday in EndDaypanels)
        {
            endday.SetActive(false);
        }
    }

    void Start()
    {
        isMouseInputChecking = false;
        StartDayFadeIn();
    }

    void Update()
    {
        if (isMouseInputChecking && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            isMouseInputChecking = false;
            EndDayAndUpdateToFile();
        }
    }

    private void StartDayFadeIn()
    {
        StartDayPanel.SetActive(true);
        PlayerManager.Instance.player.FreezeAndUnFreezePlayer(true);
        NextDayText.text = "DAY " + (DataController.Instance.playData.cur_day_ID + 1);
        AudioManager.Instance.PlaySFX("DayStart");

        Sequence fadeSequence = DOTween.Sequence();
        
        fadeSequence.Append(NextDayText.DOFade(1f, duration));
        fadeSequence.AppendInterval(0.5f);
        fadeSequence.Append(NextDayText.DOFade(0f, duration));
        fadeSequence.Append(StartFadeInPanel.DOFade(0f, duration));
        fadeSequence.OnComplete(() =>
        {
            StartDayPanel.SetActive(false);
            PlayerManager.Instance.player.FreezeAndUnFreezePlayer(false);
            myTutoManager.SetActive(true);
        });
    }

    public void EndDayShowSummary()
    {
        PlayerManager.Instance.player.FreezeAndUnFreezePlayer(true);
        foreach (var endday in EndDaypanels)
        {
            endday.SetActive(true);
        }

        // 페이드 인
        FirstFadeInPanel.DOFade(1f, duration).OnComplete(() =>
        {
            Tablet.ShowDaySummary();
            PlayScaleUp(CustomerReviewPanel.transform);
        });
    }

    void EndDayAndUpdateToFile()
    {
        FinalFadeOutPanel.DOFade(1f, duration).OnComplete(() =>
        {
            DataController.Instance.playData.cur_day_ID++;
            DataController.Instance.ToPlayJson(DataController.Instance.gameData.cur_save_file_ID);

            //DontDestroyOnLoad(DontDestroyOnCityMapReload);
            if (DataController.Instance.playData.cur_day_ID == stage_count)
            {
                SceneManager.LoadScene("Last");
            }
            else
            {
                SceneManager.LoadScene("CityMap");
            }
            
        });
    }

    public void LoadOtherScene()
    {
        //Destroy(DontDestroyOnCityMapReload);
    }

    // 띠용~ 이러면서 커지는거
    public void PlayScaleUp(Transform targetObject, float duration = 1f, float overshoot = 1.2f)
    {
        // 초기 스케일 설정 (필요하면 생략 가능)
        targetObject.localScale = Vector3.one;

        // Scale 애니메이션: 띠용~ 커지기
        targetObject.DOScale(Vector3.one * overshoot, duration)
            .SetEase(Ease.OutBack) // 부드럽게 늘어나기
            .OnComplete(() =>
            {
                Debug.Log("Scale Up Complete");
                isMouseInputChecking = true;
            });
    }
}
