using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// initiate the npc infos
/// </summary>
public class City_NpcInfoUI : MonoBehaviour
{
    [SerializeField] NpcInfoRectrans[] npcs;
    [SerializeField] NpcInfoSummaryRectrans[] npcSum;
    [SerializeField] int npcsCount = 0;

    Dictionary<int, int> NpcIDToUIIdx = new Dictionary<int, int>();
    string age;
    public void InitNpc(NpcInfo npc, string coloredMbti, Sprite sprite = null)
    {
        int i = npcsCount++;
        NpcIDToUIIdx.Add(npc.NpcID, i);

        age = npc.NpcAge == 0? "???" : npc.NpcAge.ToString();

        npcInit(npc, coloredMbti, i, sprite);
        npcSumInit(npc, coloredMbti, i, sprite);
    }

    public void UpdateItemData(string randItem, int thisNpcID)
    {
        int idx = NpcIDToUIIdx[thisNpcID];
        npcs[idx].item.text = $"당신이 갖고 있는 아이템 : <color=red>{randItem}</color>"; ;
    }

    private void npcInit(NpcInfo npc, string coloredMbti, int i, Sprite sprite = null)
    {
        npcs[i].Name.text = $"고객 정보 : {npc.NpcName} ( {age}세 )";
        npcs[i].Persuasion.text = $"설득 유형 : {coloredMbti}";
        npcs[i].Keyword.text = $"키워드 : {npc.KeyWord}";

        if (sprite != null)
            Util.ChangeSprite(npcs[i].profileImg, sprite);

        npcs[i].concern.text = $"{npc.Concern}\n그래서 {npc.WantItem}을(를) 사고 싶어함.";
        npcs[i].item.text = "당신이 갖고 있는 아이템 : ???";
    }

    private void npcSumInit(NpcInfo npc, string coloredMbti, int i, Sprite sprite = null)
    {
        npcSum[i].SuccessImg.gameObject.SetActive(false);
        npcSum[i].Name.text = npc.NpcName;
        npcSum[i].Age.text = age + "(" + (npc.NpcSex == "female" ? " 여" : "남") + ")";
        npcSum[i].Persuasion.text = coloredMbti;
        npcSum[i].Keyword.text = npc.KeyWord;
        npcSum[i].Item.text = npc.WantItem;

        if (sprite != null)
            Util.ChangeSprite(npcSum[i].ProfileImg, sprite);
    }

}