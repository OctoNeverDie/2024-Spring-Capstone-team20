using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using static UnityEditor.Progress;

#region Item

[Serializable]
public class ItemInfo
{
    public int ObjID;
    public string ObjName;
    public string ObjInfo;
    public int npcFirstSuggestPrice;
    public int expensive;
    public int tooExpensive;
}

public class ItemData : ILoader<ItemInfo>
{
    public List<ItemInfo> items = new List<ItemInfo>();
    public List<ItemInfo> GetList()
    => items;
}
#endregion

#region NPC
[Serializable]
public class NpcInfo
{
    public int NpcID;
    public string NpcName;
    public string NpcSex;
    public int NpcAge;
    public string Situation_Description;
    public string Personality;
    public string Dialogue_Style;
}

public class NpcData : ILoader<NpcInfo>
{
    public List<NpcInfo> npcs = new List<NpcInfo>();
    public List<NpcInfo> GetList()
    => npcs;

    protected void MakeSO()
    {
        //base.MakeDirectory("npcs");

        foreach (var npc in npcs)
        {
            MakeSOInstance(npc);
        }
    }
    protected void MakeSOInstance(NpcInfo npc)
    {
        NpcSO npcSO = ScriptableObject.CreateInstance<NpcSO>();
        npcSO.Initialize(npc);

#if UNITY_EDITOR
        //UnityEditor.AssetDatabase.CreateAsset(npcSO, $"{base.basePath}Npcs/{npcSO.npcInfo.NpcName}.asset");
        //UnityEditor.AssetDatabase.SaveAssets();
#endif
    }
}
#endregion