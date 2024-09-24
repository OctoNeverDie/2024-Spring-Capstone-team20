using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private List<ItemInfo> makeItemList;
    [SerializeField] private GameObject itemPrefab;  // 인스펙터에서 아이템 프리팹을 지정
    [SerializeField] Transform gridParent;
    //public ItemSO itemSO;

    private void Start()
    {
        SpawnEntities();
    }

    void SpawnEntities()
    {
        makeItemList = Managers.Data.itemList;
      
        foreach (var item in makeItemList)
        {
            GameObject newItem = Instantiate(itemPrefab, gridParent);
            // 아이템 이미지, 이름, 가격을 동적으로 설정
            newItem.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.ObjName;
            newItem.transform.Find("Price/PriceText").GetComponent<TextMeshProUGUI>().text = item.defaultPrice.ToString() + "$";
            // 필요한 경우 다른 데이터 추가
            //GameObject spawnedItem = Instantiate(itemSO.Obj3D[item.ObjID - 1], Vector3.zero, Quaternion.identity);
            //spawnedItem.name = item.ObjName + item.ObjID;
        }
    }

}
