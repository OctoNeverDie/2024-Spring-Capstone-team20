using System.Collections.Generic;
using System;

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
    public int [] Mbtis;// 1 = like, 0 = norm, -1 = dislike

    //prompt needed
    public string Personality;
    public string DialogueStyle;
    public string Example;
}

public class NpcData : ILoader<NpcInfo>
{
    public List<NpcInfo> npcs = new List<NpcInfo>();
    public List<NpcInfo> GetList()
    => npcs;
}
#endregion