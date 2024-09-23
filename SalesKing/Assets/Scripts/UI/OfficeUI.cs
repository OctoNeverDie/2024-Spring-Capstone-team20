using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeUI : MonoBehaviour
{
    public GameObject WelcomePanel;
    void Start()
    {
        WelcomePanel.SetActive(true);
    }

    void Update()
    {
        
    }

    public void OnClickWelcomeOK()
    {
        Managers.Office.myPlayer.FreezeAndUnFreezePlayer(false);
        WelcomePanel.SetActive(false);
        Managers.Office.SwitchToFirstPersonCam();
    }

    public void OnClickWelcomeWhat()
    {
        Managers.Office.myPlayer.FreezeAndUnFreezePlayer(false);
        WelcomePanel.SetActive(false);
        Managers.Office.SwitchToFirstPersonCam();
    }
}
