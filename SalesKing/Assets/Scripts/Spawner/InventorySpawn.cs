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
        SpawnInventoryItems();

        // 가격 확인 버튼 클릭 이벤트 연결
        confirmPriceButton.onClick.AddListener(ConfirmPrice);
    }

    void SpawnInventoryItems()
    {
        // 인벤토리에서 아이템 정보 가져오기
        Dictionary<int, (ItemInfo itemInfo, int quantity)> inventory = Managers.Inven.GetInventory();

        foreach (var entry in inventory)
        {
            ItemInfo item = entry.Value.itemInfo;
            int quantity = entry.Value.quantity;

            // UI 프리팹 인스턴스화
            GameObject newItem = Instantiate(itemPrefab, gridParent);

            // 아이템 이름, 가격 및 수량 설정
            newItem.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.ObjName;
            newItem.transform.Find("PriceText").GetComponent<TextMeshProUGUI>().text = item.defaultPrice.ToString() + "$";
            newItem.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>().text = "수량:" + quantity;

            // 아이템 사용 버튼 설정
            Button itemButton = newItem.GetComponent<Button>();
            itemButton.onClick.AddListener(() => UseItem(item));  // 아이템 사용
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
        float inputPrice;

        if (float.TryParse(inputFieldGO.text, out inputPrice))
        {
            Managers.Chat.EvalManager.InitItem(inputPrice, selectedItem);
        }
        else
        {
            Debug.LogError("It's not float type");
        }
    }

}
