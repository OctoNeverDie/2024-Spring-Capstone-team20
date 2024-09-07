using System;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{ 
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Item> ItemDict { get; private set; } = new Dictionary<int, Item>();
    public void Init()
    {
        //���⼭ loader�� ItemData �ȴ�
        ItemDict = LoadJson<ItemData, int, Item>("Item").MakeDict();
        //LogDictionary(ItemDict);
    }

    private void LogDictionary(Dictionary<int, Item> dict)
    {
        foreach (KeyValuePair<int, Item> Item in dict)
        {
            Debug.Log("??");
            string logMessage = $"Key: {Item.Key}, Value: {{ ObjID: {Item.Value.ObjID}, ObjName: {Item.Value.ObjName}, ObjInfo: {Item.Value.ObjInfo}, " +
                                $"defaultPrice: {Item.Value.defaultPrice}, expensive: {Item.Value.expensive}, tooExpensive: {Item.Value.tooExpensive} }}";

            Debug.Log(logMessage);
        }
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/{path}");
        Debug.Log($"�ε�� json ���� : {textAsset}"  );
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
