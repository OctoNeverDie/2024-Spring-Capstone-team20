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

    //떠나기 버튼 누르면
    public void OnLeaveClickFSM()
    {
        Managers.Chat._endType = EndType.reject;
        Managers.Chat.TransitionToState(SendChatType.Endpoint);
    }

    //Deal 버튼 누르면
    public void OnClickDealFSM()
    {
        Managers.Chat.EvalManager.AddItemPriceSold();

        Managers.Chat._endType = EndType.buy;
        Managers.Chat.TransitionToState(SendChatType.Endpoint);
        //go to template recieve
        this.gameObject.SetActive(false);
    }
}
