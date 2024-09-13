using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "SO/ItemSO")]
public class ItemSO : ScriptableObject
{
    public List<GameObject> Obj3D;
    public List<Sprite> Obj2D;
}