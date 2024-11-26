using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

public interface ILoader<DataFormat>
{ List<DataFormat> GetList(); }

public class DataGetter : Singleton<DataGetter>, ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => true;

    private List<NpcInfo> npcList = new List<NpcInfo>();
    public List<NpcInfo> NpcList { get => npcList; }

    private List<ItemInfo> itemList = new List<ItemInfo>();
    public List<ItemInfo> ItemList { get => itemList; }
    private Dictionary<Define.ItemCategory, List<ItemInfo>> categorizedItems;
    public IReadOnlyDictionary<Define.ItemCategory, List<ItemInfo>> CategorizedItems{ get => categorizedItems; }

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public void Init()
    {
        npcList = LoadJson<NpcData, NpcInfo>("NpcData").GetList();
        //foreach (var npc in npcList)
        //{
        //    // 확장 메서드를 사용하여 출력
        //    string logMessage = $"NpcID: {npc.NpcID}, " +
        //                        $"NpcAge: {npc.NpcAge}, " +
        //                        $"Mbtis: [{string.Join(", ", npc.Mbtis)}], " +
        //                        $"ItemCategory: {npc.ItemCategory}, " +
        //                        $"NpcName: {npc.NpcName}, " +
        //                        $"NpcSex: {npc.NpcSex}, " +
        //                        $"KeyWord: {npc.KeyWord}, " +
        //                        $"Concern: {npc.Concern}, " +
        //                        $"WantItem: {npc.WantItem}, " +
        //                        $"Personality: {npc.Personality}, " +
        //                        $"DialogueStyle: {npc.DialogueStyle}, " +
        //                        $"Example: {npc.Example}";

        //    Debug.Log(logMessage);
        //}
        itemList = LoadJson<ItemData, ItemInfo>("ItemData").GetList();
        ListToDict();
    }

    Loader LoadJson<Loader, DataFormat>(string path) where Loader : ILoader<DataFormat>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/JsonFile/{path}");
        Debug.Log($"{textAsset.text}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }

    private void ListToDict()
    {
        categorizedItems = itemList.GroupBy(item => item.Category)
                                    .ToDictionary(group => group.Key, group => group.ToList());
    }
}
