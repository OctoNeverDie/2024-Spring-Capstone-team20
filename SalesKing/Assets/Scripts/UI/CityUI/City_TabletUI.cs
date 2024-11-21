using UnityEngine;
using System.Collections.Generic; // DoTween 네임스페이스 추가

public class City_TabletUI : MonoBehaviour
{
    [SerializeField] City_TabletAction tabletAction;
    [SerializeField] City_NpcInfoUI npcInfoUI;
    [SerializeField] City_SummaryUI summaryUI;

    private List<int> npcIDs;
    private void Start()
    {
        Debug.Log("TODO : Tablet Mock, stage 확인하고, 해당되는 id 넣을 것");
        

        InitNpc();
    }

    private void InitNpc()
    {
        for (int i = 0; i < npcIDs.Count; i++)
        {
            NpcInfo npc = DataGetter.Instance.NpcList[npcIDs[i]];

            Debug.Log($"mbti : {npc.Mbtis}");
            string colorPersuasion = ColorPersuasion(npc.Mbtis);
            //npc sprite
            npcInfoUI.InitNpc(npc, colorPersuasion);
            summaryUI.InitNpc(npc, colorPersuasion);
        }
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
        npcInfoUI.gameObject.SetActive(false);
        summaryUI.gameObject.SetActive(true);
        tabletAction.OnClickShowTablet();
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
                result = "<color=red>" + mbtiType + "싫어" + "</color>";
                break;
            case 1:
                result = "<color=green>" + mbtiType + "좋아" + "</color>";
                break;
            default: break;
        }

        return result;
    }
}
