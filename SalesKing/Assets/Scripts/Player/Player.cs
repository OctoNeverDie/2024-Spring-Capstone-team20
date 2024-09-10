using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerCameraRot cam;
    PlayerMove move;

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
        FreezeAndUnFreezePlayer(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            NPC thisNPC = other.GetComponent<NPC>();
            if (thisNPC.currentTalkable == NPCDefine.Talkable.Able)
            {
                PlayerEnterConvo(other.gameObject);
            }
        }
    }

    public void FreezeAndUnFreezePlayer(bool isFreeze)
    {
        move.isMovementLocked = isFreeze;
        cam.isCameraLocked = isFreeze;
    }

    public void PlayerEnterConvo(GameObject npc)
    {
        FreezeAndUnFreezePlayer(true);
        Managers.Turn.ConvoStarted();
        transform.DOLookAt(npc.transform.position, 1f, AxisConstraint.None, null).
            SetUpdate(true);
        PlayerBody.SetActive(true);
    }


    public void PlayerExitConvo()
    {
        FreezeAndUnFreezePlayer(false);
    }
}
