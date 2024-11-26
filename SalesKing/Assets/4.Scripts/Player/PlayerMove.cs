using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float gravity;
    private CharacterController controller;
    private Vector3 mov;

    private float mouseX;
    public bool isMovementLocked;

    private float speed;
    private float mouseSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mov = Vector3.zero;
        gravity = 10f;

        speed = PlayerManager.Instance.player.playerSpeed;
        mouseSpeed = PlayerManager.Instance.player.cameraSpeed;
    }

    void Update()
    {
        if (!isMovementLocked)
        {
            mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
            this.transform.localEulerAngles = new Vector3(0, mouseX, 0);

            if (controller.isGrounded)
            {
                mov = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                mov = controller.transform.TransformDirection(mov);
            }
            else
            {
                mov.y -= gravity * Time.deltaTime;
            }

            controller.Move(mov * Time.deltaTime * speed);
        }

    }
}
