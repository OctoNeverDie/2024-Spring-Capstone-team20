using System.Collections.Generic;
using System;
using UnityEngine;

#region Item

[Serializable]
public class Item
{
    public int ObjID;
    public string ObjName;
    public string ObjInfo;
    public int defaultPrice;
    public int expensive;
    public int tooExpensive;
}

[Serializable]
public class ItemData : ILoader<int, Item>
{
    public List<Item> Items = new List<Item>();

    public Dictionary<int, Item> MakeDict()
    {
        Dictionary<int, Item> dict = new Dictionary<int, Item>();

        foreach (Item Item in Items)
        {
            dict.Add(Item.ObjID, Item);
            Debug.Log($"Key: {Item.ObjID}, ObjName: {Item.ObjName}, ObjInfo: {Item.ObjInfo}, " );
        }
        return dict;
    }
}
#endregion