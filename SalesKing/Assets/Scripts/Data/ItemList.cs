using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    List<ItemInfo> itemList;

    public void Start()
    {
        Test();
    }
    public void Test()
    {
        itemList = Managers.Data.itemList;
        foreach (var item in itemList)
        {
            Debug.Log($"{item.ObjID}, {item.ObjName}, {item.ObjInfo}, {item.Category}");
        }
    }
}
