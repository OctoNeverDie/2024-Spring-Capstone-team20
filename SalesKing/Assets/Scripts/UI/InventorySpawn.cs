using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySpawn : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;  // Prefab for the UI item display
    [SerializeField] private Transform gridParent;   // Parent transform for the UI items
    [SerializeField] private GameObject choosePricePanel;

    private void Start()
    {
        SpawnInventoryItems();
    }

    void SpawnInventoryItems()
    {
        // Get the inventory from the InventoryManager
        Dictionary<int, (ItemInfo itemInfo, int quantity)> inventory = Managers.Inven.GetInventory();

        foreach (var entry in inventory)
        {
            ItemInfo item = entry.Value.itemInfo;
            int quantity = entry.Value.quantity;

            // Instantiate the UI prefab for each item
            GameObject newItem = Instantiate(itemPrefab, gridParent);

            // Set item name, price, and quantity in the UI
            newItem.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.ObjName;
            newItem.transform.Find("PriceText").GetComponent<TextMeshProUGUI>().text = item.defaultPrice.ToString() + "$";
            newItem.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>().text = "수량:" + quantity;

            // If needed, add functionality to interact with the inventory items (e.g., use, sell, etc.)
            Button itemButton = newItem.GetComponent<Button>();
            itemButton.onClick.AddListener(() => UseItem(item));  // Example usage interaction
        }
    }

    // Example method for using an item
    void UseItem(ItemInfo item)
    {
        choosePricePanel.SetActive(true); 
    }
}
