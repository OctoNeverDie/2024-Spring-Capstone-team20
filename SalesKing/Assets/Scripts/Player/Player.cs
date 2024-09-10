using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerCameraRot cam;
    PlayerMove move;

    [SerializeField]
    GameObject ConvoPanel;
    [SerializeField]
    public CinemachineVirtualCamera Camera1;
    public CinemachineVirtualCamera Camera2;
    public GameObject PlayerBody;

    void Awake()
    {
        cam = transform.GetComponentInChildren<PlayerCameraRot>();
        move = GetComponent<PlayerMove>();
    }

    void Start()
    {
        cam.isCameraLocked = false;
        move.isMovementLocked = false;
    }


    public void FinishConvo()
    {
        cam.isCameraLocked = false;
        move.isMovementLocked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            NPC thisNPC = other.GetComponent<NPC>();
            if (thisNPC.currentTalkable == NPCDefine.Talkable.Able)
            {
                Debug.Log("collide with npc");
                ConvoPanel.SetActive(true);
                cam.isCameraLocked = true;
                move.isMovementLocked = true;
                Managers.Turn.StopAndRestartTime(true);
                thisNPC.UnbotheredByTime();
                Managers.Cam.SwitchToDialogueCam();
                transform.DOLookAt(other.transform.position, 1f, AxisConstraint.None, null).SetUpdate(true);
                PlayerBody.SetActive(true);
            }
        }
    }

    public void BackToWalking()
    {
        move.isMovementLocked = false;
        cam.isCameraLocked = false;
    }

}
