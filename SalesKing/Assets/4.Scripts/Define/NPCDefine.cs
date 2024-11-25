using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.ParticleSystem;

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

    public enum EyebrowType { Aggressive, Normal, Timid }
    public enum SexType { Male, Female, Unknown }
    public enum AgeType { Young, Common, Old }
    public enum BackpackType { None, Common, Wings, Tube, Youtuber, Jetpack, Hiking, Sword, Gun }
    public enum FullbodyType { None, OuterSpace, Soldier, Halloween, Food, Party }
    public enum GlassesType { None, CommonGlasses, Sunglasses, Special }
    public enum GloveType { None, Common, Boxing, Ski, LadyGloves, WorkGloves }
    public enum HairType { None, MaleCommon, FemaleCommon, Oriental, Workout }
    public enum HatType
    {
        None, Common, Cap, Headphones, Beret, Theif, Police, Farmer, Chef, Maid, Firefighter, Goggles, Graduate,
        ConstructionWorker, Pilot, SerialKiller, Clown, CuteCostume, Pancho, PowderedWig, Elf, Kid, Santa, Gentleman,
        Helmet, Cleopatra, Snorkling, Lady, Crown, HeroMask, WinterHat, Yoda, GasMask, JackSparrow, PaperBag, Alien, PlagueMask
    }
    public enum MustacheType { None, Common, Thin, Beard, White }
    public enum OuterwearType { Hoodie, TankTop, Dress, Shirt, Jacket, Suit, TShirt }
    public enum PantsType { None, Shorts, Pants, Skirt }
    public enum ShoeType { None, Sneakers, Boots, Skate, Slippers }

}
