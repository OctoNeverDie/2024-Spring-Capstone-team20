using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum State { Stand, Walk }
    public enum Talkable { Able, Not }
    public State currentState;
    public Talkable currentTalkable;
    public Transform destination;
    public float minStandTime = 10f;
    public float maxStandTime = 30f;

    GameObject myCanvas;

    private NPCMove npcMove;

    void Start()
    {
        npcMove = GetComponent<NPCMove>();
        myCanvas = transform.Find("Canvas").gameObject;
        AssignRandomState();
    }

    void AssignRandomState()
    {
        if (Random.Range(0, 2) == 0)
        {
            currentState = State.Stand;
            StartCoroutine(StandCoroutine());
        }
        else
        {
            currentState = State.Walk;
            AssignRandomDestination();
        }
    }

    IEnumerator StandCoroutine()
    {
        float standTime = Random.Range(minStandTime, maxStandTime);
        yield return new WaitForSeconds(standTime);
        AssignRandomState();
    }

    void AssignRandomDestination()
    {
        
        NPCSpawner spawner = FindObjectOfType<NPCSpawner>();
        destination = spawner.GetRandomSpawnPoint();
        npcMove.SetDestination(destination);
    }

    public void OnDestinationReached()
    {
        AssignRandomState();
    }

    void ShowCurTalkable()
    {
        myCanvas.SetActive(true);
    }

}
