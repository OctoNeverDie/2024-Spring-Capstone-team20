using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraRot : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 8f; //회전속도
    private float mouseY = 0f; //위아래 회전값을 담을 변수

    Player _player;

    void Start()
    {
        _player = transform.parent.GetComponent<Player>();
    }

    void Update()
    {
        if (!_player.isPlayerLocked)
        {
            mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;

            mouseY = Mathf.Clamp(mouseY, -50f, 30f);

            this.transform.localEulerAngles = new Vector3(-mouseY, 0, 0);
        }
        
    }
}
