using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
        int min;
        if (MuhanNpcDataManager.Instance != null)
        {
            min = MuhanNpcDataManager.Instance.npc_IDs.Min();//최솟값 찾기
        }
        else 
        {
            min = City_TabletDataManager.Instance.npcIDs.Min();
        }

        int order = npcID - min;
        swipeController.OnClickMatchPage(order);
        tabletMovement.OnClickShoworHideTablet();
    }
}
