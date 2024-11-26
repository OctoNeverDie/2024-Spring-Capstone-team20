using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// initiate the npc summary
/// </summary>
public class City_SummaryUI : MonoBehaviour
{
    [SerializeField] NpcEvalRectrans[] npcs;
    [SerializeField] Sprite Success;
    [SerializeField] Sprite Failed;

    int npcsCount = 0;
    Dictionary<int, int> NpcIDToUIIdx = new Dictionary<int, int>();

    public void InitNpc(NpcInfo npc, string coloredMbti, Sprite sprite = null)
    {
        int i = npcsCount++;
        NpcIDToUIIdx.Add(npc.NpcID, i);//npc ui object와 npc id를 매칭한 걸 기록한 dictionary

        npcs[i].SuccessImg.gameObject.SetActive(false);
        npcs[i].Name.text = npc.NpcName;
        npcs[i].Persuasion.text = coloredMbti;
        if (sprite != null)
            Util.ChangeSprite(npcs[i].ProfileImg, sprite);

        npcs[i].Item.text = "???";
        npcs[i].Evaluation.text = "";
    }

    public void UpdateItemData(string randItem, int thisNpcID)
    {
        int idx = NpcIDToUIIdx[thisNpcID];
        npcs[idx].Item.text = randItem;
    }

    public void UpdateEvaluationData(string summary, int thisNpcID, bool isBuy)
    {
        int idx = NpcIDToUIIdx[thisNpcID];
        npcs[idx].Evaluation.text = summary;

        if (isBuy)
        {
            npcs[idx].SuccessImg.color = Color.green;
            Util.ChangeSprite(npcs[idx].SuccessImg, Success);
        }
        else
        {
            npcs[idx].SuccessImg.color = Color.red;
            Util.ChangeSprite(npcs[idx].SuccessImg, Failed);
        }

        npcs[idx].SuccessImg.gameObject.SetActive(true);
    }
}
