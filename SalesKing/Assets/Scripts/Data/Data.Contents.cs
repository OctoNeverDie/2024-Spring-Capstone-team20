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
    public Define.ItemCategory Category;
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
    public string KeyWord;
    public string Concern;
    public string WantItem;
    public Define.ItemCategory ItemCategory;
    public int [] Mbtis;

    //prompt needed
    public string SituationDescription;
    public string Personality;
    public string DialogueStyle;
    public string Example;

    public NpcInfo(int npcID, string npcName, string npcSex, int npcAge, string keyWord, string concern, string wantItem, int[] mbtis, string situationDescription, string personality, string dialogueStyle, string example)
    {
        NpcID = npcID;
        NpcName = npcName;
        NpcSex = npcSex;
        NpcAge = npcAge;
        KeyWord = keyWord;
        Concern = concern;
        WantItem = wantItem;
        Mbtis = mbtis;
        SituationDescription = situationDescription;
        Personality = personality;
        DialogueStyle = dialogueStyle;
        Example = example;
    }

    public NpcInfo(NpcInfo other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        NpcID = other.NpcID;
        NpcName = other.NpcName;
        NpcSex = other.NpcSex;
        NpcAge = other.NpcAge;
        KeyWord = other.KeyWord;
        Concern = other.Concern;
        WantItem = other.WantItem;
        ItemCategory = other.ItemCategory;
        Mbtis = other.Mbtis != null ? (int[])other.Mbtis.Clone() : null;

        SituationDescription = other.SituationDescription;
        Personality = other.Personality;
        DialogueStyle = other.DialogueStyle;
        Example = other.Example;
    }
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