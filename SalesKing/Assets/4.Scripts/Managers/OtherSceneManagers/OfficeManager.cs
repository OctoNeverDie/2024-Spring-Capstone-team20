using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro.Examples;

public class OfficeManager : MonoBehaviour
{
    /*
    public Player myPlayer;
    public OfficeUI officeUI;

    public CinemachineVirtualCamera firstPersonCam; // 1인칭 시점 가상 카메라
    public CinemachineVirtualCamera WelcomeCam;
    public CinemachineVirtualCamera MyPCCamera;

    void Start()
    {
        myPlayer = Managers.Player.MyPlayer.GetComponent<Player>();
        myPlayer.FreezeAndUnFreezePlayer(true);
        officeUI = GameObject.FindWithTag("Office_UI").GetComponent<OfficeUI>();
            firstPersonCam = myPlayer.Camera1;
        Destroy(myPlayer.Camera2.gameObject);
        WelcomeCam = GameObject.Find("WelcomeCamera").GetComponent<CinemachineVirtualCamera>();
        MyPCCamera = GameObject.Find("MyPCCamera").GetComponent<CinemachineVirtualCamera>();
        SwitchToWelcomeCam();
    }


    public void SwitchToFirstPersonCam()
    {
        firstPersonCam.Priority = 10;
        WelcomeCam.Priority = 5;
        MyPCCamera.Priority = 5;
    }

    public void SwitchToWelcomeCam()
    {
        firstPersonCam.Priority = 5;
        WelcomeCam.Priority = 10;
        MyPCCamera.Priority = 5;
    }

    public void SwitchToMyPCCam()
    {
        firstPersonCam.Priority = 5;
        WelcomeCam.Priority = 5;
        MyPCCamera.Priority = 10;
    }
    */
}
