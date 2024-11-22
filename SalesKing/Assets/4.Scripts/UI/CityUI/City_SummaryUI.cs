using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// initiate the npc summary
/// </summary>
public class City_SummaryUI : MonoBehaviour
{
    [SerializeField] NpcEvalRectrans[] npcs;
    int npcsCount = 0;
    Dictionary<int, int> NpcIDToUIIdx = new Dictionary<int, int>();

    public void InitNpc(NpcInfo npc, string coloredMbti, Sprite sprite = null)
    {
        int i = npcsCount++;
        NpcIDToUIIdx.Add(npc.NpcID, i);//npc ui object와 npc id를 매칭한 걸 기록한 dictionary
        Debug.Log($"{npc.NpcID}, {i} 만들었어요!");

        npcs[i].SuccessImg.gameObject.SetActive(false);
        npcs[i].Name.text = npc.NpcName;
        npcs[i].Persuasion.text = "<size=15>"+coloredMbti+"</size>";
        if(sprite!=null)
            npcs[i].ProfileImg.sprite = sprite;

        npcs[i].Item.text = "???";
        npcs[i].Evaluation.text = "";
    }

    public void UpdateItemData(string randItem, int thisNpcID)
    {
        Debug.Log($"원래 이거 0 아니야? {thisNpcID}");
        int idx = NpcIDToUIIdx[thisNpcID];
        npcs[idx].Item.text = randItem;
    }

    public void UpdateEvaluationData(string summary, int thisNpcID, bool isBuy)
    {
        npcs[thisNpcID].Evaluation.text = summary;
        npcs[thisNpcID].SuccessImg.color = isBuy? Color.green : Color.red;
        npcs[thisNpcID].SuccessImg.gameObject.SetActive(true);
    }
}
