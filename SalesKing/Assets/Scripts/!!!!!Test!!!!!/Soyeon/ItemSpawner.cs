using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private List<ItemInfo> makeItemList;
    public ItemSO itemSO;

    private void Start()
    {
        SpawnEntities();
    }

    void SpawnEntities()
    {
        makeItemList = Managers.Data.itemList;

        foreach (var item in makeItemList)
        { 
            GameObject spawnedItem = Instantiate(itemSO.Obj3D[item.ObjID - 1], Vector3.zero, Quaternion.identity);
            spawnedItem.name = item.ObjName + item.ObjID;
        }
    }

}
