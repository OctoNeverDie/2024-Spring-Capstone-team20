using static Define;
/// <summary>
/// 1. turn 3번인지 확인
/// 2. summary pop up
/// 3. save 파일에 전부 저장
/// </summary>
public class EndPointState : ChatBaseState
{
    public override void Enter()
    {
        _sendChatType = SendChatType.Endpoint;
        Chat.NpcCountUp();
        
        ShowFront();
    }

    public override void Exit(){}


    private void ShowFront()
    {
        Chat.ActivatePanel(_sendChatType);
    }
}
