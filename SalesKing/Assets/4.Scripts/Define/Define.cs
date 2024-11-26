using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

public class Define
{
    public enum Talkable
    { 
        Yes,
        No
    }

    public enum SceneMode
    {
        Start,
        CityMap, 
        SaveFile
    }

    public enum PersuasionLevel
    { 
        Like,
        Normal,
        Dislike
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
        MuhanInit,
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


    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemCategory
    {
        [EnumMember(Value = "Nothing")]
        None,
        [EnumMember(Value = "Money")]
        Money,
        [EnumMember(Value = "Music")]
        Music,
        [EnumMember(Value = "Books")]
        Books,
        [EnumMember(Value = "Dessert")]
        Dessert,
        [EnumMember(Value = "Game")]
        Game,
        [EnumMember(Value = "Clothes")]
        Clothes,
        [EnumMember(Value = "Cosmetics")]
        Cosmetics,
        [EnumMember(Value = "Exercise")]
        Exercise,
        [EnumMember(Value = "Food")]
        Food,
        [EnumMember(Value = "Hobby")]
        Hobby,
        [EnumMember(Value = "Weapons")]
        Weapons,
        [EnumMember(Value = "Furniture")]
        Furniture,
        [EnumMember(Value = "Random")]
        Random,
        MaxCnt
    }

    public enum GameMode
    {
        Story,
        Infinity
    }
}
