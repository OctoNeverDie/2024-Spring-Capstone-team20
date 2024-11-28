using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimTest : MonoBehaviour
{
    
    NPC npc;
    //NPCLooks looks;
    private Rigidbody rb;

    Vector3 init_position;
    Quaternion init_rotation;

    void Start()
    {
        npc = GetComponent<NPC>();
        //looks = GetComponent<NPCLooks>();
        rb = GetComponent<Rigidbody>();

        init_position = transform.position;
        init_rotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            npc.PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.Idle);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            npc.PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.SlightlyPositive);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            npc.PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.Positive);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            npc.PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.VeryPositive);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            npc.PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.SlightlyNegative);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            npc.PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.Negative);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            npc.PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.VeryNegative);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            transform.position = init_position;
            transform.rotation = init_rotation;
            npc.PlayRandomNPCAnimByAnimType(NPCDefine.AnimType.Attack);
        }
    }
    

}
