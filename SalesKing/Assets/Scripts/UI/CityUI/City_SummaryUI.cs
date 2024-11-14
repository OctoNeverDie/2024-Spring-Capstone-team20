using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class City_SummaryUI : MonoBehaviour
{
    [SerializeField] GameObject[] npcs;
    private List<NpcUI> npcUIs;
    private int npcsIdx=0;
    
    private class NpcUI
    {
        public int NpcID;
        public TextMeshProUGUI txtName;
        public TextMeshProUGUI txtPersuasion;
        public TextMeshProUGUI txtSummary;
        public TextMeshProUGUI txtItem;
        public Image profile;
    }
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        foreach (var npc in npcs)
        {
            NpcUI npcUI = new NpcUI();
            npcUI.txtName = npc.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
            npcUI.txtPersuasion = npc.transform.Find("Persuasion")?.GetComponent<TextMeshProUGUI>();
            npcUI.txtSummary = npc.transform.Find("Evaluation")?.GetComponent<TextMeshProUGUI>();
            npcUI.txtItem = npc.transform.Find("Item")?.GetComponent<TextMeshProUGUI>();
            npcUI.profile = npc.transform.Find("Profile")?.GetComponent<Image>();
            npcUIs.Add(npcUI);
        }
    }

    public void UpdateItemData(ItemInfo randItem, int thisNpcID)
    {
        if (npcsIdx > npcs.Length)
        {
            Debug.Log("4명째임.쓸 수 없는 summary 프로필이 없삼.");
            return;
        }
        npcUIs[npcsIdx].NpcID = thisNpcID;
        npcUIs[npcsIdx].txtItem.text = randItem.ObjName;
        npcsIdx++;
    }

    public void UpdateEvaluationData(int npcID, string summary)
    {
        //search where npcUIs[?]'s npcID == npcID,
        //that thing's txtSummary.text=summary;
        var npcUI = npcUIs.FirstOrDefault(n => n.NpcID == npcID);
        npcUI.txtSummary.text = summary;
    }
}
