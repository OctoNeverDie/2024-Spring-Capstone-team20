using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OfficeManager : MonoBehaviour
{
    public Player myPlayer;
    public GameObject officeUI;

    public CinemachineVirtualCamera firstPersonCam; // 1인칭 시점 가상 카메라
    public CinemachineVirtualCamera WelcomeCam;

    void Start()
    {
        myPlayer = Managers.Player.MyPlayer.GetComponent<Player>();
        myPlayer.FreezeAndUnFreezePlayer(true);
        officeUI = GameObject.Find("Canvas");

        firstPersonCam = myPlayer.Camera1;
        Destroy(myPlayer.Camera2.gameObject);
        WelcomeCam = GameObject.Find("WelcomeCamera").GetComponent<CinemachineVirtualCamera>();
        SwitchToWelcomeCam();
    }

    void Update()
    {
        
    }

    public void SwitchToFirstPersonCam()
    {
        firstPersonCam.Priority = 10;
        WelcomeCam.Priority = 5;
    }

    public void SwitchToWelcomeCam()
    {
        firstPersonCam.Priority = 5;
        WelcomeCam.Priority = 10;
    }
}