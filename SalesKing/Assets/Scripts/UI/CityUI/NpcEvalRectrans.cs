using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class NpcEvalRectrans : MonoBehaviour
{
    [Header("Default")]
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Persuasion;
    
    [Header("Deal")]
    public TextMeshProUGUI Item;
    public TextMeshProUGUI Evaluation;

    [Header("ProfileImg")]
    public Image ProfileImg;
}
