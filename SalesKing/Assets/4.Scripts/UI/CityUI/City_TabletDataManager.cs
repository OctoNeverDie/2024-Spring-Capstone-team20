using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class City_TabletDataManager : Singleton<City_TabletDataManager>, ISingletonSettings
{
    [SerializeField] City_TabletMovement tabletMovement;
    [SerializeField] City_NpcInfoUI npcInfoUI;
    [SerializeField] City_SummaryUI summaryUI;
    [SerializeField] GameObject customerScroll;
    [SerializeField] StoryNpcSO storyNpcSO;
    [SerializeField] NpcLookSO npcLookSO;

    private int today = -1;
    public Dictionary<int, NpcInfo> todaysIDdict = new Dictionary<int, NpcInfo>();
    public List<int> npcIDs = new List<int>();//오늘의 npcs, 순서대로 태블릿에 들어감

    public bool ShouldNotDestroyOnLoad => false;

    private void Start()
    {
        if(MuhanNpcDataManager.Instance==null)
            InitNpc(true);
    }

    public void InitNpc(bool isStory)
    {
        if(isStory)
        {
            today = DataController.Instance.playData.cur_day_ID;
            npcIDs = storyNpcSO.storyNpcs[today].npc_IDs;
        }
        else
        {
            npcIDs = MuhanNpcDataManager.Instance.npc_IDs;
        }

        for (int i = 0; i < npcIDs.Count; i++)
        {
            NpcInfo npc = DataGetter.Instance.NpcList
                    .Where(n => n.NpcID == npcIDs[i])
                    .FirstOrDefault(); // 해당 요소 반환 또는 null
            todaysIDdict.Add(npcIDs[i], npc);
            Debug.Log($"todaysIDdict.Add(npcIDs[i], npc);, {npcIDs[i]}, {npc.NpcName}");

            string colorPersuasion = ColorPersuasion(npc.Mbtis);
            Sprite npcProfile = npcLookSO.npcLooks
                    .Where(n => n.npcId == npcIDs[i]) // npcId가 npcIDs[i]와 일치하는 항목 필터링
                    .Select(n => n.npcProfileImg) // 필터링된 항목에서 npcProfileImg 속성 선택
                    .FirstOrDefault(); // 첫 번째 항목 반환, 없으면 null

            npcInfoUI.InitNpc(npc, colorPersuasion, npcProfile);
            summaryUI.InitNpc(npc, npcProfile, today);
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
                mbtiType = "구걸";
                break;
            case 1:
                mbtiType = "논리";
                break;
            case 2:
                mbtiType = "아부";
                break;
            case 3:
                mbtiType = "관계형성";
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
        npcInfoUI.UpdateItemData(randItem.ObjName, thisNpcID);
    }

    public void UpdateEvaluationData(string Evaluation, int thisNpcID, bool isBuy)
    {
        summaryUI.UpdateEvaluationData(Evaluation, thisNpcID, isBuy);
    }

    public void ShowSummaryOrInfo(bool isSummary)
    {
        npcInfoUI.gameObject.SetActive(false);
        customerScroll.SetActive(!isSummary);
        summaryUI.gameObject.SetActive(isSummary);
        tabletMovement.OnClickShowTablet();
    }
}
