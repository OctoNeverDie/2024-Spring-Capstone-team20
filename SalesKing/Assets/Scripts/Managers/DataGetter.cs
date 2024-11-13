using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.ComponentModel;

public interface ILoader<DataFormat>
{ List<DataFormat> GetList(); }

public class DataGetter : Singleton<DataGetter>, ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => true;

    private List<NpcInfo> npcList = new List<NpcInfo>();
    public List<NpcInfo> NpcList { get; }

    private List<ItemInfo> itemList = new List<ItemInfo>();
    public List<ItemInfo> ItemList { get; }
    private Dictionary<Define.ItemCategory, List<ItemInfo>> categorizedItems;
    public IReadOnlyDictionary<Define.ItemCategory, List<ItemInfo>> CategorizedItems{ get; private set; }

    public void Init()
    {
        npcList = LoadJson<NpcData, NpcInfo>("NpcData").GetList();
        itemList = LoadJson<ItemData, ItemInfo>("ItemData").GetList();
        ListToDict();
    }

    Loader LoadJson<Loader, DataFormat>(string path) where Loader : ILoader<DataFormat>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/JsonFile/{path}");
        Debug.Log($"{textAsset.text}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    private void ListToDict()
    {
        categorizedItems = itemList.GroupBy(item => item.Category)
                                    .ToDictionary(group => group.Key, group => group.ToList());
    }
}
