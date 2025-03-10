using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class City_TabletDataManager : Singleton<City_TabletDataManager>, ISingletonSettings
{
    [SerializeField] City_TabletMovement tabletMovement;
    [SerializeField] City_NpcInfoUI npcInfoUI;
    [SerializeField] NewsSpawner newsSpawner;
    [SerializeField] GameObject customerScroll;
    [SerializeField] StoryNpcSO storyNpcSO;
    [SerializeField] NpcLookSO npcLookSO;

    private int today = -1;
    public Dictionary<int, NpcInfo> todaysIDdict;
    public List<int> npcIDs;//오늘의 npcs, 순서대로 태블릿에 들어감

    public bool ShouldNotDestroyOnLoad => true;

    private void Start()
    {
        todaysIDdict = new Dictionary<int, NpcInfo>();
        npcIDs = new List<int>();
        InitNpc(true);
    }

    public void InitNpc(bool isStory)
    {
        if(isStory)
        {
            today = DataController.Instance.gameData.cur_day_ID;
            Debug.Log($"today : {today}");
            npcIDs = storyNpcSO.storyNpcs[today].npc_IDs;
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
        }
    }

    private string ColorPersuasion(int[] mbtis)
    {
        string persuasionStr = "";
        for (int i = 0; i < mbtis.Length; i++)
        {
            string str = MakeColor(i, mbtis[i]);
            if (str == "")
                continue;

            persuasionStr += str;
        }

        if (persuasionStr.Length > 10)
            persuasionStr = persuasionStr.Substring(0, persuasionStr.Length - 10);//, <color> 뻬는 거임

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
                result = "<color=red>" + "-" + mbtiType + ", </color>";
                break;
            case 1:
                result = "<color=green>" + "+" + mbtiType + ", </color>";
                break;
            default: break;
        }

        return result;
    }

    public void UpdateItemData(string ObjName, int thisNpcID)
    { 

        npcInfoUI.UpdateItemData(ObjName, thisNpcID);
    }

    public void UpdateEvaluationData(NpcInfo thisNpc, string Evaluation, bool isBuy)
    {
        newsSpawner.UpdateEvaluationData(Evaluation, thisNpc, isBuy);
    }

    public void ShowDaySummary()
    {
        DataController.Instance.gameData.cleared_npc_count += newsSpawner.success;
        int day = DataController.Instance.gameData.cur_day_ID;
        newsSpawner.ShowNews(day);
    }
}
