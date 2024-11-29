using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class NpcInfoSummaryRectrans : MonoBehaviour
{
    [Header("Default")]
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Age;
    public TextMeshProUGUI Persuasion;
    public TextMeshProUGUI Keyword;
    public TextMeshProUGUI Item;

    [Header("Img")]
    public Image ProfileImg;
    public Image SuccessImg;
}
