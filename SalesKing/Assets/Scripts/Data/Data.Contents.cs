using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

#region Item

[Serializable]
public class ItemInfo
{
    public int ObjID;
    public string ObjName;
    public string ObjInfo;
    public string Category;
}

public class ItemData : ILoader<ItemInfo>
{
    public List<ItemInfo> items = new List<ItemInfo>();
    public List<ItemInfo> GetList()
    => items;
}
#endregion

#region concern
[Serializable]
public class ConcernInfo
{
    public int ConcernID;
    public string Concern;
    public string Category;
}
public class ConcernData : ILoader<ConcernInfo>
{
    public List<ConcernInfo> concerns = new List<ConcernInfo>();
    public List<ConcernInfo> GetList()
    => concerns;
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
    public string KeyWord;
    public string SituationDescription;
    public string Personality;
    public string DialogueStyle;
    public string Example;
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