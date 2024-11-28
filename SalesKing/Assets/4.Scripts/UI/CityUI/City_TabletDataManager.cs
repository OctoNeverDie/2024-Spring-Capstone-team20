using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class City_TabletDataManager : MonoBehaviour
{
    [SerializeField] City_TabletMovement tabletMovement;
    [SerializeField] City_NpcInfoUI npcInfoUI;
    [SerializeField] City_SummaryUI summaryUI;
    [SerializeField] StoryNpcSO storyNpcSO;
    [SerializeField] NpcLookSO npcLookSO;

    public Dictionary<int, NpcInfo> todaysIDdict = new Dictionary<int, NpcInfo>();
    private void Start()
    {
        InitNpc();
    }

    private void InitNpc()
    {
        int today = DataController.Instance.playData.cur_day_ID;
        List<int> npcIDs = storyNpcSO.storyNpcs[today].npc_IDs;

        for (int i = 0; i < npcIDs.Count; i++)
        {
            NpcInfo npc = DataGetter.Instance.NpcList
                    .Where(n => n.NpcID == npcIDs[i])
                    .FirstOrDefault(); // 해당 요소 반환 또는 null
            todaysIDdict.Add(npcIDs[i], npc);

            string colorPersuasion = ColorPersuasion(npc.Mbtis);
            Sprite npcProfile = npcLookSO.npcLooks
                    .Where(n => n.npcId == npcIDs[i]) // npcId가 npcIDs[i]와 일치하는 항목 필터링
                    .Select(n => n.npcProfileImg) // 필터링된 항목에서 npcProfileImg 속성 선택
                    .FirstOrDefault(); // 첫 번째 항목 반환, 없으면 null

            npcInfoUI.InitNpc(npc, colorPersuasion, npcProfile);
            summaryUI.InitNpc(npc, colorPersuasion, npcProfile);
        }
    }

    private string ColorPersuasion(int[] mbtis)
    {
        string persuasionStr = "";
        for (int i = 0; i < mbtis.Length; i++)
        {
            persuasionStr += MakeColor(i, mbtis[i]) + " ";
        }

        return persuasionStr;
    }
    private string MakeColor(int mbti, int prefer)
    {
        string mbtiType = "";
        string result = "";

        if (prefer == 0)
            return "";

        switch (mbti)
        {
            case 0:
                mbtiType = "감성";
                break;
            case 1:
                mbtiType = "논리";
                break;
            case 2:
                mbtiType = "아부";
                break;
            case 3:
                mbtiType = "유혹";
                break;
            default: break;
        }

        switch (prefer)
        {
            case -1:
                result = "<color=red>" + "-" + mbtiType + "</color>";
                break;
            case 1:
                result = "<color=green>" + "+" + mbtiType + "</color>";
                break;
            default: break;
        }

        return result;
    }

    public void UpdateItemData(ItemInfo randItem, int thisNpcID)
    { 
        summaryUI.UpdateItemData(randItem.ObjName, thisNpcID);
        npcInfoUI.UpdateItemData(randItem.ObjName, thisNpcID);
    }

    public void UpdateEvaluationData(string Evaluation, int thisNpcID, bool isBuy)
    {
        summaryUI.UpdateEvaluationData(Evaluation, thisNpcID, isBuy);
    }

    public void ShowSummary()
    {
        Debug.Log("?????????");
        npcInfoUI.gameObject.SetActive(false);
        summaryUI.gameObject.SetActive(true);
        tabletMovement.OnClickShowTablet();
    }
}
