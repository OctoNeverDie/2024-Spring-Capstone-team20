using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NpcSO", menuName = "SO/NpcSO")]
public class NpcSO : ScriptableObject
{
    public NpcInfo npcInfo;

    public void Initialize(NpcInfo data)
    {
        if (npcInfo == null) { npcInfo = new NpcInfo(); }

        npcInfo.NpcID = data.NpcID;
        npcInfo.NpcName = data.NpcName;
        npcInfo.NpcSex = data.NpcSex;
        npcInfo.NpcAge = data.NpcAge;
        npcInfo.NpcPersonality = data.NpcPersonality;
        npcInfo.NpcProplemType = data.NpcProplemType;
        npcInfo.NpcProblemInfo = data.NpcProblemInfo;
    }
}