using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class City_TimeUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI time_text;

    void Start()
    {
        time_text.text = "DAY " + (DataController.Instance.playData.cur_day_ID+1);
    }
}
