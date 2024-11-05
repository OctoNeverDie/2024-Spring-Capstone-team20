using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using Newtonsoft.Json.Bson;

public class UIManager : MonoBehaviour
{
    public City_MainAction c_main;
    public City_PopupAction c_popup;
    public City_TabletAction c_tablet;
    public City_EndingAction c_ending;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        c_main = gameObject.AddComponent<City_MainAction>();
        c_popup = gameObject.AddComponent<City_PopupAction>();
        c_tablet = gameObject.AddComponent<City_TabletAction>();
        c_ending = gameObject.AddComponent<City_EndingAction>();
    }
}
