using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestItem : MonoBehaviour
{
    //[SerializeField] private GameObject itemSelect;
    //[SerializeField] private GameObject itemPrice;

    ItemInfo _itemInfo;

    public void OnItemClick(GameObject clickedButtonGOs)
    {
        //TODO:
        //item's information
        //_itemInfo = clickedButton.GetComponent<ItemInfo>();
        //item's id's 2dsprite

        //Image itemImage = itemPrice.GetComponent<Image>();
        //put in here
        //itemSelect.SetActive(false);
        //itemPrice.SetActive(true);
    }

    public void OnPriceClick(TMP_InputField inputFieldGO)
    {
        ForItemInfoMockData();

        float inputPrice;

        if (float.TryParse(inputFieldGO.GetComponent<TMPro.TMP_InputField>().text, out inputPrice))
        {
            VariableList.InitItem(inputPrice, _itemInfo);
            //itemPrice.SetActive(false);
        }
        else
        {
            Debug.LogError("It's not float type");
        }
    }
    private void ForItemInfoMockData()
    {
        _itemInfo = new ItemInfo();

        _itemInfo.ObjID = 1;
        _itemInfo.ObjName = "사탕";
        _itemInfo.ObjInfo = "딸기맛 사탕이다.";
        _itemInfo.defaultPrice = 5;
        _itemInfo.expensive = 10;
        _itemInfo.tooExpensive = 50;
    }
}
