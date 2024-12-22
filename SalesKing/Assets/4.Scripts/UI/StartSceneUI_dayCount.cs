using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartSceneUI_dayCount : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI day_text;

    void Start()
    {
        day_text.text = (DataController.Instance.playData.cur_day_ID+1)+"일차부터";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
