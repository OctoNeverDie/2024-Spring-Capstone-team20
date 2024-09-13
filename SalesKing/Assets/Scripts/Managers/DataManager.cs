using System;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<DataFormat>
{ List<DataFormat> GetList(); }

public class DataManager
{
    public List<ItemInfo> itemList = new List<ItemInfo>();
    public List<NpcInfo> npcList = new List<NpcInfo>();
    public void Init()
    {
        itemList = LoadJson<ItemData, ItemInfo>("ItemData").GetList();
        npcList = LoadJson<NpcData, NpcInfo>("NpcData").GetList();
    }

    Loader LoadJson<Loader, DataFormat>(string path) where Loader : ILoader<DataFormat>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
