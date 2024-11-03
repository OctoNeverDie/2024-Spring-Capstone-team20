using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChecker
{
    public enum CheckerStage
    { 
        None,
        U_U,
        U_D,
        D_D,
        D_G_D,
        D_D2,
        D_U,
        U_U2
    }

    private static Dictionary<CheckerStage, float> TimeCheckList = new Dictionary<CheckerStage, float>();
    private static Dictionary<CheckerStage, float> StartCheckList = new Dictionary<CheckerStage, float>();

    public void StartTimeCheck(CheckerStage stage, float time)
    {
        if(StartCheckList[stage]!=0f)
            StartCheckList[stage] = time;

    }

    public void EndTimeCheck(CheckerStage stage, float time)
    { 
    }

    public void AddTimeCheck(CheckerStage stage, float time)
    {
        //TimeCheckList.Add((stage, time));
    }

    public void PrintTimes()
    {
        string timeDebug ="++++++++++++++++++";

        foreach (var times in TimeCheckList)
        {
            //timeDebug += $"\n{times.Item1} 단계에는 {times.Item2}초";
        }
        Debug.Log(timeDebug);
    }
}
