using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class NpcEvalRectrans : MonoBehaviour
{
    [Header("Default")]
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Day;
    
    [Header("Deal")]
    public TextMeshProUGUI Evaluation;

    [Header("Img")]
    public Image ProfileImg;
    public Image SuccessImg;
}
