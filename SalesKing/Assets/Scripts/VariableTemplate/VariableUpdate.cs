using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableUpdate : MonoBehaviour
{

    int fixedAdjustment = 2; //고정 증가값
    double percentageAdjustment = 0.05; //퍼센테이지 증가값

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
        VariableList.S_AlphaPrice = (int)(VariableList.S_Affinity);
        Debug.Log($"addAffinity�� ������ ���� AlphaPrice ���� : {VariableList.S_AlphaPrice}, {VariableList.S_Affinity}");
    }

    private void calculateRelationship()
    {
        int affinity = VariableList.S_Affinity;
        int neutralThreshold = 4;

        if (affinity < neutralThreshold*(-1)*3)
        {
            VariableList.S_Relationship = "fuckoff";
        }
        else if (affinity < neutralThreshold*(-1))
        {
            VariableList.S_Relationship = "dislike";
        }
        else if (affinity <= neutralThreshold) // Using <= 8 to include exactly 8 in the "neutral" category
        {
            VariableList.S_Relationship = "neutral";
        }
        else if (affinity < neutralThreshold*3)
        {
            VariableList.S_Relationship = "like";
        }
        else
        {
            VariableList.S_Relationship = "hotlike";
        }

        Debug.Log($"���� ����: {VariableList.S_Relationship}");
    }

    private void calculateExpectedPrice()
    {
        int totalAdjustment = VariableList.S_Usefulness * (fixedAdjustment + (int)Math.Ceiling(VariableList.S_DefaultPrice * percentageAdjustment));
        VariableList.S_ExpectedPrice = VariableList.S_DefaultPrice + totalAdjustment;
        Debug.Log($"userfulness�� ������ ���� expectedprice�� ����: {VariableList.S_ExpectedPrice}, {VariableList.S_Usefulness}");
    }

}
