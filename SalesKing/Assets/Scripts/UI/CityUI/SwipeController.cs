using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] Vector3 _scrollWidth = new Vector3(-1400f, 0, 0);
    [SerializeField] Vector3 _spacing = new Vector3(-1400, 0, 0);
    [SerializeField] RectTransform _npcPagesRect;

    [SerializeField] float _tweenTime = 0.8f;
    [SerializeField] float _ratio = 0.75f;
    [SerializeField] Ease _tweenType = Ease.InCubic;

    int _maxPage = 3;
    int _currentPage;
    Vector3 _targetPos;
    Vector3 _pageStep;
    Vector3 _circularPage;
    float _dragThreshold;
    private void Awake()
    {
        _currentPage = 1;
        _circularPage = _npcPagesRect.localPosition;
        _pageStep = _scrollWidth + _spacing;
        _dragThreshold = Screen.width / 10;

        _npcPagesRect.localPosition += _pageStep;
        _targetPos = _npcPagesRect.localPosition;
    }

    #region Scroll Input
    public void Front()
    {
        if (_currentPage > 1)
        {
            _currentPage--;
            _targetPos -= _pageStep;
            MovePage();
        }
        else//맨 뒤로 가기
        {
            _currentPage = _maxPage;
            _targetPos -= _pageStep / 2;
            MovePage(Ease.InCubic, 1, _tweenTime / 2f);
        }
    }

    public void Back()
    {
        if (_currentPage < _maxPage)
        {
            _currentPage++;
            _targetPos += _pageStep;
            MovePage();
        }
        else//맨 앞으로 가기
        {
            _currentPage = 1;
            _targetPos += _pageStep / 2;
            MovePage(Ease.InCubic, 2, _tweenTime / 2f);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > _dragThreshold)
        {
            if (eventData.position.x > eventData.pressPosition.x) Front();
            else Back();
        }
        else
        {
            MovePage();
        }
    }
    #endregion

    private void Start()
    {
        _maxPage = _npcPagesRect.childCount - 2;
    }

    private void MovePage(Ease tweenType = Ease.InOutCubic, int resetAfterMove = 0, float customTweenTime = -1f)
    {
        float time = customTweenTime > 0 ? customTweenTime : _tweenTime;

        _npcPagesRect.DOLocalMove(_targetPos, time).SetEase(_tweenType).OnComplete(() => {
            if (resetAfterMove == 1)
            {
                _npcPagesRect.localPosition = _circularPage + _pageStep * (_maxPage + _ratio);
                _targetPos = _npcPagesRect.localPosition - _pageStep * _ratio;
                MovePage(Ease.InCubic, 0, _tweenTime / 1.7f);
            }

            else if (resetAfterMove == 2)
            {
                _npcPagesRect.localPosition = _circularPage + (_pageStep * (1 - _ratio));
                _targetPos = _circularPage + _pageStep;
                MovePage(Ease.InCubic, 0, _tweenTime / 1.7f);
            }
        });
    }

    public void UpdateChapter(int currentChapter)
    {
        _currentPage = Mathf.Min(currentChapter, _maxPage);

        Vector3 pagesStep = _pageStep * (_currentPage - 1);
        _npcPagesRect.localPosition += pagesStep;
        _targetPos += pagesStep;
    }
}
