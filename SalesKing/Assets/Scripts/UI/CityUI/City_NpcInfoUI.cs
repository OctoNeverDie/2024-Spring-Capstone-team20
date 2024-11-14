using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// initiate the npc infos
/// </summary>
public class City_NpcInfoUI : MonoBehaviour
{
    [SerializeField] NpcInfoRectrans[] npcs;
    Dictionary<int, int> IdtoGoIdx = new Dictionary<int, int>();

    public void InitNpc(int i, NpcInfo npc, string coloredMbti, Sprite sprite = null)
    {
        IdtoGoIdx.Add(npc.NpcID, i);

        npcs[i].Name.text = $"고객 정보 : {npc.NpcName} ( ${npc.NpcAge} ${npc.NpcSex} )";
        npcs[i].Persuasion.text = $"설득 유형 : ${coloredMbti}";
        npcs[i].Keyword.text = $"키워드 : ${npc.KeyWord}";

        if (sprite != null)
            npcs[i].profileImg.sprite = sprite;
        Debug.Log("TODO : 그림 넣기");
        npcs[i].concern.text = $"{npc.SituationDescription}\n{npc.Concern}\n{npc.WantItem}";
        npcs[i].item.text = "당신이 갖고 있는 아이템 : ???";
    }

    public void UpdateItemData(string randItem, int thisNpcID)
    {
        int idx = IdtoGoIdx[thisNpcID];
        npcs[idx].item.text = $"당신이 갖고 있는 아이템 : <color=red>{randItem}</color>"; ;
    }
}