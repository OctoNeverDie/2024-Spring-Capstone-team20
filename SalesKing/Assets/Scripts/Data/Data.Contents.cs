using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

#region Item
public class ItemData : ScriptableObjectManager<ItemInfo>, ILoader<ItemInfo>
{
    public List<ItemInfo> items = new List<ItemInfo>();
    public List<ItemInfo> GetItems()
    => items;

    public void Init(){ MakeSOs();}
    protected override void MakeSOs()
    {
        base.MakeDirectory("items");

        foreach (var item in items)
        {
            MakeSOInstance(item);
        }
    }
    protected override void MakeSOInstance(ItemInfo item)
    {
        ItemSO itemSO = ScriptableObject.CreateInstance<ItemSO>();
        itemSO.Initialize(item);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.CreateAsset(itemSO, $"{base.basePath}Items/{itemSO.itemInfo.ObjName}.asset");
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }
}
#endregion