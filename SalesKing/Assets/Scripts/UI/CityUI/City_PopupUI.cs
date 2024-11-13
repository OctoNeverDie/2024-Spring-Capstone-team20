using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City_PopupUI : MonoBehaviour
{
    public GameObject StartPanel;
    public Button StartPanelOKBtn;
    
    void Start()
    {
        StartPanelOKBtn.onClick.AddListener(OnClickStartPanelOKBtn);
    }

    void OnClickStartPanelOKBtn()
    {
        Managers.UI.c_popup.OnClickOKStartPanel();
    }
}
