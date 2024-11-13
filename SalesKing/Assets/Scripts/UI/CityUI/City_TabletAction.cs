using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City_TabletAction : MonoBehaviour
{
    public bool isTablet = false;
    void Start()
    {
        isTablet = false;
        //Managers.UI.ui_tablet.InitTablet();
    }
    void Update()
    {
        if (Input.GetButtonDown("Tab"))
        {
            if (isTablet)
            {
                //Managers.UI.ui_tablet.OnClickHideTablet();
                isTablet = false;
            }
            else
            {
                //Managers.UI.ui_tablet.OnClickShowTablet();
                isTablet = true;
            }
            
        }
    }
}
