using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRot : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 8f; //ȸ���ӵ�
    //private float mouseX = 0f; //�¿� ȸ������ ���� ����
    private float mouseY = 0f; //���Ʒ� ȸ������ ���� ����

    void Start()
    {

    }

    void Update()
    {
        //mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;

        mouseY = Mathf.Clamp(mouseY, -50f, 30f);

        //this.transform.localEulerAngles = new Vector3(-mouseY, mouseX, 0);
        this.transform.localEulerAngles = new Vector3(-mouseY, 0, 0);
    }
}
