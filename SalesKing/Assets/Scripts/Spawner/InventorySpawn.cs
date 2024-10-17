using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySpawn : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;  // UI 아이템 프리팹
    [SerializeField] private Transform gridParent;   // UI 아이템의 부모 Transform
    [SerializeField] private GameObject choosePricePanel; // 가격 입력 패널
    [SerializeField] private TMP_InputField priceInputField; // 가격 입력 필드
    [SerializeField] private Button confirmPriceButton; // 가격 확인 버튼

    private ItemInfo selectedItem; // 선택된 아이템 정보

    private void Start()
    {
        // 가격 확인 버튼 클릭 이벤트 연결
        confirmPriceButton.onClick.AddListener(ConfirmPrice);
    }

    private Dictionary<int, GameObject> itemUIObjects = new Dictionary<int, GameObject>();

    private void OnEnable()
    {
        SpawnInventoryItems(); 
    }
    void SpawnInventoryItems()
    {
        // 인벤토리에서 아이템 정보 가져오기
        Dictionary<int, (ItemInfo itemInfo, int quantity)> inventory = Managers.Inven.GetInventory();
        // 현재 아이템 ID 목록
        HashSet<int> currentItemIds = new HashSet<int>(inventory.Keys);

        foreach (var entry in inventory)
        {
            int itemId = entry.Key;
            ItemInfo item = entry.Value.itemInfo;
            int quantity = entry.Value.quantity;

            if (itemUIObjects.ContainsKey(itemId))
            {
                // 이미 존재하는 UI 업데이트 및 활성화
                GameObject existingItem = itemUIObjects[itemId];
                existingItem.SetActive(true);
                existingItem.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>().text = "수량:" + quantity;
            }
            else
            {
                // 새로운 아이템 UI 생성
                GameObject newItem = Instantiate(itemPrefab, gridParent);

                newItem.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.ObjName;
                newItem.transform.Find("PriceText").GetComponent<TextMeshProUGUI>().text = item.defaultPrice.ToString() + "$";
                newItem.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>().text = "수량:" + quantity;

                Button itemButton = newItem.GetComponent<Button>();
                itemButton.onClick.AddListener(() => UseItem(item));

                // 딕셔너리에 추가
                itemUIObjects.Add(itemId, newItem);
            }
        }

        // 인벤토리에서 제거된 아이템 UI 비활성화
        foreach (var itemId in itemUIObjects.Keys)
        {
            if (!currentItemIds.Contains(itemId))
            {
                itemUIObjects[itemId].SetActive(false);
            }
        }
    }

    // 아이템 사용 메서드
    void UseItem(ItemInfo item)
    {
        selectedItem = item; // 선택된 아이템 정보 저장
        choosePricePanel.SetActive(true); // 가격 입력 패널 활성화
        priceInputField.text = ""; // 가격 입력 필드 초기화
    }

    // 가격 확인 메서드
    public void ConfirmPrice()
    {
       OnPriceClick(priceInputField, selectedItem); // PriceHandler의 메서드 호출
    }

    public void OnPriceClick(TMP_InputField inputFieldGO, ItemInfo selectedItem)
    {
        float inputPrice = 0;

        if (float.TryParse(inputFieldGO.text, out inputPrice))
        {
            Managers.Chat.EvalManager.InitItem(inputPrice, selectedItem);
        }
        else
        {
            Managers.Chat.EvalManager.InitItem(inputPrice, selectedItem);
        }
    }

}
