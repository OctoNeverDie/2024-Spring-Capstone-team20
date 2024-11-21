using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    PlayerCameraRot cam;
    PlayerMove move;
    public PlayerUI ui;

    // first person
    public CinemachineVirtualCamera Camera1;
    // dialogue
    public CinemachineVirtualCamera Camera2;
    public GameObject PlayerBody;

    public float rayDistance = 3f;  // 레이캐스트 거리
    public bool isRaycast = true;
    private GameObject previousTarget = null; // 이전에 Raycast가 감지한 오브젝트
    public GameObject RaycastCollider;

    void Awake()
    {
        cam = transform.GetComponentInChildren<PlayerCameraRot>();
        move = GetComponent<PlayerMove>();
        ui = transform.GetComponentInChildren<PlayerUI>();
    }

    void Update()
    {
        if (isRaycast)
        {
            // 카메라의 정면 방향으로 Ray 생성
            Ray ray = new Ray(Camera1.transform.position, Camera1.transform.forward);
            RaycastHit hit;

            // 레이캐스트 쏘기 (선택한 레이어만 충돌하거나 모든 물체와 충돌)
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                GameObject currentTarget = hit.collider.gameObject;

                // 이전에 감지한 오브젝트가 있다면 (별도 작업을 하지 않음)
                if (previousTarget != null)
                {
                    // 필요시 추가 기능: previousTarget과 현재 Target이 다를 때 처리
                    ui.CrosshairTriggersButton(false);
                }

                // 특정 태그를 가진 오브젝트가 히트되었을 때
                if (hit.collider.CompareTag("Office_MyPC"))
                {
                    ui.ShowCurInteractableIcon(Define.Interactables.Office_MyPC);
                    ui.CrosshairTriggersButton(true);
                }
                else if (hit.collider.CompareTag("Office_Door_Out"))
                {
                    ui.ShowCurInteractableIcon(Define.Interactables.Office_Door_Out);
                    ui.CrosshairTriggersButton(true);
                    RaycastCollider = hit.collider.gameObject;
                }
                else if (hit.collider.CompareTag("NPC"))
                {
                    NPC thisNPC = hit.collider.gameObject.GetComponent<NPC>();
                    if (thisNPC.currentTalkable == NPCDefine.Talkable.Able)
                    {
                        ui.ShowCurInteractableIcon(Define.Interactables.City_NPC);
                        ui.CrosshairTriggersButton(true);
                        RaycastCollider = hit.collider.gameObject;
                    }
                }
                else if (hit.collider.CompareTag("Office_Secretary"))
                {
                    ui.ShowCurInteractableIcon(Define.Interactables.Office_Secretary);
                    ui.CrosshairTriggersButton(true);
                    RaycastCollider = hit.collider.gameObject;
                }

                // 이전 타겟 업데이트
                previousTarget = currentTarget;
            }
            else
            {
                // 아무것도 감지되지 않았을 때
                if (previousTarget != null)
                {
                    // 이전 타겟 초기화
                    previousTarget = null;
                    ui.CrosshairTriggersButton(false);
                }
            }
        }
    }

    public void FreezeAndUnFreezePlayer(bool isFreeze)
    {
        move.isMovementLocked = isFreeze;
        cam.isCameraLocked = isFreeze;
        isRaycast = !isFreeze;
        if(isFreeze) ui.CrosshairTriggersButton(false);
        //ui.CrosshairTriggersButton(!isFreeze);
    }

    public void PlayerEnterConvo(GameObject npc)
    {
        FreezeAndUnFreezePlayer(true);
        //Managers.Convo.ConvoStarted();
        transform.DOLookAt(npc.transform.position, 1f, AxisConstraint.None, null).
            SetUpdate(true);
        PlayerBody.SetActive(true);
    }

    public void PlayerExitConvo()
    {
        FreezeAndUnFreezePlayer(false);
    }


}
