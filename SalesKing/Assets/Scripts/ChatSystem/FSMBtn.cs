using UnityEngine;
using UnityEngine.UI;
using static Define;

public class FSMBtn : MonoBehaviour
{
    //말을 건다 버튼 누르면
    public void OnClickYesTalkFSM()
    {
        //start fsm
        Managers.Chat.Init();
    }

    //NpcInit 고민 okay 버튼 누르면
    public void OnClickNpcInitFSM()
    {
        Managers.Chat.TransitionToState(SendChatType.ItemInit);
    }

    //떠나기 버튼 누르면
    public void OnLeaveClickFSM()
    {
        Managers.Chat._endType = EndType.leave;
        Managers.Chat.TransitionToState(SendChatType.Endpoint);
    }

    //Deal 버튼 누르면
    public void OnClickDealFSM()
    {
        Managers.Chat.EvalManager.AddItemPriceSold();

        Managers.Chat.reason = 4;
        Managers.Chat._endType = EndType.buy;
        Managers.Chat.TransitionToState(SendChatType.Endpoint);
        //go to template recieve
        this.gameObject.SetActive(false);
    }

    //최종 버튼 누르면
    public void OnClickFinalFSM()
    {
        Managers.Turn.AddTurnAndCheckTalkTurn();
    }
}
