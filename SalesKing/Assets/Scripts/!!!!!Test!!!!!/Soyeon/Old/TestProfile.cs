using TMPro;
using UnityEngine;

public class TestProfile : MonoBehaviour
{
    [SerializeField]
    ChatManager chatManager;

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
        var eval = chatManager.EvalManager;
        if (name == nameof(eval.currentNpcId))
        {
            npcName.text = eval.NpcEvalDict[eval.currentNpcId].npcName;
            npcAge.text = eval.NpcEvalDict[eval.currentNpcId].npcAge.ToString();
            npcSex.text = eval.NpcEvalDict[eval.currentNpcId].npcSex ? "Female" : "Male";
            npcPersonality.text = eval.NpcEvalDict[eval.currentNpcId].npcKeyword;
        }
    }
}
