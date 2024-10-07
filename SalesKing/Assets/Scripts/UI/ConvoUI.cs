using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConvoUI : MonoBehaviour
{
    public GameObject TalkOrNotPanel;
    public GameObject ChooseItemPanel;
    public GameObject ConvoPanel;
    public GameObject ItemSoldPanel;
    public GameObject YoufailedPanel;
    public GameObject UnniiEndpoint;

    public TMP_InputField UserText;
    public GameObject NPCSpeechBubble;
    public TextMeshProUGUI NPCSpeechText;

    public TextMeshProUGUI StatusText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI CashText;

    private void Start()
    {
        ChatManager.OnPanelUpdated -= ShowPanel;
        ChatManager.OnPanelUpdated += ShowPanel;
    }

    #region 대화 시작하겠습니까?
    public void OnClickYesTalkBtn()
    {
        ConvoPanel.SetActive(true);
        TalkOrNotPanel.SetActive(false);
    }

    public void OnClickNoTalkBtn()
    {
        Managers.Convo.ConvoFinished();
        TalkOrNotPanel.SetActive(false);
        ConvoPanel.SetActive(false);
    }
    #endregion


    public void ShowPanel(Define.SendChatType sendChatType)
    {
        if (sendChatType == Define.SendChatType.ItemInit)
        {
            ConvoPanel.SetActive(false);
            ChooseItemPanel.SetActive(true);
        }
        else if (sendChatType == Define.SendChatType.ChatBargain)
        {
            ConvoPanel.GetComponentInChildren<IDeal>().gameObject.SetActive(true);
        }
        else if (sendChatType == Define.SendChatType.Success)
        {
            ItemSoldPanel.SetActive(true);
        }
        else if (sendChatType == Define.SendChatType.Fail)
        { 
            YoufailedPanel.SetActive(true);
        }

    }

    public void OnClickSelectItemBtn()
    {
        ChooseItemPanel.SetActive(false);
        ConvoPanel.SetActive(true);
        //ConvoPanel.GetComponentInChildren<Button>()
    }

    #region 물건 사기
    public void OnClickBuy()
    {
        Managers.Chat.CheckTurnSuccess();
        
        //ServerManager.Instance.GetGPTReply("$buy", SendChatType.Success);
        this.gameObject.SetActive(false);
    }

    public void OnEndChat()
    {
        Managers.Chat.Clear();
        this.gameObject.SetActive(false);
        UnniiEndpoint.SetActive(true);
    }
    #endregion
    public void OnPriceClick(TMP_InputField inputFieldGO)
    {
        ItemInfo _itemInfo = ForItemInfoMockData();

        float inputPrice;

        if (float.TryParse(inputFieldGO.GetComponent<TMPro.TMP_InputField>().text, out inputPrice))
        {
            VariableList.InitItem(inputPrice, _itemInfo);
            //itemPrice.SetActive(false);
        }
        else
        {
            Debug.LogError("It's not float type");
        }
    }
    private ItemInfo ForItemInfoMockData()
    {
        ItemInfo _itemInfo = new ItemInfo();

        _itemInfo.ObjID = 1;
        _itemInfo.ObjName = "사탕";
        _itemInfo.ObjInfo = "딸기맛 사탕이다.";
        _itemInfo.defaultPrice = 5;
        _itemInfo.expensive = 10;
        _itemInfo.tooExpensive = 50;

        return _itemInfo;
    }

    public void OnClickTalkToNPCBtn()
    {

    }

    public void OnClickExitNPCBtn()
    {

    }

}

