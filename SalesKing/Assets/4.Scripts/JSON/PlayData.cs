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
    private int _cur_day_ID; // 실제 값을 저장하는 필드

    public int cur_day_ID
    {
        get => _cur_day_ID; // 값을 반환할 때는 백킹 필드 사용
        set
        {
            Debug.Log($"Day changed from {_cur_day_ID} to {value}");
            _cur_day_ID = value; // 값을 백킹 필드에 저장
        }
    }

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