using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCam; // 1��Ī ���� ���� ī�޶�
    public CinemachineVirtualCamera dialogueCam; // ��ȭ ���� ���� ī�޶�

    public Transform player; // �÷��̾� ��ġ
    public Transform npc; // NPC ��ġ

    public float transitionSpeed = 3f; // ī�޶� ��ȯ �ӵ�

    //private bool isInDialogue = false; // ��ȭ ������ üũ

    void Start()
    {
        firstPersonCam = Managers.Player.MyPlayer.GetComponent<Player>().Camera1;
        dialogueCam = Managers.Player.MyPlayer.GetComponent<Player>().Camera2;
        SwitchToFirstPersonCam();
    }

    public void SwitchToFirstPersonCam()
    {
        // ó������ 1��Ī ī�޶� Ȱ��ȭ
        firstPersonCam.Priority = 10; // ���� �켱������ �����Ͽ� 1��Ī ī�޶� �⺻���� �ǵ���
        dialogueCam.Priority = 5; // ���� �켱������ ����
    }

    public void SwitchToDialogueCam()
    {

        firstPersonCam.Priority = 5;
        dialogueCam.Priority = 10;
    }

}
