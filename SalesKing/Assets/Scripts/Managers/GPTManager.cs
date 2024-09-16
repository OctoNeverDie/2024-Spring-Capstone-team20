using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPTManager : MonoBehaviour
{
    public void TalkToNPC(string text)
    {
        // ���⿡ gpt�� ������ �ڵ� ����
    }

    public void ReceiveNPCAnswer()
    {
        List<string> test_pos = new List<string> { "�� �׷�����.", "���Ͼ���", "����ؿ�~"};
        List<string> test_neg = new List<string> { "��? ���� ������?", "�־� �װ� �̻��ѵ���.", "����, ���� ����"};
        int pos_int = Random.Range(0, test_pos.Count);
        int neg_int = Random.Range(0, test_neg.Count);

        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            Managers.UI.SetNPCAnswerText(test_pos[pos_int]);
            Managers.NPC.curTalkingNPC.GetComponent<NPC>().PlayRandomNPCAnim(NPCDefine.AnimType.Positive);
        }
        else
        {
            Managers.UI.SetNPCAnswerText(test_neg[neg_int]);
            Managers.NPC.curTalkingNPC.GetComponent<NPC>().PlayRandomNPCAnim(NPCDefine.AnimType.Negative);
        }
        

        
    }
}
