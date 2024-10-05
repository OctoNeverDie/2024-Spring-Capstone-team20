using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Define
{
    public enum SceneMode
    {
        Start,
        CityMap, 
        OfficeMap
    }

    public enum Emotion
    {
        Neutral,
        SlightlyPositive,
        Positive,
        VeryPositive,
        SlightlyNegative,
        Negative,
        VeryNegative
    }

    public enum Skybox
    {
        Morning, 
        Day, 
        Sunset, 
        Night
    }

    public enum SendChatType
    {
        None,
        NpcInit,
        ChatSale,
        ItemInit,
        ChatBargain,
        Leave,
        Fail,
        Success,
        MaxCnt
    }

}
