using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConcernManager : MonoBehaviour
{
    private List<ConcernInfo> concerns;
    private List<ItemInfo> items;

    private HashSet<int> usedConcernIDs = new HashSet<int>();

    public string GetRandomConcernAndItem()
    {
        concerns = Managers.Data.concernList;
        items = Managers.Data.itemList;

        // Check if concerns list is empty
        if (concerns.Count == 0)
        {
            return "No concerns available.";
        }

        // Randomly select a concern
        var randomConcern = concerns[Random.Range(0, concerns.Count)];

        // Get the corresponding category
        string category = randomConcern.Category;

        // Filter items by the selected concern's category
        var itemsInCategory = items.Where(item => item.Category == category).ToList();

        if (itemsInCategory.Count == 0)
        {
            return "No items available in the selected category.";
        }

        // Randomly select an item
        var randomItem = itemsInCategory[Random.Range(0, itemsInCategory.Count)];

        // Return the formatted result
        return $"고민: {randomConcern.Concern} \n구매자가 사고자 하는 물건: {randomItem.ObjName}";
    }

}

