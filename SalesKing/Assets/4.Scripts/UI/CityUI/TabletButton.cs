using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class TabletButton : MonoBehaviour
{
    [SerializeField] GameObject tablet;
    [SerializeField] GameObject PhonePanel;
    City_TabletMovement tabletMovement;
    SwipeController swipeController;
    List<int> npcIDs = new List<int>();

    void Awake()
    {
        swipeController = tablet.GetComponent<SwipeController>();
        tabletMovement = PhonePanel.GetComponent<City_TabletMovement>();

        this.GetComponent<Button>().onClick.AddListener(TabletMatch);
    }

    private void TabletMatch()
    {
        int npcID = ChatManager.Instance.ThisNpc.NpcID;

        if (MuhanNpcDataManager.Instance != null)
        {
            npcIDs = MuhanNpcDataManager.Instance.npc_IDs;//최솟값 찾기
        }
        else 
        {
            npcIDs = City_TabletDataManager.Instance.npcIDs;
        }

        int order = npcIDs.IndexOf(npcID);
        
        swipeController.OnClickMatchPage(order);
        City_TabletDataManager.Instance.ShowSummaryOrInfo(false);
        tabletMovement.OnClickShoworHideTablet();
    }
}
