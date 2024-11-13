using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Define
{
    public enum SceneMode
    {
        Start,
        //StageSelect,
        CityMap, 
        OfficeMap, 
        SaveFile
    }

    public enum Emotion
    {
        worst,
        bad,
        normal,
        good,
        best
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
        ChatInit,
        Chatting,
        Endpoint,
        MaxCnt
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

    public enum ItemCategory
    { 
        Default,
        Food,
        Clothes,
        MaxCnt
    }
}
