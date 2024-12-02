using UnityEngine;
using UnityEngine.UI;

public class TabletButton : MonoBehaviour
{
    [SerializeField] GameObject tablet;
    [SerializeField] GameObject PhonePanel;
    City_TabletMovement tabletMovement;
    SwipeController swipeController;

    void Awake()
    {
        swipeController = tablet.GetComponent<SwipeController>();
        tabletMovement = PhonePanel.GetComponent<City_TabletMovement>();

        this.GetComponent<Button>().onClick.AddListener(TabletMatch);
    }

    private void TabletMatch()
    {
        int npcID = ChatManager.Instance.ThisNpc.NpcID;
        swipeController.OnClickMatchPage(npcID);
        tabletMovement.OnClickShoworHideTablet();
    }
}
