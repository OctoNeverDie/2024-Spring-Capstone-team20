using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConcernManager : MonoBehaviour
{
    private List<ConcernInfo> concerns;
    private List<ItemInfo> items;

    private HashSet<int> usedConcernIDs = new HashSet<int>();

    public (int firstValue, int secondValue) GetRandomConcernAndItem()
    {
        concerns = Managers.Data.concernList;
        items = Managers.Data.itemList;

        // Check if concerns list is empty
        if (concerns.Count == 0)
        {
            Debug.Log("No concerns available.");
            return (0, 0);
        }

        // Randomly select a concern
        var randomConcern = concerns[UnityEngine.Random.Range(0, concerns.Count)];

        // Get the corresponding category
        string category = randomConcern.Category;

        // Filter items by the selected concern's category
        var itemsInCategory = items.Where(item => item.Category == category).ToList();

        if (itemsInCategory.Count == 0)
        {
            Debug.Log("No items available in the selected category.");
            return (0, 0);
        }

        // Randomly select an item
        var randomItem = itemsInCategory[UnityEngine.Random.Range(0, itemsInCategory.Count)];

        // Return the formatted result
        return (randomConcern.ConcernID, randomItem.ObjID);
    }

}

