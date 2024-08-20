using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerCameraRot cam;
    PlayerMove move;

    [SerializeField]
    GameObject ConvoPanel;

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
            }
        }
    }
}
