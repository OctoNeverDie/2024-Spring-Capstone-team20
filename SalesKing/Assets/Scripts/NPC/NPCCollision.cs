using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class NPCCollision : MonoBehaviour
{
    bool isLookAt = false;
    Transform playerTransform;
    public float rotationSpeed = 5.0f; // ������ ȸ���ϱ� ���� �ӵ�
    private NPC npc;

    void Start()
    {
        npc = GetComponent<NPC>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && npc.currentTalkable == NPC.Talkable.Able)
        {
            //transform.LookAt(other.transform);
            playerTransform = other.transform;
            isLookAt = true;
        }
    }

    void FixedUpdate()
    {
        if (isLookAt) 
        {
            //isLookAt = false;
            // Ÿ���� ���� ���� ���� ���
            Vector3 direction = playerTransform.position - transform.position;
            //direction.x = 0; // x�� ȸ���� �����մϴ�.
            direction.y = 0; // y�� ȸ���� �����մϴ�.
            //direction.z = 0; // z�� ȸ���� �����մϴ�.

            // ��ǥ ȸ�� ���
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // ������ ȸ��
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            if(transform.position == playerTransform.position)
            {
                isLookAt = false;
            }
        }
    }
}
