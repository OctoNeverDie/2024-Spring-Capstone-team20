using Newtonsoft.Json;
using UnityEngine;

public class NPCDefine : MonoBehaviour
{
    public enum MeshType
    {
        Backpack,
        Body,
        Eyebrow,
        FullBody,
        Glasses,
        Glove,
        Hair,
        Hat,
        Mustache,
        Outerwear,
        Pants,
        Shoe
    }

    public enum AnimType
    {
        Idle,
        SlightlyPositive, 
        Positive,
        VeryPositive,
        SlightlyNegative,
        Negative,
        VeryNegative, 
        Attack,
        Moving,
        Standing
    }


    // 외양 enum
    [JsonConverter(typeof(SafeEnumConverter<SexType>), SexType.None)]
    public enum SexType { None, Male, Female }

    [JsonConverter(typeof(SafeEnumConverter<AgeType>), AgeType.None)]
    public enum AgeType { None, Young, Old }

    [JsonConverter(typeof(SafeEnumConverter<BackpackType>), BackpackType.None)]
    public enum BackpackType { None, Common, Wings, Tube, Youtuber, Jetpack, Hiking, Sword, Gun, Turtle, Drums, Skateboard }

    [JsonConverter(typeof(SafeEnumConverter<BodyType>), BodyType.None)]
    public enum BodyType { None, Common, Alien }

    [JsonConverter(typeof(SafeEnumConverter<EyebrowType>), EyebrowType.None)]
    public enum EyebrowType { None, Aggressive, Common, Timid, Old, Thick }
    
    [JsonConverter(typeof(SafeEnumConverter<FullBodyType>), FullBodyType.None)]
    public enum FullBodyType { None, OuterSpace, Soldier, Halloween, Food, Party }

    [JsonConverter(typeof(SafeEnumConverter<GlassesType>), GlassesType.None)]
    public enum GlassesType { None, Common, Sunglasses, Special }

    [JsonConverter(typeof(SafeEnumConverter<GloveType>), GloveType.None)]
    public enum GloveType { None, Common, Boxing, FingerGloves, LadyGloves, Mitten }

    [JsonConverter(typeof(SafeEnumConverter<HairType>), HairType.None)]
    public enum HairType { None, MaleCommon, FemaleCommon, Oriental, Workout, Punk, HotGirl, CuteGirl }

    [JsonConverter(typeof(SafeEnumConverter<HatType>), HatType.None)]
    public enum HatType
    {
        None, Common, Cap, Headphones, Beret, Theif, Police, Farmer, Chef, Maid, Firefighter, Goggles, Graduate,
        ConstructionWorker, Pilot, SerialKiller, Clown, CuteCostume, Pancho, PowderedWig, Elf, Kid, Santa, Gentleman,
        Helmet, Cleopatra, Snorkling, Lady, Crown, HeroMask, WinterHat, Yoda, GasMask, JackSparrow, PaperBag, Alien, PlagueMask
    }

    [JsonConverter(typeof(SafeEnumConverter<MustacheType>), MustacheType.None)]
    public enum MustacheType { None, Common, Thin, Curly, Guru, White }

    [JsonConverter(typeof(SafeEnumConverter<OuterwearType>), OuterwearType.None)]
    public enum OuterwearType { None, Hoodie, TankTop, Dress, Shirt, Jacket, Suit, TShirt }

    [JsonConverter(typeof(SafeEnumConverter<PantsType>), PantsType.None)]
    public enum PantsType { None, Shorts, Pants, Skirt }

    [JsonConverter(typeof(SafeEnumConverter<ShoeType>), ShoeType.None)]
    public enum ShoeType { None, Sneakers, Boots, Skate, Slippers }

    // 까지
}
