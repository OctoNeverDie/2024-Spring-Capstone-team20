using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;
    private Dictionary<int, (ItemInfo itemInfo, int quantity)> inventory = new Dictionary<int, (ItemInfo, int)>();


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this); // 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스 방지
        }
    }

    // Add item to inventory, with quantity management
    public void AddToInventory(ItemInfo item)
    {
        if (inventory.ContainsKey(item.ObjID))
        {
            // Increase the quantity if the item already exists
            inventory[item.ObjID] = (item, inventory[item.ObjID].quantity + 1);
        }
        else
        {
            // Add new item with quantity 1
            inventory.Add(item.ObjID, (item, 1));
        }
        Debug.Log(item.ObjName + " added to inventory. Quantity: " + inventory[item.ObjID].quantity);
    }

    // Remove item or decrease quantity
    public void RemoveFromInventory(int itemId)
    {
        if (inventory.ContainsKey(itemId))
        {
            if (inventory[itemId].quantity > 1)
            {
                // Decrease the quantity
                inventory[itemId] = (inventory[itemId].itemInfo, inventory[itemId].quantity - 1);
            }
            else
            {
                // Remove the item completely if quantity reaches 0
                inventory.Remove(itemId);
            }
            Debug.Log("Item removed or quantity decreased. Remaining quantity: " + inventory[itemId].quantity);
        }
        else
        {
            Debug.Log("Item not found in inventory.");
        }
    }

    public bool IsItemInInventory(int itemId)
    {
        return inventory.ContainsKey(itemId);
    }

    public int GetItemQuantity(int itemId)
    {
        if (inventory.ContainsKey(itemId))
        {
            return inventory[itemId].quantity;
        }
        return 0;  // Return 0 if item not found
    }
    
    //--------------이 밑은 playerprefs. 지금은 귀찮으니까 사용 안함

    // PlayerPrefs를 사용하여 인벤토리 데이터를 저장
    public void SaveInventory()
    {
        foreach (var entry in inventory)
        {
            PlayerPrefs.SetInt("Inventory_" + entry.Value.itemInfo.ObjID, entry.Value.quantity);
        }
        PlayerPrefs.Save(); // PlayerPrefs의 변경 내용을 저장
        Debug.Log("Inventory saved to PlayerPrefs!");
    }

    // PlayerPrefs를 사용하여 인벤토리 데이터를 불러오기
    public void LoadInventory(List<ItemInfo> allItems)
    {
        inventory.Clear();  // 기존 인벤토리 초기화

        foreach (ItemInfo item in allItems)
        {
            // PlayerPrefs에 저장된 아이템의 수량을 불러오기
            if (PlayerPrefs.HasKey("Inventory_" + item.ObjID))
            {
                int quantity = PlayerPrefs.GetInt("Inventory_" + item.ObjID);
                inventory[item.ObjID] = (item, quantity);
                Debug.Log(item.ObjName + " loaded with quantity: " + quantity);
            }
        }
    }
}