using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCam; // 1인칭 시점 가상 카메라
    public CinemachineVirtualCamera dialogueCam; // 대화 시점 가상 카메라

    public Transform player; // 플레이어 위치
    public Transform npc; // NPC 위치

    public float transitionSpeed = 3f; // 카메라 전환 속도

    //private bool isInDialogue = false; // 대화 중인지 체크

    void Start()
    {
        firstPersonCam = Managers.Player.MyPlayer.GetComponent<Player>().Camera;
        dialogueCam = GameObject.Find("VCam2(Dialogue)").GetComponent<CinemachineVirtualCamera>();
        SwitchToFirstPersonCam();
    }

    public void SwitchToFirstPersonCam()
    {
        // 처음에는 1인칭 카메라만 활성화
        firstPersonCam.Priority = 10; // 높은 우선순위로 설정하여 1인칭 카메라가 기본값이 되도록
        dialogueCam.Priority = 5; // 낮은 우선순위로 설정
    }

    public void SwitchToDialogueCam(Transform player, Transform npc)
    {
        firstPersonCam.Priority = 5; 
        dialogueCam.Priority = 10;


        // 카메라 위치 설정 (NPC와 플레이어 사이에 배치)
        Vector3 direction = (npc.position - player.position).normalized; // NPC와 플레이어 사이 방향
        Vector3 camPosition = player.position + direction * 2.0f + new Vector3(0, 1.5f, 0); // 플레이어 뒤쪽에 카메라 위치
        dialogueCam.transform.position = camPosition;

        // NPC를 바라보도록 카메라 회전 설정
        dialogueCam.transform.LookAt(npc.position + new Vector3(0, 1.5f, 0)); // NPC의 상체(정면)를 바라보도록 설정

        // 카메라의 약간의 측면 각도 조정 (NPC의 정면이 잘 보이도록)
        dialogueCam.transform.RotateAround(npc.position, Vector3.up, 30.0f); // NPC 기준으로 카메라를 약간 회전시킴
    }

}
