/// <summary>
/// 혹시 모르니 UI들의 action과 ui를 이어주는 ui helper
/// </summary>
public class UIActionBinder : Singleton<UIActionBinder>, ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => true;

    public City_SummaryUI ui_summary;
    public City_TabletMovement ui_tablet;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    void Init()
    {

        Util.GetOrAddComponent<City_TabletMovement>(ui_tablet.gameObject);
    }
}
