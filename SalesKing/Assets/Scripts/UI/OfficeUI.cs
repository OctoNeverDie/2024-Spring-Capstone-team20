using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; // DOTween 사용

public class OfficeUI : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject WelcomePanel;
    public GameObject ShoppingPanel;

    Player myPlayer;

    void Start()
    {
        WelcomePanel.SetActive(true);
        myPlayer = Managers.Player.MyPlayer.GetComponent<Player>();
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

    public void OnClickMyPC()
    {
        DOVirtual.DelayedCall(Managers.Trans.ui.FadeTime, () => ShoppingPanel.SetActive(true));
        Managers.Office.myPlayer.FreezeAndUnFreezePlayer(true);
        //Managers.Office.SwitchToMyPCCam();
        Managers.Trans.ui.StartFadeInFadeOut(0.5f);
        //myPlayer.isRaycast = false;
    }

    public void OnClickExitMyPC()
    {
        DOVirtual.DelayedCall(Managers.Trans.ui.FadeTime, () => ShoppingPanel.SetActive(false));
        Managers.Office.myPlayer.FreezeAndUnFreezePlayer(false);
        //Managers.Office.SwitchToFirstPersonCam();
        Managers.Trans.ui.StartFadeInFadeOut(0.5f);
        //myPlayer.isRaycast = true;
    }

}
