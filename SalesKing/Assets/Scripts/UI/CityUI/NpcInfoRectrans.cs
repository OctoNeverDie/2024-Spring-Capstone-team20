using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class NpcInfoRectrans : MonoBehaviour
{
    [Header("Default Info")]
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Persuasion;
    public TextMeshProUGUI Keyword;

    [Header("Profile Img")]
    public Image profileImg;

    [Header("SituationsUI")]
    public TextMeshProUGUI concern;
    public TextMeshProUGUI item;
}