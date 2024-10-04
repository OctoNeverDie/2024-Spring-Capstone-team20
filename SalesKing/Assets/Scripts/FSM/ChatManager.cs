using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class ChatManager : MonoBehaviour
{
    #region Singletone
    public static ChatManager ChatInstance { get; private set; }
    private void Awake()
    {
        if (ChatInstance == null)
        {
            ChatInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    [SerializeField] private GameObject _chatPanel;//contains : logpanel, gptpanel, leavebutton, submitbutton, inputbutton
    [SerializeField] private GameObject _confirmPanel;
    [SerializeField] private GameObject _endPanel;
    
    private ChatStateMachine _chatStateMachine;
    private void Start()
    {
        _confirmPanel.SetActive(false);
        _endPanel.SetActive(false);
        _chatPanel.SetActive(false);

        _chatStateMachine = new ChatStateMachine();
        _chatStateMachine.SetState(new NpcInitState());
    }

    private void Update()
    {
        _chatStateMachine.UpdateState();
    }

    public void ActivatePanel(SendChatType chatState)
    {
        if (chatState == SendChatType.Fail)
        {
            if (!_confirmPanel.activeSelf)
            {
                _confirmPanel.SetActive(true); 
            }
            else
            {
                _confirmPanel.SetActive(false);
                _chatPanel.SetActive(false);
                _endPanel.SetActive(true);
            }
        }
        else if (chatState == SendChatType.Leave)
        {
            _chatPanel.SetActive(false);
            _endPanel.SetActive(true);
        }
        else if (chatState == SendChatType.ChatSale)
        {
            _chatPanel.SetActive(true);
        }
    }
    public void UpdatePanel(string gptOutput)
    {
        
    }

    //TODO : 나중에 지울 것.
    public void TestReply(String stateType, String input ="")
    {
        if (Enum.TryParse(stateType, out SendChatType sendChatType))
        {
            Debug.Log("Success");
            Debug.Log($"next stateType : {stateType}, input : {input}");
            _chatStateMachine.TransitionToState(sendChatType);
        }
        else
        {
            Debug.Log("Failed to parse enum");
        }
    }
}
