using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "SO/ItemInfoSO")]
public class ItemInfoSO : ScriptableObject
{
    public int ObjID;
    public string ObjName;
    public string ObjInfo;
    public int defaultPrice;
    public int expensive;
    public int tooExpensive;

    public GameObject Obj3D;
    public Sprite Obj2D;
}
