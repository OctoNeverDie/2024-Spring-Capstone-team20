using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private void Update()
    {
        _chatStateMachine?.UpdateState();
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

    public int _turn;
    public float _npcSuggest =0;
    public float _userSuggest =0;

    public static event Action<int, float, float> OnNumberUpdated;

    public void UpdateTurn(int turn, float npcSuggest = -1.37f, float userSuggest = -1.37f)
    { 
        _turn = turn;
        if (npcSuggest != -1.37f)
            _npcSuggest = npcSuggest;
        if(userSuggest != -1.37f)
            _userSuggest = userSuggest;

        OnNumberUpdated?.Invoke(_turn, _npcSuggest, _userSuggest);
        //TODO : Panel에 남은 turn 수 출력, 서로 제시한 거 출력
    }

    public EndType _endType { get; set; }

    public void CheckTurnEndpoint(EndType endType)
    {
        _endType = endType;
        TransitionToState(SendChatType.Endpoint);
    }

    public void Clear()
    {
        ReplyManager.ClearReplyData();
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