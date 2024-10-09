using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NpcInitState : ChatBaseState
{
    int NpcID;

    //말을 거시겠습니까? 클릭하면 일어남.
    public override void Enter()
    {
        //npcinfo로 받는다.

        string npcInfo =
            "Name: 곽제리, Age: 22, Sex: Female"
            + "Situation_Description: 너는 평범한 하루를 보내고 있을 때, 낯선 사람과 마주쳐 대화를 시작하게 돼. 그들은 친근하게 다가와서 이야기를 나누고 싶어 해. 처음엔 그들의 진짜 의도를 모르지만, 대화가 흥미롭게 진행될 수 있어. 그렇지만 대학생이라 그런지 과한 지출은 피하고 싶어해.\n"
            + "Personality: 너는 긍정적이고 활발한 성격을 가지고 있어. 사람들과 소통하는 것을 좋아하고, 언제나 열린 마음으로 대화를 시도해. 다른 사람의 의견을 존중하며, 소통 중에 자연스럽게 자신의 생각을 공유하는 것을 선호해. 실수나 비판에 대해 크게 개의치 않으며, 유머 감각이 뛰어나 대화를 유쾌하게 이끌어 나가는 편이야. 새로운 경험에 대한 호기심이 많아, 상인과의 대화에서 흥미를 느끼고 쉽게 마음을 열어.\n"
            + "Dialogue_Style: 일반적으로 친근하고 정중한 표현을 사용해. 대화가 어색해지지 않도록 자주 유머를 섞고, 상대방의 이야기에 귀 기울이며 반응을 잘 보인다. 물건을 살 기회가 생기면, 기분이 좋고 흥미롭게 반응해볼 수 있어. 재밌네요! 같은 말을 자주 하며, 솔직하게 자신의 생각을 전달하는 것을 중요시해.";

        NpcID = 2;

        string _userSend= MakeAnswer(npcInfo);
        Managers.Chat.EvalManager.InitNpcDict(NpcID, "곽제리", 22, true);

        _sendChatType = SendChatType.NpcInit;

        Debug.Log($"NpcInitState에서 보냄 {_sendChatType}");
        ServerManager.Instance.GetGPTReply(_userSend, _sendChatType);
    }

    protected override string MakeAnswer(string user_send = "")
    {
        string initData = $"\n {user_send}";

        return initData;
    }
}
