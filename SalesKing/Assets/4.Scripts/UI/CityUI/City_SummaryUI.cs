using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// initiate the npc summary
/// </summary>
public class City_SummaryUI : MonoBehaviour
{
    [SerializeField] NpcEvalRectrans[] npcs;
    [SerializeField] GameObject Empty;
    [SerializeField] TextMeshProUGUI goodCnt;
    [SerializeField] TextMeshProUGUI badCnt;
    [SerializeField] Sprite Success;
    [SerializeField] Sprite Failed;

    int npcsCount = 0;
    int successCnt = 0;
    int failCnt = 0;
    Dictionary<int, int> NpcIDToUIIdx = new Dictionary<int, int>();

    public void InitNpc(NpcInfo npc, Sprite sprite = null, int today = -1)
    {
        int i = npcsCount++;
        NpcIDToUIIdx.Add(npc.NpcID, i);//npc ui object와 npc id를 매칭한 걸 기록한 dictionary

        npcs[i].SuccessImg.gameObject.SetActive(false);
        npcs[i].Name.text = npc.NpcName;
        npcs[i].Day.text = "Day "+ ((today)==-1? "무한" : today+1);


        if (sprite != null)
            Util.ChangeSprite(npcs[i].ProfileImg, sprite);

        npcs[i].Evaluation.text = "";
        goodCnt.text = "0";
        badCnt.text = "0";
        npcs[i].gameObject.SetActive(false);
        Empty.gameObject.SetActive(true);
    }

    public void UpdateEvaluationData(string summary, int thisNpcID, bool isBuy)
    {
        int idx = NpcIDToUIIdx[thisNpcID];
        npcs[idx].Evaluation.text = summary;

        if (isBuy)
        {
            npcs[idx].SuccessImg.color = Color.green;
            Util.ChangeSprite(npcs[idx].SuccessImg, Success);

            goodCnt.text = (++successCnt) +"";
        }
        else
        {
            npcs[idx].SuccessImg.color = Color.red;
            Util.ChangeSprite(npcs[idx].SuccessImg, Failed);

            badCnt.text = (++failCnt) + "";
        }

        npcs[idx].SuccessImg.gameObject.SetActive(true);
        npcs[idx].gameObject.SetActive(true);
        Empty.gameObject.SetActive(false);
    }
}
