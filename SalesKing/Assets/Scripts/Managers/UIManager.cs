using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using Newtonsoft.Json.Bson;

public class UIManager : Singleton<NPCManager>
{
    public City_MainAction c_main;
    public City_PopupAction c_popup;
    public City_ChattingAction c_chatting;
    public City_SummaryAction c_summary;
    public City_TabletAction c_tablet;
    public City_EndingAction c_ending;

    public City_MainUI ui_main;
    public City_PopupUI ui_popup;
    public City_ChattingUI ui_chatting;
    public City_SummaryUI ui_summary;
    public City_TabletUI ui_tablet;
    public City_EndingUI ui_ending;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    void Start()
    {
        ui_main = FindObjectOfType<City_MainUI>();
        ui_popup = FindObjectOfType<City_PopupUI>();
        ui_chatting = FindObjectOfType<City_ChattingUI>();
        ui_summary = FindObjectOfType<City_SummaryUI>();
        ui_tablet = FindObjectOfType<City_TabletUI>();
        ui_ending = FindObjectOfType<City_EndingUI>();
    }

    void Init()
    {
        c_main = gameObject.AddComponent<City_MainAction>();
        c_popup = gameObject.AddComponent<City_PopupAction>();
        c_chatting = gameObject.AddComponent<City_ChattingAction>();
        c_summary = gameObject.AddComponent<City_SummaryAction>();
        c_tablet = gameObject.AddComponent<City_TabletAction>();
        c_ending = gameObject.AddComponent<City_EndingAction>();
    }
}
