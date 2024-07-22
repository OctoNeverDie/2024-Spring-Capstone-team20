using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class NPCMove : MonoBehaviour
{
    NPC _npc;
    public float speed = 0.5f;

    public float rotationSpeed = 5.0f; // 회전 속도
    private NavMeshAgent agent; // NavMeshAgent 컴포넌트

    void Start()
    {
        _npc = GetComponent<NPC>();

        // NavMeshAgent 컴포넌트를 가져옵니다.
        agent = GetComponent<NavMeshAgent>();

    }

    void Update()
    {
        if(_npc.destination != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _npc.destination.position, speed * Time.deltaTime);

            // 목표 위치를 NavMeshAgent의 목적지로 설정합니다.
            agent.SetDestination(_npc.destination.position);

            // NPC가 이동하는 방향을 계산합니다.
            Vector3 direction = _npc.destination.position - transform.position;
            direction.y = 0; // Y축 회전을 방지하기 위해 y 값을 0으로 설정

            if (direction != Vector3.zero)
            {
                // NPC의 현재 회전을 목표 방향으로 부드럽게 보정합니다.
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
