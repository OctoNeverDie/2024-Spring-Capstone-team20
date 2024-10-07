using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using static ServerBase;

public class ChatManager : MonoBehaviour
{
    private ChatStateMachine _chatStateMachine;
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
        if (chatState == SendChatType.ChatSale)
        {
            //_chatPanel.SetActive(true);
        }
        else if (chatState == (SendChatType.ItemInit))
        {
            OnPanelUpdated?.Invoke(chatState, _endType);
            //_itemPanel.SetActive(true);
        }
        else if (chatState == SendChatType.ChatBargain)
        {
            OnPanelUpdated?.Invoke(chatState, _endType);
            //TODO : 2초 뒤에 흥정시작~! panel 나오고 점점 fade out
            //위에 거 이미 했으면, 한 1초 뒤에 Deal panel 나오게.
        }
        else if (chatState == SendChatType.Endpoint)
        {
            /*
             * fail
            if (!_confirmPanel.activeSelf && !previousStateIfDiff)
            {
                //TODO : 1초 뒤 생성
                _confirmPanel.SetActive(true);
            }
            else
            {
                _confirmPanel.SetActive(false);
                _chatPanel.SetActive(false);
                _endPanel.SetActive(true);
            }*/
            OnPanelUpdated?.Invoke(chatState, _endType);
        }
    }

    public void UpdatePanel(string gptOutput)
    {
        //
    }

    private int _turn;
    private float _npcSuggest;
    private float _userSuggest;
    public void UpdateTurn(int turn, float npcSuggest = -1.37f, float userSuggest = -1.37f)
    { 
        _turn = turn;
        if (npcSuggest != -1.37f)
            _npcSuggest = npcSuggest;
        if(userSuggest != -1.37f)
            _userSuggest = userSuggest;
        //TODO : Panel에 남은 turn 수 출력, 서로 제시한 거 출력
    }

    public EndType _endType { get; private set; }

    public void CheckTurnEndpoint(EndType endType)
    {
        _endType = endType;
        TransitionToState(SendChatType.Endpoint);
    }

    public void Clear()
    {
        VariableList.ClearStaticData();
        _endType = EndType.None;
        _chatStateMachine.EndStateMachine();
    }

    public void TransitionToState(SendChatType sendChatType)
    {
        _chatStateMachine.TransitionToState(sendChatType);
    }

    public string RatePrice(float userSuggest, ItemInfo itemInfo=null)
    {
        if (itemInfo == null)
        {
            itemInfo = VariableList.S_itemInfo;
        }

        string expensiveRate = "";

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
