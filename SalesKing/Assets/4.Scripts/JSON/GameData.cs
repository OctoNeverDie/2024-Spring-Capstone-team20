using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

[System.Serializable]
public class GameData
{
    public float bgm_volume = 0;
    public float sfx_volume = 0;
    // save_files_IDs의 현재 인덱스
    public string cur_save_file_ID = null;
    
    // json file들의 리스트 저장
    public List<string> save_files_IDs = new List<string>();

    public Define.GameMode this_game_mode;

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
}

