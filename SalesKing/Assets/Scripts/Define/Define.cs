using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Define
{
    public enum SceneMode
    {
        Start,
        StageSelect,
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
        ItemInit,
        ChatBargain,
        Endpoint,
        MaxCnt
    }

    public enum EndType
    {
        None,
        reject,
        buy,
        leave,
        clear
    }

    public enum Interactables
    {
        Office_MyPC,
        Office_Door_Out,
        City_NPC,
        Office_Secretary
    }

    public enum UserInputMode
    {
        Keyboard, 
        Voice
    }

}
