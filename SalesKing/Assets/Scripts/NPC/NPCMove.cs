using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMove : MonoBehaviour
{
    public float speed = 0.5f;
    public float rotationSpeed = 5.0f;

    private NavMeshAgent agent;
    private NPC npc;

    void Start()
    {
        npc = GetComponent<NPC>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    void Update()
    {
        if (npc.destination != null)
        {
            agent.SetDestination(npc.destination.position);

            if (Vector3.Distance(transform.position, npc.destination.position) < 1f)
            {
                npc.destination = null;
                npc.OnDestinationReached();
            }
        }
    }

    public void SetDestination(Transform destination)
    {
        npc.destination = destination;
        agent.SetDestination(destination.position);
    }
}
