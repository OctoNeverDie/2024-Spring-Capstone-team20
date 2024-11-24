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
}
