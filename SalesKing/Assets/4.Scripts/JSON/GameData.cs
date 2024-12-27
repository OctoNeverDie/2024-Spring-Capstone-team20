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
    //public string cur_save_file_ID = null;
    
    // json file들의 리스트 저장
    //public List<string> save_files_IDs = new List<string>();

    public Define.GameMode this_game_mode;
    public int cur_day_ID;
    public int cleared_npc_count;
}

