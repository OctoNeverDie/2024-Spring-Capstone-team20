using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateReceive : VariableUpdate
{
    /*
        thought: 티타늄으로 만들어졌다니 이 펜은 정말 유용할 것 같아. 그래도 혹시 모르니까 진짜인지 물어봐야겠어., 
        reason: 물건이 좋은 재료로 만들어졌음. (usefulness: +1), 펜촉이 얇아서 필기가 잘될 것 같음. (usefulness: +1), 
        emotion: 중립, 
        suggestedPrice: ?, 
        reaction: 와! 티타늄으로 만들어졌다는게 사실이라면 쓸모가 있을 것 같아요. 게다가 펜촉이 얇아서 필기도 잘 될 것 같고요. 정말 티타늄으로 만들어진 게 맞나요?
     */

    public void StringConcat(string GPTanswer)
    {
        int addAffinity = Util.ExtractValue<int>(GPTanswer, @"affinity: ([+-]?\d+)");
        int addUsefulness = Util.ExtractValue<int>(GPTanswer, @"usefulness: ([+-]?\d+)");

        Debug.Log($"add affin, add usefunl {addAffinity}, {addUsefulness}");
        updateThings(addAffinity, addUsefulness);
    }
}
