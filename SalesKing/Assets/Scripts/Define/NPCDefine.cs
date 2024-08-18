using System.Collections;
using System.Collections.Generic;
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

    public enum State { Stand, Walk }
    public enum Talkable { Able, Not }
    public enum Outfit { Normal, Abnormal}

    public enum AnimType
    {
        Idle,
        Positive, 
        Negative
    }
}
