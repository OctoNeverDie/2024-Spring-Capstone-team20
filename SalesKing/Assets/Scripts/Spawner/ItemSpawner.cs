using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            //newItem.transform.Find("Price/PriceText").GetComponent<TextMeshProUGUI>().text = item.defaultPrice.ToString() + "$";
            // 필요한 경우 다른 데이터 추가
            //GameObject spawnedItem = Instantiate(itemSO.Obj3D[item.ObjID - 1], Vector3.zero, Quaternion.identity);
            //spawnedItem.name = item.ObjName + item.ObjID;

            //아래는 임시. 지금 상태로는 누르면 바로 구매하도록 되어 있음
            // Button 컴포넌트를 가져와 클릭 이벤트 추가
             // Button 컴포넌트를 찾기
            Button itemButton = newItem.GetComponent<Button>();
            itemButton.onClick.AddListener(() => PurchaseItem(item));  // 아이템 클릭 시 호출될 메서드
        }
    }

    // 아이템 구매 시 실행되는 메서드
    void PurchaseItem(ItemInfo item)
    {
        /*
        // 현금에서 아이템 가격을 제거
        if (Managers.Cash.RemoveCash(item.defaultPrice))
        {
            // 인벤토리에 해당 아이템 추가
            Managers.Inven.AddToInventory(item);
            Debug.Log(item.ObjName + " purchased successfully!");
        }
        else
        {
            Debug.Log("Not enough cash to purchase " + item.ObjName);
        }
        */
    }
}
