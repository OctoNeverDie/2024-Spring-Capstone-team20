using System;
using UnityEngine;
using static Define;

public class ChatManager : MonoBehaviour
{
    private ChatStateMachine _chatStateMachine;
    [SerializeField] DailyInitData _dailyInitData;
    private int npcOrder = 0;

    public ReplySubManager ReplyManager = new ReplySubManager();
    public EvalSubManager EvalManager = new EvalSubManager();
    public NpcSupplyManager npcSupplyManager = new NpcSupplyManager();

    public static event Action<SendChatType, EndType> OnPanelUpdated;
    public void Init()
    {
        _chatStateMachine = new ChatStateMachine();
        _chatStateMachine.SetState(new NpcInitState());
    }

    public void ActivatePanel(SendChatType chatState)
    {
        if (chatState == SendChatType.Endpoint)
        {
            //endtype따라 마지막 패널 달라진다!
            OnPanelUpdated?.Invoke(chatState, _endType);
        }
        else
        {
            OnPanelUpdated?.Invoke(chatState, EndType.None);
        }
    }

    [HideInInspector]
    public int _turn;
    [HideInInspector]
    public float _npcSuggest =0;
    [HideInInspector]
    public float _userSuggest =0;
    [HideInInspector]
    public int reason =0;
    //1 :NPC 기분이 나빠서 Fail
    //2: 대화 에너지 다 해서 Fail
    //3 : 제시가가 판매가보다 낮아서 Success
    //4 : 이대로 받기를 선택해서 Success

    public static event Action<int, float, float> OnNumberUpdated;

    public void UpdateTurn(int turn, float npcSuggest = -1f, float userSuggest = -1f)
    { 
        _turn = turn;
        if (npcSuggest != -1f)
            _npcSuggest = npcSuggest;
        if (userSuggest != -1f)
            _userSuggest = userSuggest;

        float smaller = (_npcSuggest > _userSuggest) ? _userSuggest : _npcSuggest;
        EvalManager.UpdateSuggestInEval(smaller);

        //Panel에 남은 turn 수 출력, 서로 제시한 거 출력
        Debug.Log($"{_turn}+{_npcSuggest}+{_userSuggest}");
        if (npcSuggest <= 0f && userSuggest <= 0f)
            return;
        OnNumberUpdated?.Invoke(_turn, _npcSuggest, _userSuggest);
    }

    public void EndTurn(int turn)
    {
        OnNumberUpdated?.Invoke(turn, _npcSuggest, _userSuggest);
    }

    public EndType _endType { get; set; }

    public void Clear()
    {
        ReplyManager.ClearReplyData();
        OnNumberUpdated?.Invoke(0, 0, 0);
        Debug.Log($"지워 : ++++여기 들어가면 대화 끝났다고 보면 돼!!!!");
    }

    public void TransitionToState(SendChatType sendChatType)
    {
        _chatStateMachine.TransitionToState(sendChatType);
    }

    public string RatePrice(float userSuggest, ItemInfo itemInfo=null)
    {
        if (itemInfo == null)
        {
            itemInfo = EvalManager.itemInfo;
        }
        /*
        string expensiveRate;

        if (userSuggest < itemInfo.defaultPrice)
            expensiveRate = "Affordable";
        else if (userSuggest < itemInfo.expensive)
            expensiveRate = "Soso, Not that Cheap, not that Expensive. 시장가다.";
        else if (userSuggest < itemInfo.tooExpensive)
            expensiveRate = "Expensive, 시장가보다 조금 비싼 가격이다.";
        else
            expensiveRate = "Too Expensive, 시장가보다 많이 비싼 가격이다.";
        return expensiveRate;*/
        return "임시";
    }

}
