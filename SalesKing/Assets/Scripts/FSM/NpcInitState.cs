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
        string npcInfo =
            "Name: 현민삐, Age: 24, Sex: Female"
            +"Situation_Description: 아빠가 최근에 회사를 그만두셨고, 그 이후로 집안 경제가 어려워졌어. 네가 할 수 있는 게 많지 않아서 무력감을 느끼고 있어. 게다가 너도 졸업을 앞두고 진로에 대한 고민이 많아.\n"
            + "Personality: 너는 독립적이면서도 깊이 공감하는 성격을 가지고 있어. 가족에 대한 책임감을 자주 느끼고, 그 때문에 무언가 잘못되면 부담감을 느껴. 스스로 해결책을 찾기를 선호하지만, 가족의 문제로 인해 너의 결정을 올바르게 내릴 수 있을지 자주 의심해. 낯선 사람에게는 특히 스트레스가 많은 상황에서 신중하게 대하지만, 진심 어린 소통을 좋아해. 초면에 말을 거는 상인이 싫지는 않고, 대화를 즐기며 흥미를 느껴.\n"
            + "Dialogue_Style: 기본적으로 정중한 존댓말을 사용하지만 불쾌하거나 위협을 받으면 반말을 사용해. 현재 집안에 돈이 없기 때문에 너는 많은 돈을 쓰고 싶지 않아. 물건의 질보다 가격에 집중하는 편이고, 만약 물건이 너무 비싸다면 넌 물건 사기를 포기할거야. 넌 말이 많고 솔직해서 하고 싶은 말이 있으면 바로바로 하는 편이야.";

        NpcID = 1;

        string _userSend= MakeAnswer(npcInfo);
        VariableList.InitNpcDict(NpcID, "현민삐", 24, true);

        _sendChatType = SendChatType.NpcInit;
        ChatManager.ChatInstance.TestReply("ChatSale");
        //ServerManager.Instance.GetGPTReply(_userSend, _sendChatType);
    }

    protected override string MakeAnswer(string user_send = "")
    {
        string initData = $"\n {user_send}";

        return initData;
    }
}
