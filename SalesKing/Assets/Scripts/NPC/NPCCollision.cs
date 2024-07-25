using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class NPCCollision : MonoBehaviour
{
    bool isLookAt = false;
    Transform playerTransform;
    public float rotationSpeed = 5.0f; // 서서히 회전하기 위한 속도
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
            // 타겟을 향한 방향 벡터 계산
            Vector3 direction = playerTransform.position - transform.position;
            //direction.x = 0; // x축 회전을 고정합니다.
            direction.y = 0; // y축 회전을 고정합니다.
            //direction.z = 0; // z축 회전을 고정합니다.

            // 목표 회전 계산
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 서서히 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            if(transform.position == playerTransform.position)
            {
                isLookAt = false;
            }
        }
    }
}
