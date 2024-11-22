using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraRot : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 3f; //회전속도
    private float mouseY = 0f; //위아래 회전값을 담을 변수

    public bool isCameraLocked;

    void Awake()
    {
        mouseSpeed = 3f;
    }

    void Update()
    {
        if (!isCameraLocked)
        {
            mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;
            mouseY = Mathf.Clamp(mouseY, -50f, 30f);
            this.transform.localEulerAngles = new Vector3(-mouseY, 0, 0);
        }
        
    }
}
