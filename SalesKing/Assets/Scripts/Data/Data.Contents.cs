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

#region NPC
public class NpcData : ScriptableObjectManager<NpcInfo>, ILoader<NpcInfo>
{
    public List<NpcInfo> npcs = new List<NpcInfo>();
    public List<NpcInfo> GetItems()
    => npcs;

    protected override void MakeSOs()
    {
        base.MakeDirectory("npcs");

        foreach (var npc in npcs)
        {
            MakeSOInstance(npc);
        }
    }
    protected override void MakeSOInstance(NpcInfo npc)
    {
        NpcSO npcSO = ScriptableObject.CreateInstance<NpcSO>();
        npcSO.Initialize(npc);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.CreateAsset(npcSO, $"{base.basePath}Npcs/{npcSO.npcInfo.NpcName}.asset");
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }
}
#endregion