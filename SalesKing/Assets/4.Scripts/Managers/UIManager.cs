using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>, ISingletonSettings
{
    public bool ShouldNotDestroyOnLoad => true;

    public City_MainUI Main;


    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    void Init()
    {

    }
}
