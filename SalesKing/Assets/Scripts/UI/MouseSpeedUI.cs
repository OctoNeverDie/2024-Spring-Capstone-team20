using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseSpeedUI : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI sliderText;

    /*void Start()
    {
        slider.value = DataController.Instance.gameData.mouseSpeed;
        sliderText.text = DataController.Instance.gameData.mouseSpeed.ToString();
    }

    public void UpdateMouseSpeed()
    {
        DataController.Instance.gameData.mouseSpeed = slider.value;
        DataController.Instance.ToGameJson();
        sliderText.text = slider.value.ToString();
    }
    */
}
