using System;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<DataFormat>
{ List<DataFormat> GetList(); }

public class DataManager
{
    public List<ItemInfo> itemList = new List<ItemInfo>();
    public List<NpcInfo> npcList = new List<NpcInfo>();
    public List<ConcernInfo> concernList = new List<ConcernInfo>();

    public void Init()
    {
        itemList = LoadJson<ItemData, ItemInfo>("ItemData").GetList();
        npcList = LoadJson<NpcData, NpcInfo>("NpcData").GetList();
        concernList = LoadJson<ConcernData, ConcernInfo>("concernData").GetList();
    }

    Loader LoadJson<Loader, DataFormat>(string path) where Loader : ILoader<DataFormat>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/JsonFile/{path}");
        Debug.Log($"{textAsset.text}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
