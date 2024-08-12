using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateReceive : VariableUpdate
{
    /*
        thought: ƼŸ������ ��������ٴ� �� ���� ���� ������ �� ����. �׷��� Ȥ�� �𸣴ϱ� ��¥���� ������߰ھ�., 
        reason: ������ ���� ���� ���������. (usefulness: +1), ������ ��Ƽ� �ʱⰡ �ߵ� �� ����. (usefulness: +1), 
        emotion: �߸�, 
        suggestedPrice: ?, 
        reaction: ��! ƼŸ������ ��������ٴ°� ����̶�� ���� ���� �� ���ƿ�. �Դٰ� ������ ��Ƽ� �ʱ⵵ �� �� �� �����. ���� ƼŸ������ ������� �� �³���?
     */

    public void StringConcat(string GPTanswer)
    {
        int addAffinity = Util.ExtractValue<int>(GPTanswer, @"affinity: ([+-]?\d+)");
        int addUsefulness = Util.ExtractValue<int>(GPTanswer, @"usefulness: ([+-]?\d+)");

        Debug.Log($"add affin, add usefunl {addAffinity}, {addUsefulness}");
        updateThings(addAffinity, addUsefulness);
    }
}
