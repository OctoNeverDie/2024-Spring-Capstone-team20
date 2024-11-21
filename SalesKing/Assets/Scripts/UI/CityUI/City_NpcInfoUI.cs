using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// initiate the npc infos
/// </summary>
public class City_NpcInfoUI : MonoBehaviour
{
    [SerializeField] NpcInfoRectrans[] npcs;
    int npcsCount = 0;
    Dictionary<int, int> NpcIDToUIIdx = new Dictionary<int, int>();

    public void InitNpc(NpcInfo npc, string coloredMbti, Sprite sprite = null)
    {
        int i = npcsCount++;
        NpcIDToUIIdx.Add(npc.NpcID, i);

        npcs[i].Name.text = $"고객 정보 : {npc.NpcName} ( {npc.NpcAge}세 )";
        npcs[i].Persuasion.text = $"설득 유형 : <size=35>{coloredMbti}</size>";
        npcs[i].Keyword.text = $"키워드 : {npc.KeyWord}";

        if (sprite != null)
            npcs[i].profileImg.sprite = sprite;
        Debug.Log("TODO : 그림 넣기");
        npcs[i].concern.text = $"{npc.Concern}\n그래서 {npc.WantItem}을(를) 사고 싶어함.";
        npcs[i].item.text = "당신이 갖고 있는 아이템 : ???";
    }

    public void UpdateItemData(string randItem, int thisNpcID)
    {
        int idx = NpcIDToUIIdx[thisNpcID];
        npcs[idx].item.text = $"당신이 갖고 있는 아이템 : <color=red>{randItem}</color>"; ;
    }
}