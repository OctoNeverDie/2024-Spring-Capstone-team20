using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// initiate the npc summary
/// </summary>
public class City_SummaryUI : MonoBehaviour
{
    [SerializeField] NpcEvalRectrans[] npcs;
    Dictionary<int, int>IdtoGoIdx = new Dictionary<int, int>();

    public void InitNpc(int i, NpcInfo npc, string coloredMbti, Sprite sprite = null)
    {
        IdtoGoIdx.Add(npc.NpcID, i);

        npcs[i].Name.text = npc.NpcName;
        npcs[i].Persuasion.text = coloredMbti;
        if(sprite!=null)
            npcs[i].ProfileImg.sprite = sprite;

        npcs[i].Item.text = "???";
        npcs[i].Evaluation.text = "";
    }

    public void UpdateItemData(string randItem, int thisNpcID)
    {
        int idx = IdtoGoIdx[thisNpcID];
        npcs[idx].Item.text = randItem;
    }

    public void UpdateEvaluationData(string summary, int thisNpcID)
    {
        int idx = IdtoGoIdx[thisNpcID];
        npcs[idx].Evaluation.text = summary;
    }
}
