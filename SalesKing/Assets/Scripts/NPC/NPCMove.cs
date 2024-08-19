using Newtonsoft.Json.Bson;
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

    public Transform curDestination;

    public float minStandTime = 10f;
    public float maxStandTime = 30f;

    private Animator animator;

    void Start()
    {
        npc = GetComponent<NPC>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = speed;
    }

    void Update()
    {
        if (curDestination != null)
        {
            agent.SetDestination(curDestination.position);

            if (Vector3.Distance(transform.position, curDestination.position) < 1f)
            {
                curDestination = null;
                OnDestinationReached();
            }
        }
    }

    public void ChooseNextDestination()
    {
        Transform thisTransform = Managers.NPC.Move.GetRandomSpawnPoint();
        if (thisTransform != null) curDestination = thisTransform;
        else curDestination = this.transform;
    }

    public void OnDestinationReached()
    {
        npc.AssignRandomState();
    }

    public IEnumerator StandForAwhile()
    {
        float standTime = Random.Range(minStandTime, maxStandTime);
        yield return new WaitForSeconds(standTime);
        npc.AssignRandomState();
    }

    public void PlayRandomNPCAnim(NPCDefine.AnimType type)
    {
        int randAnimIndex = Random.Range(0, Managers.NPC.Anim.NPCAnimDictionary[type].Count);
        animator.Play(Managers.NPC.Anim.NPCAnimDictionary[type][randAnimIndex].name);
    }
}
