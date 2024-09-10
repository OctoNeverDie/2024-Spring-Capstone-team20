using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ItemInfo
{
    public int ObjID;
    public string ObjName;
    public string ObjInfo;
    public int defaultPrice;
    public int expensive;
    public int tooExpensive;
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "SO/ItemSO")]
public class ItemSO : ScriptableObject
{
    public ItemInfo itemInfo;

    public GameObject Obj3D;
    public Sprite Obj2D;

    public void Initialize(ItemInfo data)
    {
        if (itemInfo == null) { itemInfo = new ItemInfo(); }

        itemInfo.ObjID = data.ObjID;
        itemInfo.ObjName = data.ObjName;
        itemInfo.ObjInfo = data.ObjInfo;
        itemInfo.defaultPrice = data.defaultPrice;
        itemInfo.expensive = data.expensive;
        itemInfo.tooExpensive = data.tooExpensive;
    }
}