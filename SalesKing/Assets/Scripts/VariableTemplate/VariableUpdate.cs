using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableUpdate : MonoBehaviour
{
    public void updateThings(int addAffinity, int addUsefulness)
    {
        VariableList.S_Affinity += addAffinity;
        VariableList.S_Usefulness += addUsefulness;

        calculateExpectedPrice(); 
        calculateAlphaPrice(); 
        calculateRelationship();

        calculateAffordable();
    }

    //-------------------------------------------------------------------------
    private void calculateAffordable()
    {
        VariableList.S_AffordablePrice = VariableList.S_ExpectedPrice + VariableList.S_AlphaPrice;
    }

    private void calculateAlphaPrice()
    {
        VariableList.S_AlphaPrice += VariableList.S_Affinity;
        Debug.Log($"addAffinity�� ������ ���� AlphaPrice ���� : {VariableList.S_AlphaPrice}, {VariableList.S_Affinity}");
    }

    private void calculateRelationship()
    {
        if (VariableList.S_Affinity > 0)
            VariableList.S_Relationship = "like";
        else
            VariableList.S_Relationship = "unlike";
        Debug.Log("addAffinity�� ������ ���� Relationship ����");
    }

    private void calculateExpectedPrice()
    {
        VariableList.S_ExpectedPrice += VariableList.S_Usefulness;
        Debug.Log($"userfulness�� ������ ���� expectedprice�� ����: {VariableList.S_ExpectedPrice}, {VariableList.S_Usefulness}");
    }

}
