using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class NPCMove : MonoBehaviour
{
    NPC _npc;
    public float speed = 0.5f;

    public float rotationSpeed = 5.0f; // ȸ�� �ӵ�
    private NavMeshAgent agent; // NavMeshAgent ������Ʈ

    void Start()
    {
        _npc = GetComponent<NPC>();

        // NavMeshAgent ������Ʈ�� �����ɴϴ�.
        agent = GetComponent<NavMeshAgent>();

    }

    void Update()
    {
        if(_npc.destination != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _npc.destination.position, speed * Time.deltaTime);

            // ��ǥ ��ġ�� NavMeshAgent�� �������� �����մϴ�.
            agent.SetDestination(_npc.destination.position);

            // NPC�� �̵��ϴ� ������ ����մϴ�.
            Vector3 direction = _npc.destination.position - transform.position;
            direction.y = 0; // Y�� ȸ���� �����ϱ� ���� y ���� 0���� ����

            if (direction != Vector3.zero)
            {
                // NPC�� ���� ȸ���� ��ǥ �������� �ε巴�� �����մϴ�.
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
