using TMPro;
using UnityEngine;

public class TestProfile : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI npcName;
    [SerializeField]
    TextMeshProUGUI npcAge;
    [SerializeField]
    TextMeshProUGUI npcSex;
    [SerializeField]
    TextMeshProUGUI npcPersonality;
    void Awake()
    {
        EvalSubManager.OnChatDataUpdated -= show;
        EvalSubManager.OnChatDataUpdated += show;
    }

    private void OnDestroy()
    {
        EvalSubManager.OnChatDataUpdated -= show;
    }

    void show(string name)
    {
        Debug.Log($"{name} 돼!");
        var eval = Managers.Chat.EvalManager;
        if (name == nameof(eval.currentNpcId))
        {
            npcName.text = eval.NpcEvalDict[eval.currentNpcId].npcName;
            npcAge.text = eval.NpcEvalDict[eval.currentNpcId].npcAge.ToString();
            npcSex.text = eval.NpcEvalDict[eval.currentNpcId].npcSex ? "Female" : "Male";
            npcPersonality.text = "#순진한 #무속신앙믿는";
        }
    }
}
