using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficePlayer : MonoBehaviour
{
    PlayerCameraRot cam;
    PlayerMove move;

    void Awake()
    {
        cam = transform.GetComponentInChildren<PlayerCameraRot>();
        move = GetComponent<PlayerMove>();
    }

    void Start()
    {
        cam.isCameraLocked = true;
        move.isMovementLocked = true;
    }

    public void FinishConvo()
    {
        cam.isCameraLocked = false;
        move.isMovementLocked = false;
    }
}
