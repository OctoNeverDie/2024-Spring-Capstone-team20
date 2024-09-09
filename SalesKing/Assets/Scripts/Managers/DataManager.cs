using System;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{ 
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, ItemInfo> ItemDict { get; private set; } = new Dictionary<int, ItemInfo>();
    public void Init()
    {
        ItemDict = LoadJson<ItemData, int, ItemInfo>("ItemData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
