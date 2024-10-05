using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestItem : MonoBehaviour
{
    [SerializeField] private GameObject itemSelect;
    [SerializeField] private GameObject itemPrice;

    ItemInfo _itemInfo;
    int itemSuggestPrice;
    InputField inputField;

    private void Awake()
    {
        itemSelect.SetActive(true);
        itemPrice.SetActive(false);

        ForItemInfoMockData();
    }

    private void ForItemInfoMockData()
    {
        _itemInfo.ObjID = 1;
        _itemInfo.ObjName = "사탕";
        _itemInfo.ObjInfo = "딸기맛 사탕이다.";
        _itemInfo.defaultPrice = 5;
        _itemInfo.expensive = 10;
        _itemInfo.tooExpensive = 50;
    }

    public void OnItemClick(GameObject clickedButtonGOs)
    {
        //TODO:
        //item's information
        //_itemInfo = clickedButton.GetComponent<ItemInfo>();
        //item's id's 2dsprite

        //Image itemImage = itemPrice.GetComponent<Image>();
        //put in here
        itemSelect.SetActive(false);
        itemPrice.SetActive(true);
    }

    public void OnPriceClick(TMP_InputField inputFieldGO)
    {
        float inputPrice;

        if (float.TryParse(inputFieldGO.GetComponent<TMPro.TMP_InputField>().text, out inputPrice))
        {
            VariableList.InitItem(inputPrice, _itemInfo);
            itemPrice.SetActive(false);
        }
        else
        {
            Debug.LogError("It's not float type");
        }
    }
}
