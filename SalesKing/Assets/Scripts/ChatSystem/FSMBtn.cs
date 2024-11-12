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
    public void OnClickLeaveFSM()
    {
        Managers.Chat._endType = EndType.leave;
        Managers.Chat.TransitionToState(SendChatType.Endpoint);
    }

    //최종 버튼 누르면
    public void OnClickFinalFSM()
    {
        Managers.Turn.AddTurnAndCheckTalkTurn();
    }
}
