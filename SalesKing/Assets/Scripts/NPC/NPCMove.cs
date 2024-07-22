using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPCMove : MonoBehaviour
{
    NPC _npc;
    public float speed = 0.5f;

    void Start()
    {
        _npc = GetComponent<NPC>();

    }

    void Update()
    {
        if(_npc.destination != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _npc.destination.position, speed * Time.deltaTime);
        }
    }
}
