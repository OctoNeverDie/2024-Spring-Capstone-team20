using System.Collections.Generic;
using System;
using UnityEngine;

#region Item

[Serializable]
public class ItemInfo
{
    public int ObjID;
    public string ObjName;
    public string ObjInfo;
    public int defaultPrice;
    public int expensive;
    public int tooExpensive;
}

[Serializable]
public class ItemData : ILoader<int, ItemInfo>
{
    public List<ItemInfo> items = new List<ItemInfo>();

    public Dictionary<int, ItemInfo> MakeDict()
    {
        Dictionary<int, ItemInfo> dict = new Dictionary<int, ItemInfo>();

        foreach (ItemInfo Item in items)
        {
            dict.Add(Item.ObjID, Item);
            Debug.Log($"Key: {Item.ObjID}, ObjName: {Item.ObjName}, ObjInfo: {Item.ObjInfo}, " );
        }
        
        LogDictionary( dict );
        return dict;
    }
    private void LogDictionary(Dictionary<int, ItemInfo> dict)
    {
        foreach (KeyValuePair<int, ItemInfo> Item in dict)
        {
            string logMessage = $"Key: {Item.Key}, Value: {{ ObjID: {Item.Value.ObjID}, ObjName: {Item.Value.ObjName}, ObjInfo: {Item.Value.ObjInfo}, " +
                                $"defaultPrice: {Item.Value.defaultPrice}, expensive: {Item.Value.expensive}, tooExpensive: {Item.Value.tooExpensive} }}";

            Debug.Log(logMessage);
        }
    }
}
#endregion