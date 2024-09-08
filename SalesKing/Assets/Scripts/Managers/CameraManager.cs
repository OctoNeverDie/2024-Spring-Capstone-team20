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

    private bool isInDialogue = false; // 대화 중인지 체크

    void Start()
    {
        // 처음에는 1인칭 카메라만 활성화
        firstPersonCam.Priority = 10; // 높은 우선순위로 설정하여 1인칭 카메라가 기본값이 되도록
        dialogueCam.Priority = 5; // 낮은 우선순위로 설정
    }

    void Update()
    {
        
    }
}
