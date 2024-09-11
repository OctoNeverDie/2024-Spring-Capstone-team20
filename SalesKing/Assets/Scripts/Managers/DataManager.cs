using System;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<DataFormat>
{ List<DataFormat> GetList(); }

public class DataManager
{
    public List<ItemInfo> itemList = new List<ItemInfo>();
    public void Init()
    {
        itemList = LoadJson<ItemData, ItemInfo>("ItemData").ItemInit();
        LoadJson<NpcData, NpcInfo>("NpcData").Init();
    }

    Loader LoadJson<Loader, DataFormat>(string path) where Loader : ILoader<DataFormat>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
