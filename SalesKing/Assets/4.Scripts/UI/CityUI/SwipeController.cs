using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] Vector3 _scrollWidth = new Vector3(-1400f, 0, 0);
    [SerializeField] Vector3 _spacing = new Vector3(-160, 0, 0);
    [SerializeField] RectTransform _npcPagesRect;
    [Header("Effects--------------------")]
    [SerializeField] float _tweenTime = 0.8f;
    [SerializeField] Ease _tweenType = Ease.InCubic;
    [SerializeField] float _ratio = 0.75f;
    [Header("Buttons--------------------")]
    [SerializeField] Button Prev;
    [SerializeField] Button Next;

    int _contentPageCnt = 3;
    int _currentPage;
    Vector3 _targetPos;
    Vector3 _pageStep;
    Vector3 _firstVacantPageLocation;
    float _dragThreshold;

    private enum Move
    { 
        none,
        gotoFrontAgain,
        gotoBackAgain,
    }
    private void Awake()
    {
        _currentPage = 1;//현재 page, 1부터 시작
        _firstVacantPageLocation = _npcPagesRect.localPosition;
        //Debug.Log($"{_npcPagesRect.localPosition} _npcPagesRect.localPosition");
        //Debug.Log($"_firstVacantPageLocation = _npcPagesRect.localPosition;{_firstVacantPageLocation}");
        _pageStep = (_scrollWidth+ _spacing); //scroll 나오는 화면 + spacing
        _dragThreshold = Screen.width / 10;//드래그 인식 범위

        _npcPagesRect.localPosition += _pageStep;//1번 페이지로 보내기
        _targetPos = _npcPagesRect.localPosition;//target 현재와 맞추기, move할 때 업데이트 됨

        Prev.onClick.AddListener(Front);
        Next.onClick.AddListener(Back);
    }

    #region Scroll Input
    public void Front()
    {
        if (_currentPage > 1)//page가 2일 때 1이 된다.
        {
            _currentPage--;
            _targetPos -= _pageStep;
            MovePage();
        }
        else//page가 1일 때 맨 뒤로 가기
        {
            _currentPage = _contentPageCnt;
            _targetPos -= _pageStep * _ratio;//반만 가다, 순간이동 한 다음 +_pageStep/2해야함
            MovePage(Ease.InCubic, Move.gotoBackAgain, _tweenTime / 2f);
        }
    }

    public void Back()
    {
        if (_currentPage < _contentPageCnt)//_contentPageCnt : content가 있는 page 개수
        {
            _currentPage++;
            _targetPos += _pageStep;
            MovePage();
        }
        else//맨 앞으로 가기
        {
            Debug.Log($"_currentPage {_currentPage}");
            _currentPage = 1;
            _targetPos += _pageStep * _ratio;//반만 가다, 순간이동 한 다음 -_pageStep/2해야함
            MovePage(Ease.InCubic, Move.gotoFrontAgain, _tweenTime / 2f);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > _dragThreshold)
        {
            if (eventData.position.x > eventData.pressPosition.x) Front();
            else Back();
        }
    }
    #endregion

    public void OnClickMatchPage(int npcOrder)//1 ~ contentPageCnt 중 몇 번째?
    {
        if (npcOrder > 0 && npcOrder <= _contentPageCnt)
            _currentPage = npcOrder;
        else
        {
            Debug.Log($"npcOrder > 0 && npcOrder <= _contentPageCnt {npcOrder}, {_contentPageCnt}");
            _currentPage = _contentPageCnt;
        }
        

        Vector3 pagesStep = _pageStep * _currentPage;
        _npcPagesRect.localPosition = _firstVacantPageLocation + pagesStep;
        _targetPos = _npcPagesRect.localPosition;
    }

    private void Start()
    {
        _contentPageCnt = _npcPagesRect.childCount - 2;
    }

    private void MovePage(Ease tweenType = Ease.InOutCubic, Move resetAfterMove = Move.none, float customTweenTime = -1f)
    {
        float time = customTweenTime > 0 ? customTweenTime : _tweenTime;

        _npcPagesRect.DOLocalMove(_targetPos, time).SetEase(_tweenType).OnComplete(() => {
            if (resetAfterMove == Move.gotoBackAgain)
            {
                _npcPagesRect.localPosition = _firstVacantPageLocation + _pageStep * (_contentPageCnt + _ratio);
                _targetPos = _npcPagesRect.localPosition - _pageStep * _ratio;
                MovePage(Ease.InCubic, 0, _tweenTime / 1.7f);
            }

            else if (resetAfterMove == Move.gotoFrontAgain)
            {
                _npcPagesRect.localPosition = _firstVacantPageLocation + _pageStep * (1 - _ratio);
                _targetPos = _npcPagesRect.localPosition + _pageStep * _ratio;
                MovePage(Ease.InCubic, 0, _tweenTime / 1.7f);
            }
        });
    }
}
