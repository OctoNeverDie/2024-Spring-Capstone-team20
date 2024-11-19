using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartChattingMockData : MonoBehaviour
{
    private void Start()
    {
        ChatManager.Instance.Init(1);
    }
    public NpcInfo Encountered(int npcID = 0)
    {
        NpcInfo thisNpc;

        //thisNpc = DataGetter.Instance.npcList[npcID];
        thisNpc = new NpcInfo(0, "장난해", "여성", 25, "#똑부러지는 #야망있는",
            "기분이 안 좋아서 단 걸 먹고 싶어요.",
            "케이크",
            new int[] { 1, 0, 0, 0 },
            "갓 졸업하고 취업준비 중", 
            "똑부러지나, 현재 취준 중이라 약간 히스테릭하다.",
            "말투는 또박또박, 말 끝을 흐리지 않고 요점을 탁탁 잘 말한다. 무조건 존댓말. 그러나 취업 관련 이야기가 나오면 예민해지면서 반말을 쓴다.",
            "네, 제가 원하는 건 케이크에요. 다른 건 필요 없어요. 네? 직업이 뭐냐고요? 그런 건 왜 물어보는 거야?"
            );

        return thisNpc;
    }
}
