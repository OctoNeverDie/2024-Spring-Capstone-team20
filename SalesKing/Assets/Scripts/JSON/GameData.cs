using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

[System.Serializable]
public class GameData
{
    public float mouseSpeed = 0;
    public int day = 0;    

    public List<MyItem> itemList = new List<MyItem>();
}


[System.Serializable]
public class MyItem
{
    public MyItem(int _index)
    {
        index = _index;
    }

    public int index;

}
