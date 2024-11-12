/// <summary>
/// 혹시 모르니 UI들의 action과 ui를 이어주는 ui helper
/// </summary>
public class UIActionBinder : Singleton<UIActionBinder>, ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => true;

    public City_MainUI ui_main;
    public City_PopupUI ui_popup;
    public City_ChattingUI ui_chatting;
    public City_SummaryUI ui_summary;
    public City_TabletUI ui_tablet;
    public City_EndingUI ui_ending;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    void Init()
    {
        Util.GetOrAddComponent<City_MainAction>(ui_main.gameObject);
        Util.GetOrAddComponent<City_PopupAction>(ui_popup.gameObject);
        Util.GetOrAddComponent<City_ChattingAction>(ui_chatting.gameObject);
        Util.GetOrAddComponent<City_SummaryAction>(ui_summary.gameObject);
        Util.GetOrAddComponent<City_TabletAction>(ui_tablet.gameObject);
        Util.GetOrAddComponent<City_EndingAction>(ui_ending.gameObject);
    }
}
