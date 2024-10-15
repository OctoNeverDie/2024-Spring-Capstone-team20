using System;
using UnityEngine;
using static Define;

public class ChatManager : MonoBehaviour
{
    private ChatStateMachine _chatStateMachine;
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
        if (chatState == (SendChatType.ItemInit))
        {
            OnPanelUpdated?.Invoke(chatState, EndType.None);
        }
        else if (chatState == SendChatType.ChatBargain)
        {
            OnPanelUpdated?.Invoke(chatState, EndType.None);
            //TODO : 2초 뒤에 흥정시작~! panel 나오고 점점 fade out
            //위에 거 이미 했으면, 한 1초 뒤에 Deal panel 나오게.
        }
        else if (chatState == SendChatType.Endpoint)
        {
            //endtype따라 마지막 패널 달라진다!
            OnPanelUpdated?.Invoke(chatState, _endType);
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

    [SerializeField]
    public VariableInput variableInput;
    public void GetInputKey()
    {
        if (variableInput == null)
        {
            Debug.Log("Null exception : variableInput이 비었습니다. ");
            variableInput = FindObjectOfType<VariableInput>();
        }
        variableInput.OnClick();
    }


    public static event Action<int, float, float> OnNumberUpdated;

    public void UpdateTurn(int turn, float npcSuggest = -1f, float userSuggest = -1.37f)
    { 
        _turn = turn;
        if (npcSuggest != -1f)
            _npcSuggest = npcSuggest;
        if(userSuggest != -1f)
            _userSuggest = userSuggest;

        float smaller = (_npcSuggest > _userSuggest) ? _userSuggest : _npcSuggest;
        EvalManager.UpdateSuggestInEval(smaller);

        //Panel에 남은 turn 수 출력, 서로 제시한 거 출력
        if (npcSuggest <= 0f && userSuggest <= 0f)
            return;

        Debug.Log($"{_turn}+{_npcSuggest}+{_userSuggest}");
        OnNumberUpdated?.Invoke(_turn, _npcSuggest, _userSuggest);
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

        string expensiveRate;

        if (userSuggest < itemInfo.defaultPrice)
            expensiveRate = "Affordable";
        else if (userSuggest < itemInfo.expensive)
            expensiveRate = "Soso, Not that Cheap, not that Expensive";
        else if (userSuggest < itemInfo.tooExpensive)
            expensiveRate = "Expensive, little bit upset about the price";
        else
            expensiveRate = "Too Expensive, you are angry about the price.";

        return expensiveRate;
    }
}
