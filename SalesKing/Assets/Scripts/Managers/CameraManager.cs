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
        firstPersonCam = Managers.Player.MyPlayer.GetComponent<Player>().Camera;
        dialogueCam = GameObject.Find("VCam2(Dialogue)").GetComponent<CinemachineVirtualCamera>();
        SwitchToFirstPersonCam();
    }

    public void SwitchToFirstPersonCam()
    {
        // ó������ 1��Ī ī�޶� Ȱ��ȭ
        firstPersonCam.Priority = 10; // ���� �켱������ �����Ͽ� 1��Ī ī�޶� �⺻���� �ǵ���
        dialogueCam.Priority = 5; // ���� �켱������ ����
    }

    public void SwitchToDialogueCam(Transform player, Transform npc)
    {
        firstPersonCam.Priority = 5; 
        dialogueCam.Priority = 10;


        // ī�޶� ��ġ ���� (NPC�� �÷��̾� ���̿� ��ġ)
        Vector3 direction = (npc.position - player.position).normalized; // NPC�� �÷��̾� ���� ����
        Vector3 camPosition = player.position + direction * 2.0f + new Vector3(0, 1.5f, 0); // �÷��̾� ���ʿ� ī�޶� ��ġ
        dialogueCam.transform.position = camPosition;

        // NPC�� �ٶ󺸵��� ī�޶� ȸ�� ����
        dialogueCam.transform.LookAt(npc.position + new Vector3(0, 1.5f, 0)); // NPC�� ��ü(����)�� �ٶ󺸵��� ����

        // ī�޶��� �ణ�� ���� ���� ���� (NPC�� ������ �� ���̵���)
        dialogueCam.transform.RotateAround(npc.position, Vector3.up, 30.0f); // NPC �������� ī�޶� �ణ ȸ����Ŵ
    }

}
