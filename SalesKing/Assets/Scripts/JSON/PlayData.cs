using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

[System.Serializable]
public class PlayData
{
    // 지금 현재 플레이하고 있는 날짜
    public int cur_day_ID = 0;
    // 지금까지 플레이했던 날들의 데이터 리스트 
    public List<DayData> item_list = new List<DayData>();
}

[System.Serializable]
public class DayData
{
    public DayData(int _id)
    {
        day_ID = _id;
    }

    public int day_ID;

}